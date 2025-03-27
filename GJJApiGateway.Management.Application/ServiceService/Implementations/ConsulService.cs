using GJJApiGateway.Management.Application.ServiceService.DTOs;
using GJJApiGateway.Management.Application.ServiceService.Interfaces;
using GJJApiGateway.Management.Application.Shared.DTOs;
using System.Linq;
using Consul;

namespace GJJApiGateway.Management.Application.ServiceService.Implementations;

public class ConsulService : IConsulService
{
    private readonly IConsulClient _consulClient;
    private const int RefreshIntervalSec = 5;

    public ConsulService(IConsulClient consulClient)
    {
        _consulClient = consulClient;
    }

    public async Task<ServiceResult<PageResult<A_ConsulServiceDto>>> GetServiceListAsync(
    string? serviceName, 
    string? status,
    int pageIndex, 
    int pageSize)
{
    try
    {
        // 获取所有服务
        var servicesResult = await _consulClient.Catalog.Services();
        var allServices = servicesResult.Response;

        // 修复Where条件括号问题
        var filtered = allServices?
            .Where(s => 
                string.IsNullOrEmpty(serviceName) || 
                s.Key.Contains(serviceName, StringComparison.OrdinalIgnoreCase))
            .Select(s => new A_ConsulServiceDto
            {
                ServiceId = s.Key,  // 使用正确的属性名
                ServiceName = s.Key,
                InstanceCount = s.Value?.Length ?? 0
            })
            .ToList() ?? new List<A_ConsulServiceDto>();

        // 获取详细信息
        var tasks = filtered.Select(async s => 
        {
            var health = await _consulClient.Health.Service(s.ServiceName);
            s.Instances = health.Response?
                .Select(h => new A_ServiceInstanceDto
                {
                    // 使用正确的属性路径
                    Node = h.Node?.Name ?? "Unknown",
                    Address = h.Service?.Address ?? "Unknown",
                    Port = h.Service?.Port ?? 0,
                    Status = h.Checks?.Any(c => c.Status == HealthStatus.Critical) ?? false 
                        ? "Unhealthy" 
                        : "Healthy"
                }).ToList() ?? new List<A_ServiceInstanceDto>();
            
            // 设置服务状态
            s.Status = s.Instances.Any(i => i.Status == "Healthy") 
                ? "Healthy" 
                : "Unhealthy";
            return s;
        });

        var detailedServices = (await Task.WhenAll(tasks)).ToList();

        // 应用状态过滤
        if (!string.IsNullOrEmpty(status))
        {
            detailedServices = detailedServices
                .Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 分页处理
        var paged = detailedServices
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return ServiceResult<PageResult<A_ConsulServiceDto>>.Success(
            new PageResult<A_ConsulServiceDto> 
            { 
                List = paged, 
                Total = detailedServices.Count 
            }, 
            "查询成功");
    }
    catch (Exception ex)
    {
        return ServiceResult<PageResult<A_ConsulServiceDto>>.Fail($"查询异常：{ex.Message}");
    }
}

    public async Task<ServiceResult<bool>> EnableServiceAsync(string serviceId)
    {
        try
        {
            // Consul启用服务逻辑（示例）
            await _consulClient.Agent.ServiceRegister(new AgentServiceRegistration
            {
                ID = serviceId,
                // 其他注册信息...
            });
            return ServiceResult<bool>.Success(true, "服务已启用");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Fail($"启用失败：{ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DisableServiceAsync(string serviceId)
    {
        try
        {
            await _consulClient.Agent.ServiceDeregister(serviceId);
            return ServiceResult<bool>.Success(true, "服务已停用");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Fail($"停用失败：{ex.Message}");
        }
    }
}
