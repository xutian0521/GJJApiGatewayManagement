using GJJApiGateway.Management.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResult<A_LoginResponseDto>> UserLoginAsync(string userName,
                string password, string code, string codeKey);
        ServiceResult<A_SysUserInfoDto> GetUserInfo(string token);
        dynamic GetValidateCode();
    }
}
