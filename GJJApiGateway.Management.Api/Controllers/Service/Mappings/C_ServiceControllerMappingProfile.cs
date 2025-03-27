using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Service.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.Mappings;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.ServiceService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.Service.Mappings;

public class C_ServiceControllerMappingProfile: Profile
{
    public C_ServiceControllerMappingProfile()
    {
        CreateMap<A_ConsulServiceDto, ConsulServiceVM>().ReverseMap();
        // 新增缺失的映射
        CreateMap<A_ServiceInstanceDto, ServiceInstanceVM>().ReverseMap();
        CreateMap<PageResult<A_ConsulServiceDto>, Pager<ConsulServiceVM>>().ReverseMap();

        CreateMap<PageResult<A_ConsulServiceDto>, Pager<ConsulServiceVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ConsulServiceDto, ConsulServiceVM>>();
    }


}