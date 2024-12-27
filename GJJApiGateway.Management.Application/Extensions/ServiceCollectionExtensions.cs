using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Application.Services;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using GJJApiGateway.Management.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GJJApiGateway.Management.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            return services;
        }

        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ManagementDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            // 注册其他仓储和基础设施服务

            return services;
        }

        public static IServiceCollection AddCommonModule(this IServiceCollection services)
        {
            // 注册通用服务，如日志记录
            services.AddLogging();
            return services;
        }
    }
}
