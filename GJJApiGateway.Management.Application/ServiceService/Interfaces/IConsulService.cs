using GJJApiGateway.Management.Application.ServiceService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Application.ServiceService.Interfaces;

public interface IConsulService
{
    Task<ServiceResult<PageResult<A_ConsulServiceDto>>> GetServiceListAsync(
        string? serviceName,
        string? status,
        int pageIndex,
        int pageSize);
    Task<ServiceResult<bool>> EnableServiceInstanceAsync(string instanceId);
    Task<ServiceResult<bool>> DisableServiceInstanceAsync(string instanceId);
}