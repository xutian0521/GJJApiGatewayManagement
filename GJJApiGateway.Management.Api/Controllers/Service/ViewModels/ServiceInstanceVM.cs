namespace GJJApiGateway.Management.Api.Controllers.Service.ViewModels;

public class ServiceInstanceVM
{
    public string Node { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Status { get; set; } = "Unknown";
}