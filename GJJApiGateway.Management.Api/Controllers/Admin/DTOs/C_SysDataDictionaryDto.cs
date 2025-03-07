namespace GJJApiGateway.Management.Api.Controllers.Admin.DTOs;

public class C_SysDataDictionaryDto
{
    /// <summary>
    /// id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// 父级id
    /// </summary>
    public int PId { get; set; }
    /// <summary>
    /// 字典key
    /// </summary>
    public string? DataKey { get; set; }
    /// <summary>
    /// 字典key别名
    /// </summary>
    public string? DataKeyAlias { get; set; }
    /// <summary>
    /// 字典值
    /// </summary>
    public string? DataValue { get; set; }
    /// <summary>
    /// 数据描述
    /// </summary>
    public string? DataDescription { get; set; }
    /// <summary>
    /// 排序号
    /// </summary>
    public int SortId { get; set; }
    /// <summary>
    /// 表记录创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
    /// <summary>
    /// 字典更新时间
    /// </summary>
    public DateTime UpdateTime { get; set; }
    /// <summary>
    /// 子字典
    /// </summary>
    public List<C_SysDataDictionaryDto>? Children { get; set; }  
}