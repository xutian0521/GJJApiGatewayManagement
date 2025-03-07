namespace GJJApiGateway.Management.Infrastructure.DTOs;

/// <summary>
/// 数据层分页结果 DTO
/// </summary>
public class DataPageResult<T>
{
    /// <summary>
    /// 分页数据列表
    /// </summary>
    public List<T> List { get; set; }
    /// <summary>
    /// 总记录数
    /// </summary>
    public int Total { get; set; }
}