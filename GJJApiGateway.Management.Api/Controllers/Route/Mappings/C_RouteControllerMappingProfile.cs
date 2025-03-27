using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Route.DTOs;
using GJJApiGateway.Management.Api.Controllers.Route.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.Mappings;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.RouteService.Constants;
using GJJApiGateway.Management.Application.RouteService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.Route.Mappings;

public class C_RouteControllerMappingProfile: Profile
{
    public C_RouteControllerMappingProfile()
    {
        CreateMap<A_ConsulRouteDto, ConsulRouteVM>().ReverseMap();
        CreateMap<J_HostAndPortsDto, HostAndPortsVM>().ReverseMap();
        CreateMap<A_ConsulRouteDto, C_AddRouteDto>().ReverseMap();

        
        CreateMap<PageResult<A_ConsulRouteDto>, Pager<ConsulRouteVM>>().ReverseMap();

        CreateMap<A_ConsulRouteDto, J_RouteConfigDto>()
            .ForMember(dest => dest.DownstreamHostAndPorts, 
                opt => opt.MapFrom(src => src.ServiceDiscoveryMode == ServiceDiscoveryModeConst.Static 
                    ? new List<J_HostAndPortsDto> { new() { Host = src.DownstreamHost, Port = src.DownstreamPort } } 
                    : null)).ReverseMap();
        
        
        CreateMap<PageResult<A_ConsulRouteDto>, Pager<ConsulRouteVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ConsulRouteDto, ConsulRouteVM>>();
    }

}