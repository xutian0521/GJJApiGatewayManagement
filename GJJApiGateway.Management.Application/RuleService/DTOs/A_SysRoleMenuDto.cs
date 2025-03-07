namespace GJJApiGateway.Management.Application.RuleService.DTOs;

/// <summary>
/// 角色菜单表
/// </summary>
public class A_SysRoleMenuDto
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 菜单 ID
    /// </summary>
    public int MenuId { get; set; }

    /// <summary>
    /// 是否有添加权限
    /// </summary>
    public int CanAdd { get; set; }

    /// <summary>
    /// 是否有编辑权限
    /// </summary>
    public int CanEdit { get; set; }

    /// <summary>
    /// 是否有删除权限
    /// </summary>
    public int CanDelete { get; set; }

    /// <summary>
    /// 是否有查看权限
    /// </summary>
    public int CanAudit { get; set; }
}
