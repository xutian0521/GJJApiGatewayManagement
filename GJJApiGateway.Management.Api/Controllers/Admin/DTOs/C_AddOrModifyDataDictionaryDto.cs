namespace GJJApiGateway.Management.Api.Controllers.Admin.DTOs;

public class C_AddOrModifyDataDictionaryDto
{
    public int id { get; set; }
    public string? dataKey { get; set; }
    public string? dataKeyAlias { get; set; }
    public int pId { get; set; }
    public string dataValue { get; set; } 
    public string dataDescription { get; set; } 
    public int sortId { get; set; }
}