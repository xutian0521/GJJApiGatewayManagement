using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.APIAuth.DTOs;
using GJJApiGateway.Management.Api.Controllers.APIAuth.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.Mappings;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.APIAuth.Mappings;

public class C_APIAuthControllerMappingProfile: Profile
{
    public C_APIAuthControllerMappingProfile()
    {
        CreateMap<A_ApiInfoDto, ApiInfoVM>().ReverseMap();
        CreateMap<A_ApiAuthorizationRequestDto, C_ApiAuthorizationRequestDto>().ReverseMap();
        CreateMap<A_ApiInfoDto, C_ApiInfoDto>().ReverseMap();
        CreateMap<A_ApiApplicationDto, C_ApiApplicationDto>().ReverseMap();
        CreateMap<A_ApiApplicationDto, ApiApplicationVM>().ReverseMap();
        
        CreateMap<PageResult<A_ApiApplicationDto>, Pager<ApiApplicationVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ApiApplicationDto, ApiApplicationVM>>();
        CreateMap<PageResult<A_ApiInfoDto>, Pager<ApiInfoVM>>()
            .ConvertUsing<PageResultToPagerConverter<A_ApiInfoDto, ApiInfoVM>>();
    }
    
}