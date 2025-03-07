using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface ISysUserInfoRepository
    {
        Task<SysUserInfo> GetUserByNameAsync(int? id, string userName);
        Task<int> UpdateAsync(SysUserInfo sysUserInfo);
        Task<SysUserInfo> FindByUsernameAsync(string userName);
        Task<bool> ExistsByUsernameAsync(string userName);

        Task<DataPageResult<SysUserInfo>> GetPagedUsersAsync(string userName, string roleId, int pageIndex,
            int pageSize);

        Task<int> InserAsync(SysUserInfo user);
        Task<int> DeleteAsync(SysUserInfo user);

    }
}
