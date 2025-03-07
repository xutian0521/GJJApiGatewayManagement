using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.RuleService.Commands;

public interface IRuleCommand
{
    Task<int> InsertRoleMenusAsync(List<A_SysRoleMenuDto> roleMenus);
    Task<int> DeleteRoleMenusAsync(List<A_SysRoleMenuDto> roleMenus);
    Task<int> InsertUserAsync(A_SysUserInfoDto userDto);
    Task<int> DeleteUserAsync(A_SysUserInfoDto userDto);
    Task<int> UpdateRoleAsync(A_SysRoleDto roleDto);
    Task<int> InsertRoleAsync(A_SysRoleDto roleDto);
    Task<int> DeleteRoleAsync(A_SysRoleDto roleDto);
    Task<int> InsertDataDictionaryAsync(A_SysDataDictionaryDto dataDictionaryDto);
    Task<int> UpdateDataDictionaryAsync(A_SysDataDictionaryDto dataDictionaryDto);
    Task<int> DeleteDataDictionaryAsync(A_SysDataDictionaryDto dataDictionaryDto);
}