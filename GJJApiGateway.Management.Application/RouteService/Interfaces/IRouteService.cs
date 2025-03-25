using GJJApiGateway.Management.Application.RouteService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Application.RouteService.Interfaces;

public interface IRouteService
{
    Task<ServiceResult<PageResult<A_ConsulRouteDto>>> GetConsulRouteListAsync(
        string? upstreamPathTemplate,
        string? serviceName,
        string? serviceDiscoveryMode,
        int pageIndex,
        int pageSize);
}