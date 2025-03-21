using GJJApiGateway.Management.Application.Extensions;
using GJJApiGateway.Management.Api.Extensions;
using GJJApiGateway.Management.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
string serviceName = "GJJApiGateway.Management.Api";

// 系统基础设施注册（配置中心、监控、日志、跨域、Consul）
IConfiguration configuration = builder.Configuration;
IConfigurationBuilder configurationBuilder = builder.Configuration;
builder.Services.AddSystemInfrastructure(configuration, configurationBuilder, serviceName);


// 业务层服务、仓储、AutoMapper
builder.Services.AddApplicationModule()
    .AddInfrastructureModule(builder.Configuration)
    .AddCommonModule()
    .AddApplicationAutoMapperProfiles();

// 控制器、过滤器、控制器AutoMapper
builder.Services.AddControllersAndFilters()
    .AddControllersAutoMapperProfiles();

var app = builder.Build();
app.UseCors("AppCors"); // 配置 CORS 策略

app.UseAuthentication(); // 启用认证中间件
app.UseAuthorization(); // 启用授权中间件
app.MapControllers(); // 映射控制器路由

app.Run(); // 运行应用程序