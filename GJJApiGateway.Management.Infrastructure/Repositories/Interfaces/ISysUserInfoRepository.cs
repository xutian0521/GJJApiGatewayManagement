using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface ISysUserInfoRepository
    {
        Task<SYS_USERINFO> GetUserByNameAsync(int? id, string userName);
        Task<int> UpdateAsync(SYS_USERINFO sysUserInfo);
    }
}
