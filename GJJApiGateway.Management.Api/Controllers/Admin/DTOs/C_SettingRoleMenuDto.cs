namespace GJJApiGateway.Management.Api.Controllers.Admin.DTOs;

/// <summary>
/// 设置角色权限菜单dto
/// </summary>
public class C_SettingRoleMenuDto
{
    /// <summary>
    /// 角色id
    /// </summary>
    public int roleId { get; set; }
    /// <summary>
    /// 菜单id集合
    /// </summary>
    public List<int> menuIds { get; set; }
}