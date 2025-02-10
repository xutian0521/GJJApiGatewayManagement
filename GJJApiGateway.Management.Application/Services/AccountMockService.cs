using System;
using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Application.Interfaces;

namespace GJJApiGateway.Management.Application.Services
{
    public class AccountMockService: IAccountService
    {
        public ServiceResult<A_LoginResponseDto> UserLogin(string username)
        {
            // 临时假数据
            return username switch
            {
                "admin" => new ServiceResult<A_LoginResponseDto>
                {
                    Code = 1,
                    Data = new A_LoginResponseDto { Token = "admin-token" }
                },
                "editor" => new ServiceResult<A_LoginResponseDto>
                {
                    Code = 1,
                    Data = new A_LoginResponseDto { Token = "editor-token" }
                },
                _ => new ServiceResult<A_LoginResponseDto>
                {
                    Code = 60204,
                    Message = "Account and password are incorrect."
                }
            };
        }

        public ServiceResult<A_UserInfoDto> GetUserInfo(string token)
        {
            // 临时假数据
            return token switch
            {
                "admin-token" => new ServiceResult<A_UserInfoDto>
                {
                    Code = 1,
                    Data = new A_UserInfoDto
                    {
                        Roles = new[] { "admin" },
                        Introduction = "I am a super administrator",
                        Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                        Name = "Super Admin"
                    }
                },
                "editor-token" => new ServiceResult<A_UserInfoDto>
                {
                    Code = 1,
                    Data = new A_UserInfoDto
                    {
                        Roles = new[] { "editor" },
                        Introduction = "I am an editor",
                        Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                        Name = "Normal Editor"
                    }
                },
                _ => new ServiceResult<A_UserInfoDto>
                {
                    Code = 50008,
                    Message = "Login failed, unable to get user details."
                }
            };
        }
    }
}