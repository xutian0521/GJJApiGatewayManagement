using System.Text;
using AutoMapper;
using Consul;
using GJJApiGateway.Management.Application.RouteService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;
using System.Text.Json;
using System.Text.Json.Serialization;
using GJJApiGateway.Management.Application.RouteService.Constants;
using GJJApiGateway.Management.Application.RouteService.Interfaces;
using GJJApiGateway.Management.Common.Extensions;

public class ConsulRouteService : IRouteService
{
    private readonly IConsulClient _consulClient;
    private readonly IMapper _mapper;

    public ConsulRouteService(IMapper mapper, IConsulClient consulClient)
    {
        _mapper = mapper;
        _consulClient = consulClient;
    }

    public async Task<ServiceResult<PageResult<A_ConsulRouteDto>>> GetConsulRouteListAsync(
        string? upstreamPathTemplate, 
        string? serviceName, 
        string? serviceDiscoveryMode,
        int pageIndex, 
        int pageSize)
    {

        // 1. 从 Consul KV 获取 ocelot/routes 的值
        var kvResult = await _consulClient.KV.Get("ocelot/routes");
        
        if (kvResult.Response == null)
            return ServiceResult<PageResult<A_ConsulRouteDto>>.Fail("未找到路由配置");

        // 2. 解码 Base64 值
        var jsonString = Encoding.UTF8.GetString(kvResult.Response.Value);

        // 3. 反序列化为动态对象（根据实际路由结构定义DTO）
        var routesData = JsonSerializer.Deserialize<A_OcelotRoutesConfigDto>(jsonString);

        // 4. 转换为业务DTO并应用过滤
        var allRoutes = _mapper.Map<List<A_ConsulRouteDto>>(routesData?.Routes ?? new List<A_RouteConfigDto>());
        foreach (var item in allRoutes)
        {
            item.Id = (allRoutes.IndexOf(item) + 1).ToString();
            item.UpstreamHttpMethodDisplay = string.Join(" | ", item.UpstreamHttpMethod);
            if (item.DownstreamHostAndPorts.Count > 0)
            {
                item.DownstreamHostAndPortsDisplay = item.DownstreamHostAndPorts[0].Host + ":" + item.DownstreamHostAndPorts[0].Port;
                item.DownstreamHost = item.DownstreamHostAndPorts[0].Host;
                item.DownstreamPort = item.DownstreamHostAndPorts[0].Port;
                item.ServiceDiscoveryMode = ServiceDiscoveryModeConst.Static;
                item.ServiceDiscoveryModeDisplay = "静态的";

            }
            else
            {
                item.ServiceDiscoveryMode = ServiceDiscoveryModeConst.Discovered;
                item.ServiceDiscoveryModeDisplay = "服务发现";
            }
        }
        var filteredRoutes = allRoutes
            .WhereIf(!string.IsNullOrEmpty(upstreamPathTemplate), r => r.UpstreamPathTemplate?.Contains(upstreamPathTemplate) == true)
            .WhereIf(!string.IsNullOrEmpty(serviceName), r => r.ServiceName?.Contains(serviceName) == true)
            .WhereIf(!string.IsNullOrEmpty(serviceDiscoveryMode), r => r.ServiceDiscoveryMode?.Equals(serviceDiscoveryMode) == true)
            .ToList();

        // 5. 内存分页处理
        var total = filteredRoutes.Count;
        var pagedRoutes = filteredRoutes
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return ServiceResult<PageResult<A_ConsulRouteDto>>.Success(
            new PageResult<A_ConsulRouteDto>
            {
                List = pagedRoutes,
                Total = total
            }, 
            "查询成功");
        

    }
}