using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Route.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.RouteService.Interfaces;
using GJJApiGateway.Management.Common.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.Route;

[SkipApplicationIdValidation]
[Route("api/[controller]")]
[ApiController]
public class RouteController : ControllerBase
{
    private readonly IRouteService _routeService;
    private readonly IMapper _mapper;

    public RouteController(IRouteService routeService, IMapper mapper)
    {
        _routeService = routeService;
        _mapper = mapper;
    }

    [HttpGet("List")]
    public async Task<s_ApiResult<Pager<ConsulRouteVM>>> GetConsulRouteListAsync(
        string? upstreamPathTemplate,
        string? serviceName,
        string? serviceDiscoveryMode,
        int pageIndex = 1, 
        int pageSize = 10)
    {
        var result = await _routeService.GetConsulRouteListAsync(upstreamPathTemplate, serviceName, serviceDiscoveryMode, pageIndex, pageSize);
        
        // 添加空值检查
        if (result?.Data == null)
        {
            return new s_ApiResult<Pager<ConsulRouteVM>>(
                result?.Code ?? 500, 
                result?.Message ?? "数据加载失败", 
                new Pager<ConsulRouteVM> { List = new List<ConsulRouteVM>(), Total = 0 }
            );
        }
        
        var pager = _mapper.Map<Pager<ConsulRouteVM>>(result.Data);
        return new s_ApiResult<Pager<ConsulRouteVM>>(result.Code, result.Message, pager);
    }
    
}