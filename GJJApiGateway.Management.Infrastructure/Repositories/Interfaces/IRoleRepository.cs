using GJJApiGateway.Management.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<SysRole> GetRoleById(int id);

        Task<DataPageResult<SysRole>> GetPagedRolesAsync(string roleName, int pageIndex, int pageSize);

        Task<int> UpdateRoleAsync(SysRole role);
        Task<int> InsertRoleAsync(SysRole role);
        Task<int> DeleteRoleAsync(SysRole role);
    }
}
