using AutoMapper;
using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Common.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<Authorization, AuthorizationViewModel>()
                .ForMember(
                    dest => dest.AllowedEndpoints,
                    opt => opt.MapFrom(src => src.AllowedEndpoints.Split(new[] { ',' }, StringSplitOptions.None).ToList())
                );

            CreateMap<AuthorizationViewModel, Authorization>()
                .ForMember(
                    dest => dest.AllowedEndpoints,
                    opt => opt.MapFrom(src => string.Join(",", src.AllowedEndpoints))
                );

            CreateMap<CreateAuthorizationDto, Authorization>();
            CreateMap<UpdateAuthorizationDto, Authorization>();
            

            CreateMap<ApiInfo, A_ApiInfoDto>().ReverseMap();

            CreateMap<ApiApplication, A_ApiApplicationDto>().ReverseMap();
            CreateMap<A_ApiConfigurationDto, ApiInfo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ApiInfo, CommonApiInfoDto>().ReverseMap();
            CreateMap<SysUserInfo, A_SysUserInfoDto>().ReverseMap();
            CreateMap<A_SysMenuDto, SysMenu>().ReverseMap();

        }
    }
}
