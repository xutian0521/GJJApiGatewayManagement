using GJJApiGateway.Management.Application.APIAuthService.Commands;
using GJJApiGateway.Management.Application.APIAuthService.Implementations;
using GJJApiGateway.Management.Application.APIAuthService.Interfaces;
using GJJApiGateway.Management.Application.APIAuthService.Mappings;
using GJJApiGateway.Management.Application.APIAuthService.Queries;
using GJJApiGateway.Management.Application.RouteService.Interfaces;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using GJJApiGateway.Management.Infrastructure;
using GJJApiGateway.Management.Infrastructure.Configuration;
using GJJApiGateway.Management.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GJJApiGateway.Management.Application.Extensions
{
    /// <summary>
    /// 服务集合扩展方法类，定义服务注册的扩展方法。
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 向服务集合中添加应用层模块的服务。
        /// </summary>
        /// <param name="services">服务集合实例。</param>
        /// <returns>配置后的服务集合。</returns>
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            // 注册授权服务接口及其实现
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IApiApplicationService, ApiApplicationService>();
            services.AddScoped<IRouteService, ConsulRouteService>();
            services.AddScoped<PasswordSettings, PasswordSettings>();
            services.AddScoped<IAuthQuery, AuthQuery>();
            services.AddScoped<IAuthCommand, AuthCommand>();
            services.AddScoped<ApiManageService>();

            return services;
        }

        /// <summary>
        /// 向服务集合中添加基础设施层模块的服务。
        /// </summary>
        /// <param name="services">服务集合实例。</param>
        /// <param name="configuration">配置接口，用于获取配置项。</param>
        /// <returns>配置后的服务集合。</returns>
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            // 配置数据库上下文，使用 SQL Server 数据库
            services.AddDbContext<ManagementDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // 注册其他仓储和基础设施服务
            services.AddScoped<IApiApplicationRepository, ApiApplicationRepository>();
            services.AddScoped<IApiApplicationMappingRepository, ApiApplicationMappingRepository>();
            services.AddScoped<IApiInfoRepository, ApiInfoRepository>();

            return services;
        }

        /// <summary>
        /// 向服务集合中添加通用模块的服务。
        /// </summary>
        /// <param name="services">服务集合实例。</param>
        /// <returns>配置后的服务集合。</returns>
        public static IServiceCollection AddCommonModule(this IServiceCollection services)
        {
            // 注册通用服务，如日志记录
            services.AddLogging();
            return services;
        }
        
        /// <summary>
        /// 添加 AutoMapper 映射配置到服务集合。
        /// </summary>
        /// <param name="services">服务集合实例。</param>
        /// <returns>配置后的服务集合。</returns>
        public static IServiceCollection AddApplicationAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(A_APIAuthServiceMappingProfile)
            );

            return services;
        }
    }
}
