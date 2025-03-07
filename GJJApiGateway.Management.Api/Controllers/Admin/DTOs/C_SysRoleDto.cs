namespace GJJApiGateway.Management.Api.Controllers.Admin.DTOs;

/// <summary>
/// 用户角色dto
/// </summary>
public class C_SysRoleDto
{
    /// <summary>
    /// ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// 排序号
    /// </summary>
    public string? SortId { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}