namespace GJJApiGateway.Management.Application.RouteService.DTOs;

public class A_RouteConfigDto
{
    public int Id { get; set; }  // 新增Id字段
    public string? ServiceName { get; set; }
    public string? DownstreamPathTemplate { get; set; }
    public string? UpstreamPathTemplate { get; set; }
    public string DownstreamScheme { get; set; }
    public List<string> UpstreamHttpMethod { get; set; }
    public List<A_HostAndPortsDto> DownstreamHostAndPorts { get; set; }
}