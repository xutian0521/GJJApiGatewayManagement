namespace GJJApiGateway.Management.Application.RouteService.DTOs;

// 辅助类定义（需根据实际Consul存储的JSON结构调整）
public class A_OcelotRoutesConfigDto
{
    public List<A_RouteConfigDto> Routes { get; set; } = new();
}
