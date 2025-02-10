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
        ServiceResult<A_LoginResponseDto> UserLogin(string username);
        ServiceResult<A_UserInfoDto> GetUserInfo(string token);
    }
}
