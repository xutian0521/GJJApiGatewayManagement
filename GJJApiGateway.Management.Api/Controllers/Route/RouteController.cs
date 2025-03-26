using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Route.DTOs;
using GJJApiGateway.Management.Api.Controllers.Route.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.RouteService.DTOs;
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
        var pager = _mapper.Map<Pager<ConsulRouteVM>>(result.Data);
        return new s_ApiResult<Pager<ConsulRouteVM>>(result.Code, result.Message, pager);
    }
    
    [HttpPost("AddRoute")]
    public async Task<s_ApiResult<string>> AddRoute([FromBody] C_AddRouteDto route)
    {
        var dto = _mapper.Map<A_ConsulRouteDto>(route);
        var result = await _routeService.AddRouteAsync(dto);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }

    [HttpPost("UpdateRoute")]
    public async Task<s_ApiResult<string>> UpdateRoute([FromBody] C_AddRouteDto route)
    {
        var dto = _mapper.Map<A_ConsulRouteDto>(route);
        var result = await _routeService.UpdateRouteAsync(dto);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }

    [HttpGet("DeleteRoute")]
    public async Task<s_ApiResult<string>> DeleteRoute(int id)
    {
        var result = await _routeService.DeleteRouteAsync(id);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }
}