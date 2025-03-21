using AutoMapper;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.APIAuthService.Mappings;

public class A_APIAuthServiceMappingProfile: Profile
{
    public A_APIAuthServiceMappingProfile()
    {
        CreateMap<ApiInfo, A_ApiInfoDto>().ReverseMap();

        CreateMap<ApiApplication, A_ApiApplicationDto>().ReverseMap();
        CreateMap<ApiInfo, CommonApiInfoDto>().ReverseMap();
    }
}