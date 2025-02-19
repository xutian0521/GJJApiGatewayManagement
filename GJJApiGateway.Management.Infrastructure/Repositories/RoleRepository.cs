using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ManagementDbContext _context;

        public RoleRepository(ManagementDbContext context)
        {
            _context = context;
        }

        //public async Task<D_SYS_ROLE> GetRoleById(int id)
        //{
        //    return await _context.D_SYS_ROLE.FindAsync(id);
        //}

        //public async Task<List<D_SYS_ROLE>> GetRoles(string roleName, int pageIndex, int pageSize)
        //{
        //    var query = _context.D_SYS_ROLE.AsQueryable();
        //    if (!string.IsNullOrEmpty(roleName))
        //        query = query.Where(r => r.ROLENAME.Contains(roleName));

        //    return await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        //}

        //public async Task<(int code, string message)> AddOrModifyRole(int id, string roleName, string remark)
        //{
        //    var role = await _context.D_SYS_ROLE.FindAsync(id);
        //    if (role != null)
        //    {
        //        role.ROLENAME = roleName;
        //        role.REMARK = remark;

        //        await _context.SaveChangesAsync();
        //        return (ApiResultCodeConst.SUCCESS, "角色修改成功");
        //    }
        //    else
        //    {
        //        var newRole = new D_SYS_ROLE
        //        {
        //            ROLENAME = roleName,
        //            REMARK = remark
        //        };

        //        await _context.D_SYS_ROLE.AddAsync(newRole);
        //        await _context.SaveChangesAsync();
        //        return (ApiResultCodeConst.SUCCESS, "角色新增成功");
        //    }
        //}

        //public async Task<(int code, string message)> DeleteRole(int id)
        //{
        //    var role = await _context.D_SYS_ROLE.FindAsync(id);
        //    if (role == null)
        //        return (ApiResultCodeConst.ERROR, "角色不存在");

        //    _context.D_SYS_ROLE.Remove(role);
        //    await _context.SaveChangesAsync();

        //    return (ApiResultCodeConst.SUCCESS, "角色删除成功");
        //}
    }

}
