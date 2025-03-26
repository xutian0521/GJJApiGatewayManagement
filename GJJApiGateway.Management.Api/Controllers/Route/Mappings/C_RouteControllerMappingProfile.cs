using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Route.DTOs;
using GJJApiGateway.Management.Api.Controllers.Route.ViewModels;
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
    /// <summary>
    /// 将业务层分页 DTO (PageResult<TSource>) 转换为控制器层分页 ViewModel (Pager<TDestination>)，
    /// 使用 AutoMapper 将 TSource 转换为 TDestination。
    /// </summary>
    public class PageResultToPagerConverter<TSource, TDestination> : ITypeConverter<PageResult<TSource>, Pager<TDestination>>
    {
        public Pager<TDestination> Convert(PageResult<TSource> source, Pager<TDestination> destination, ResolutionContext context)
        {
            var list = context.Mapper.Map<IEnumerable<TDestination>>(source.List);
            return new Pager<TDestination>
            {
                List = list.ToList(),
                Total = source.Total
            };
        }
    }
}