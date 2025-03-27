using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Service.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.ServiceService.Interfaces;
using GJJApiGateway.Management.Common.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.Service;

[SkipApplicationIdValidation]
[Route("api/[controller]")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IConsulService _consulService;
    private readonly IMapper _mapper;

    public ServiceController(IConsulService consulService, IMapper mapper)
    {
        _consulService = consulService;
        _mapper = mapper;
    }

    [HttpGet("List")]
    public async Task<s_ApiResult<Pager<ConsulServiceVM>>> GetServiceListAsync(
        string? serviceName,
        string? status,
        int pageIndex = 1, 
        int pageSize = 10)
    {
        var result = await _consulService.GetServiceListAsync(serviceName, status, pageIndex, pageSize);
        var pager = _mapper.Map<Pager<ConsulServiceVM>>(result.Data);
        return new s_ApiResult<Pager<ConsulServiceVM>>(result.Code, result.Message, pager);
    }

    [HttpPost("EnableService")]
    public async Task<s_ApiResult<string>> EnableService(string serviceId)
    {
        var result = await _consulService.EnableServiceAsync(serviceId);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }

    [HttpPost("DisableService")]
    public async Task<s_ApiResult<string>> DisableService(string serviceId)
    {
        var result = await _consulService.DisableServiceAsync(serviceId);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }
}