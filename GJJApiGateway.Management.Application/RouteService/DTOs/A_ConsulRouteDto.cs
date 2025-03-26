using System.Text.Json.Serialization;

namespace GJJApiGateway.Management.Application.RouteService.DTOs;

// 路由配置DTO（根据你的业务需求调整）
public class A_ConsulRouteDto
{
    /// <summary>
    /// 路由唯一标识（如无唯一ID可忽略）
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 路由名称（通常用上游路径模板标识）
    /// </summary>
    public string? RouteName { get; set; }

    /// <summary>
    /// 服务名称（对应下游服务）
    /// </summary>
    public string? ServiceName { get; set; }

    /// <summary>
    /// 下游路径模板（示例：/api/{everything}）
    /// </summary>
    public string? DownstreamPathTemplate { get; set; }

    /// <summary>
    /// 下游协议（http/https）
    /// </summary>
    public string? DownstreamScheme { get; set; }

    /// <summary>
    /// 上游路径模板（示例：/gateway/service/{everything}）
    /// </summary>
    public string? UpstreamPathTemplate { get; set; }

    /// <summary>
    /// 支持的HTTP方法（GET/POST等）
    /// </summary>
    public List<string>? UpstreamHttpMethod { get; set; }
    

    /// <summary>
    /// 支持的HTTP方法（GET/POST等）Display
    /// </summary>
    public string? UpstreamHttpMethodDisplay { get; set; }
    
    public List<J_HostAndPortsDto> DownstreamHostAndPorts { get; set; }
    /// <summary>
    /// 下游服务地址和端口号
    /// </summary>
    public string? DownstreamHostAndPortsDisplay { get; set; }
    
    /// <summary>
    /// 下游服务地址
    /// </summary>
    public string? DownstreamHost { get; set; }
    /// <summary>
    /// 下游服务端口号
    /// </summary>
    public int? DownstreamPort { get; set; }

    /// <summary>
    /// 下游服务主机（从ServiceDiscovery获取）
    /// </summary>
    public string? ServiceHost { get; set; }

    /// <summary>
    /// 下游服务端口（从ServiceDiscovery获取）
    /// </summary>
    public int? ServicePort { get; set; }

    /// <summary>
    /// 路由状态（可扩展启停用功能）
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 最后更新时间（可扩展审计功能）
    /// </summary>
    public DateTime LastUpdated { get; set; } 
    
    /// <summary>
    /// 用于区分服务获取方式，即通过服务名称进行动态发现（如 Service Discovery），或通过 DownstreamHostAndPorts 中指定的静态地址和端口进行配置。
    /// 服务发现方式: Discovered：表示通过服务名称发现服务。Static：表示通过硬编码的地址和端口进行服务访问。
    /// </summary>
    public string? ServiceDiscoveryMode { get; set; }
    /// <summary>
    /// 服务发现方式Display 
    /// </summary>
    public string? ServiceDiscoveryModeDisplay { get; set; }

}