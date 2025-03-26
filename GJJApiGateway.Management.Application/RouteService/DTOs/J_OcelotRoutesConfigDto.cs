namespace GJJApiGateway.Management.Application.RouteService.DTOs;

// 辅助类定义（需根据实际Consul存储的JSON结构调整）
public class J_OcelotRoutesConfigDto
{
    public List<J_RouteConfigDto> Routes { get; set; } = new();
}
