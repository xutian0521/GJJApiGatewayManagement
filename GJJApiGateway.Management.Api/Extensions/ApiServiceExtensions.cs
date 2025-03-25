using GJJApiGateway.Management.Api.Controllers.APIAuth.Mappings;
using GJJApiGateway.Management.Api.Controllers.Route.Mappings;
using GJJApiGateway.Management.Api.Filter;
using Newtonsoft.Json;

namespace GJJApiGateway.Management.Api.Extensions;

/// <summary>
/// 控制器、过滤器、控制器AutoMapper
/// </summary>
public static class ApiServiceExtensions
{
    /// <summary>
    /// 注册控制器服务及过滤器。
    /// </summary>
    /// <param name="services">服务集合实例。</param>
    /// <returns>配置后的服务集合。</returns>
    public static IServiceCollection AddControllersAndFilters(this IServiceCollection services)
    {
        services.AddControllers(options =>
            {
                // 用户授权过滤器
                options.Filters.Add<UserAuthorizeFilter>();
                // AES加密操作过滤器 (按需启用)
                // options.Filters.Add<AESEncryptionActionFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                // 日期序列化格式
                options.SerializerSettings.Converters.Add(new FixedDateTimeConverter());
                // 空值处理策略
                // 我们将 NullValueHandling 设置为 Ignore，表示在序列化时忽略 null 值。如果您希望将 null 值序列化为 JSON 字符串 "null"，则可以将其设置为 Include。
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            });

        return services;
    }
        
    /// <summary>
    /// 添加业务层 AutoMapper 映射配置到服务集合。
    /// </summary>
    public static IServiceCollection AddControllersAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(C_APIAuthControllerMappingProfile),
            typeof(C_RouteControllerMappingProfile)
            
        );

        return services;
    }
}