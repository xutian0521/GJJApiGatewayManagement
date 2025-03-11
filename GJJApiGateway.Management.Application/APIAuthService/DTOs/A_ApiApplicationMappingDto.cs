namespace GJJApiGateway.Management.Application.APIAuthService.DTOs;

public class A_ApiApplicationMappingDto
{
    /// <summary>
    /// 主键，自增
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 外键，指向 ApiInfo 表中的 Id，表示关联的 API
    /// </summary>
    public int ApiId { get; set; }
    /// <summary>
    /// 外键，指向 ApiApplication 表中的 Id，表示关联的应用
    /// </summary>
    public int ApplicationId { get; set; }
    /// <summary>
    /// 授权时长（可选），单位为分钟
    /// </summary>
    public int? AuthorizationDuration { get; set; }
}