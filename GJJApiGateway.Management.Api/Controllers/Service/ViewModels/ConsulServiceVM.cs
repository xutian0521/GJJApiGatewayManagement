using AutoMapper;

namespace GJJApiGateway.Management.Api.Controllers.Service.ViewModels;

public class ConsulServiceVM
{
    public string ServiceId { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public int InstanceCount { get; set; }
    public string Status { get; set; } = "Unknown"; // 添加Status属性
    public string[]? Tags { get; set; }
    public string TagsDisplay { get; set; }
    public List<ServiceInstanceVM> Instances { get; set; } = new();
}