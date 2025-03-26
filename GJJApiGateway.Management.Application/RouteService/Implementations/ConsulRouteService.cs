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
    private const string RouteCounterKey = "ocelot/routes";

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
        var kvResult = await _consulClient.KV.Get(RouteCounterKey);
        
        if (kvResult.Response == null)
            return ServiceResult<PageResult<A_ConsulRouteDto>>.Fail("未找到路由配置");

        // 2. 解码 Base64 值
        var jsonString = Encoding.UTF8.GetString(kvResult.Response.Value);

        // 3. 反序列化为动态对象（根据实际路由结构定义DTO）
        var routesData = JsonSerializer.Deserialize<J_OcelotRoutesConfigDto>(jsonString);

        // 4. 转换为业务DTO并应用过滤
        var allRoutes = _mapper.Map<List<A_ConsulRouteDto>>(routesData?.Routes ?? new List<J_RouteConfigDto>());
        foreach (var item in allRoutes)
        {
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
    
    public async Task<ServiceResult<bool>> AddRouteAsync(A_ConsulRouteDto route)
    {
        try
        {
            var kvPair = await _consulClient.KV.Get(RouteCounterKey);
            var config = kvPair.Response == null 
                ? new J_OcelotRoutesConfigDto() 
                : JsonSerializer.Deserialize<J_OcelotRoutesConfigDto>(
                    Encoding.UTF8.GetString(kvPair.Response.Value));

            // 生成ID
            route.Id = GetNextRouteId(config.Routes);

            // 检查路由是否已存在（根据上游路径模板和方法）
            if (config.Routes.Any(r => 
                    r.UpstreamPathTemplate == route.UpstreamPathTemplate && 
                    r.UpstreamHttpMethod.SequenceEqual(route.UpstreamHttpMethod)))
            {
                return ServiceResult<bool>.Fail("路由已存在");
            }

            var newRoute = _mapper.Map<J_RouteConfigDto>(route);
            if (route.ServiceDiscoveryMode == ServiceDiscoveryModeConst.Static)
            {
                newRoute.DownstreamHostAndPorts =
                [
                    new J_HostAndPortsDto() { Host = route.DownstreamHost, Port = route.DownstreamPort }
                ];
                newRoute.ServiceName = null;
            }
            else
            {
                newRoute.DownstreamHostAndPorts = null;
            }
            config.Routes.Add(newRoute);

            var putResult = await SaveRoutesToConsul(config, kvPair.Response?.ModifyIndex ?? 0);
            return putResult.Response 
                ? ServiceResult<bool>.Success(true, "添加成功") 
                : ServiceResult<bool>.Fail("添加失败");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Fail($"添加异常：{ex.Message}");
        }
    }
    

    public async Task<ServiceResult<bool>> UpdateRouteAsync(A_ConsulRouteDto route)
    {
        try
        {
            var kvPair = await _consulClient.KV.Get(RouteCounterKey);
            if (kvPair.Response == null)
                return ServiceResult<bool>.Fail("路由配置不存在");

            var config = JsonSerializer.Deserialize<J_OcelotRoutesConfigDto>(Encoding.UTF8.GetString(kvPair.Response.Value));
            
            // 改为用ID查找
            var existingRoute = config.Routes.FirstOrDefault(r => r.Id == route.Id);
            if (existingRoute == null)
                return ServiceResult<bool>.Fail("要修改的路由不存在");

            _mapper.Map(route, existingRoute);
            
            if (route.ServiceDiscoveryMode == ServiceDiscoveryModeConst.Static)
            {
                existingRoute.DownstreamHostAndPorts =
                [
                    new J_HostAndPortsDto() { Host = route.DownstreamHost, Port = route.DownstreamPort }
                ];
                existingRoute.ServiceName = null;
            }
            else
            {
                existingRoute.DownstreamHostAndPorts = null;
            }
            
            var putResult = await SaveRoutesToConsul(config, kvPair.Response.ModifyIndex);
            return putResult.Response 
                ? ServiceResult<bool>.Success(true, "修改成功") 
                : ServiceResult<bool>.Fail("修改失败，可能已被其他人修改");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Fail($"修改异常：{ex.Message}");
        }
    }

    public async Task<ServiceResult<bool>> DeleteRouteAsync(int id)
    {
        try
        {
            var kvPair = await _consulClient.KV.Get(RouteCounterKey);
            if (kvPair.Response == null)
                return ServiceResult<bool>.Fail("路由配置不存在");

            var config = JsonSerializer.Deserialize<J_OcelotRoutesConfigDto>(
                Encoding.UTF8.GetString(kvPair.Response.Value));
        
            var routeToRemove = config.Routes.FirstOrDefault(r => r.Id == id);
            if (routeToRemove == null)
                return ServiceResult<bool>.Fail("要删除的路由不存在");

            config.Routes.Remove(routeToRemove);

            var putResult = await SaveRoutesToConsul(config, kvPair.Response.ModifyIndex);
            return putResult.Response 
                ? ServiceResult<bool>.Success(true, "删除成功") 
                : ServiceResult<bool>.Fail("删除失败，可能已被其他人修改");
        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Fail($"删除异常：{ex.Message}");
        }
    }

    private async Task<WriteResult<bool>> SaveRoutesToConsul(J_OcelotRoutesConfigDto config, ulong modifyIndex)
    {
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        var jsonBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(config, jsonOptions));
        
        var putRequest = new KVPair(RouteCounterKey)
        {
            Value = jsonBytes,
            ModifyIndex = modifyIndex // 使用CAS机制
        };

        return await _consulClient.KV.Put(putRequest);
    }
    
    private int GetNextRouteId(List<J_RouteConfigDto> existingRoutes)
    {
        if (!existingRoutes.Any()) return 1;
        return existingRoutes.Max(r => r.Id) + 1;
    }

}