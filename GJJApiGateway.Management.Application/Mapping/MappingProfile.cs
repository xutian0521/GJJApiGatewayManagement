using AutoMapper;
using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
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
        }
    }
}
