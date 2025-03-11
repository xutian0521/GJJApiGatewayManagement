using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Account.DTOs;
using GJJApiGateway.Management.Api.Controllers.Account.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;

namespace GJJApiGateway.Management.Api.Controllers.Account.Mappings;

public class C_AccountControllerMappingProfile: Profile
{
    public C_AccountControllerMappingProfile()
    {
        CreateMap<A_LoginRequestDto, C_LoginRequestDto>().ReverseMap();
        CreateMap<A_LoginResponseDto, LoginResponseVM>().ReverseMap();
        CreateMap<A_SysUserInfoDto, SysUserInfoVM>().ReverseMap();
    }
}