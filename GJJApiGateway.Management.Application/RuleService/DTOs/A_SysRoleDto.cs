namespace GJJApiGateway.Management.Application.RuleService.DTOs;

/// <summary>
/// 业务层角色表dto
/// </summary>
public class A_SysRoleDto
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