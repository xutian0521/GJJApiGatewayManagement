namespace GJJApiGateway.Management.Application.ServiceService.DTOs;

public class A_ServiceInstanceDto
{
    public string InstanceId { get; set; } = string.Empty;  // 新增实例唯一标识
    public string Node { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Status { get; set; } = "Unknown";
}