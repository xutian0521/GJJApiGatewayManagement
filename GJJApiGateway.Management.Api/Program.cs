
using GJJApiGateway.Management.Application.Extensions;
using GJJApiGateway.Management.Infrastructure.Repositories;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using Consul;
using System.Net;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Data;
using GJJApiGateway.Management.Api.Controllers.Account.Mappings;
using GJJApiGateway.Management.Api.Controllers.Admin.Mappings;
using GJJApiGateway.Management.Api.Controllers.APIAuth.Mappings;
using Microsoft.Data.SqlClient;
using GJJApiGateway.Management.Api.Filter;
using GJJApiGateway.Management.Application.AccountService.Implementations;
using GJJApiGateway.Management.Application.AccountService.Interfaces;
using GJJApiGateway.Management.Application.AccountService.Mappings;
using GJJApiGateway.Management.Application.APIAuthService.Implementations;
using GJJApiGateway.Management.Application.APIAuthService.Interfaces;
using GJJApiGateway.Management.Application.APIAuthService.Mappings;
using GJJApiGateway.Management.Application.RuleService.Implementations;
using GJJApiGateway.Management.Application.RuleService.Interfaces;
using GJJApiGateway.Management.Application.RuleService.Mappings;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

#region 配置 Apollo 分布式配置中心

// 配置 Apollo 分布式配置中心
// Apollo 是一个分布式的配置管理系统，能够动态地为应用程序提供配置数据，支持配置的实时更新。
// 使用 AddApollo 方法，将 Apollo 配置集成到 ASP.NET Core 配置系统中。
// 通过 builder.Configuration.GetSection("apollo") 获取配置节点，并传递给 AddApollo 方法。
builder.Configuration.AddApollo(builder.Configuration.GetSection("apollo"));

//默认地址
string _consulHost = "http://192.168.2.188:8500--";
string _jaegerHostgRPC = "http://192.168.2.188:4317--";
string _jaegerHostHTTP = "http://192.168.2.188:4318--";
string _seqHostHTTP = "http://192.168.2.188:5341/ingest/otlp/v1/traces--";
string _seqHostLog = "http://192.168.2.188:5341/ingest/otlp/v1/logs--";
// 允许所有地址访问，支持不同服务器部署
var _hostIP = "192.168.2.103";
//读取配置中心
_consulHost = builder.Configuration["ConsulHost"];
_jaegerHostgRPC = builder.Configuration["JaegerHostgRPC"];
_jaegerHostHTTP = builder.Configuration["JaegerHostHTTP"];
_seqHostHTTP = builder.Configuration["SeqHostHTTP"];
_seqHostLog = builder.Configuration["SeqHostLog"];
_hostIP = builder.Configuration["LocalIP"];
int _hostPort = 5043;
#endregion





Console.WriteLine($"当前主机 IP: {_hostIP}:{_hostPort}");

#region 配置 CORS

// 添加 CORS 服务并定义一个策略，从配置文件中读取允许的来源
builder.Services.AddCors(options => options.AddPolicy("AppCors", policy =>
{
    var hosts = builder.Configuration.GetValue<string>("AppHosts");
    Console.WriteLine(hosts);
    policy.WithOrigins(hosts.Split(','))
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
}));

#endregion

// 添加控制器服务到容器
builder.Services.AddControllers();

// 配置 AutoMapper，指定映射配置文件所在的程序集


builder.Services.AddAutoMapper(typeof(C_AccountControllerMappingProfile));
builder.Services.AddAutoMapper(typeof(C_APIAuthControllerMappingProfile));
builder.Services.AddAutoMapper(typeof(C_AdminControllerMappingProfile));

builder.Services.AddAutoMapper(typeof(A_AccountServiceMappingProfile));
builder.Services.AddAutoMapper(typeof(A_APIAuthServiceMappingProfile));
builder.Services.AddAutoMapper(typeof(A_RuleServiceMappingProfile));


// 注册应用层模块，包括认证、限流和路径管理
builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddCommonModule();

// 注册业务层数据库等
builder.Services.AddScoped<IApiApplicationService, ApiApplicationService>();
builder.Services.AddScoped<IApiApplicationRepository, ApiApplicationRepository>();
builder.Services.AddScoped<IApiApplicationMappingRepository, ApiApplicationMappingRepository>();

builder.Services.AddScoped<ApiManageService>();
builder.Services.AddScoped<IApiInfoRepository, ApiInfoRepository>();
builder.Services.AddScoped<IAccountService, AccountMockService>();
builder.Services.AddScoped<ISysUserInfoRepository, SysUserInfoRepository>();
builder.Services.AddScoped<IRuleService, RuleService>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();



// 配置 JWT 认证
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // 设置默认的认证方案
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 设置默认的挑战方案
// })
// .AddJwtBearer(options =>
// {
//     // 配置 JWT 令牌验证参数
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true, // 验证发行者
//         ValidateAudience = true, // 验证受众
//         ValidateLifetime = true, // 验证令牌有效期
//         ValidateIssuerSigningKey = true, // 验证签名密钥
//         ValidIssuer = builder.Configuration["Jwt:Issuer"], // 有效的发行者
//         ValidAudience = builder.Configuration["Jwt:Audience"], // 有效的受众
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // 签名密钥
//     };
// });

// 配置 OpenAPI（使用 NSwag）
// builder.Services.AddOpenApiDocument(configure =>
// {
//     configure.Title = "API Gateway Management API"; // 设置 API 文档标题
//     configure.Version = "v1"; // 设置 API 版本
//     configure.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
//     {
//         Type = NSwag.OpenApiSecuritySchemeType.Http, // 安全类型为 HTTP
//         Scheme = "bearer", // 认证方案为 bearer
//         BearerFormat = "JWT", // 认证格式为 JWT
//         Description = "输入 JWT Token，格式为 Bearer {token}" // 安全描述
//     });

//     configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT")); // 添加安全处理器
// });


#region 配置 OpenTelemetry
string serviceName = "GJJApiGateway.Management.Api";
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService($"{serviceName}_Service")) // 设置服务名
    .WithTracing(tracing => tracing
        .AddSource($"{serviceName}_Source")
        .AddAspNetCoreInstrumentation(options =>  // 收集 HTTP 请求数据
        {
            options.RecordException = true; // 确保记录 HTTP 请求中的异常
            options.Filter = (httpContext) =>
            {
                // 过滤掉不需要记录的 URL 或请求路径
                var path = httpContext.Request.Path.Value;
                return //!path.StartsWith("/v1/health/service/") &&
                    //!path.StartsWith("/v1/catalog/nodes") &&
                    //!path.StartsWith("/notifications/v2") &&
                    !path.StartsWith("/health") 
                    //&&!path.StartsWith("/v1/agent")
                    ;
            };
        })
        .AddHttpClientInstrumentation(options =>  // 收集 HttpClient 数据
        {
            options.RecordException = true; // 确保记录 HTTP 客户端请求的异常
                                            // 过滤掉不需要的 HTTP 请求
            options.FilterHttpRequestMessage = httpRequestMessage =>
            {
                // 假设要排除某些路径，如健康检查相关的路径
                return //!httpRequestMessage.RequestUri.AbsoluteUri.Contains("/v1/health/service/") &&
                    //!httpRequestMessage.RequestUri.AbsoluteUri.Contains("/v1/catalog/nodes") &&
                    //!httpRequestMessage.RequestUri.AbsoluteUri.Contains("/notifications/v2") &&
                    !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/health") 
                    //&& !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/v1/agent")
                    ;
            };
        })
        .AddSqlClientInstrumentation(options =>
        {
            options.SetDbStatementForText = true; // 记录 SQL 语句文本
            options.RecordException = true; // 记录数据库异常
            options.EnableConnectionLevelAttributes = true; // 记录数据库连接级别的属性
            options.Enrich = (activity, eventName, command) =>  //额外添加一些信息
            {
                if (command is SqlCommand sqlCommand)
                {
                    activity.SetTag("db.query_type", sqlCommand.CommandType.ToString());
                    activity.SetTag("db.timeout", sqlCommand.CommandTimeout);
                    // 遍历 command.Parameters，记录每个参数的名称和值
                    foreach (IDataParameter param in sqlCommand.Parameters)
                    {
                        string parameterName = param.ParameterName;
                        string parameterValue = param.Value?.ToString() ?? "NULL"; // 防止空值
                        activity.SetTag($"db.command_parameter.{parameterName}", parameterValue);
                    }
                }
            };
        })
        //.AddEntityFrameworkCoreInstrumentation(options =>
        //{
        //    options.SetDbStatementForStoredProcedure = true; // 记录存储过程
        //    options.SetDbStatementForText = true; // 记录 EF Core 生成的 SQL
        //    options.EnrichWithIDbCommand = (activity, command) => // 额外添加一些信息
        //    {
        //        activity.SetTag("db.command_type", command.CommandType.ToString());
        //        activity.SetTag("db.command_timeout", command.CommandTimeout);
        //        // 遍历 command.Parameters，记录每个参数的名称和值
        //        foreach (IDataParameter param in command.Parameters)
        //        {
        //            string parameterName = param.ParameterName;
        //            string parameterValue = param.Value?.ToString() ?? "NULL"; // 防止空值
        //            activity.SetTag($"db.command_parameter.{parameterName}", parameterValue);
        //        }

        //    };
        //    options.Filter = (providerName, command) =>
        //    {
        //        return !command.CommandText.Contains("SensitiveTable"); // 过滤掉敏感数据表
        //    };

        //})
        .AddOtlpExporter(options =>
        {
            // 推送到 Jaeger
            options.Endpoint = new Uri(_jaegerHostgRPC); // Jaeger 的 OTLP gRPC 端点
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc; // 使用 gRPC 协议
        })
        .AddOtlpExporter(options =>
        {
            // 推送到 Seq
            options.Endpoint = new Uri(_seqHostHTTP); // Seq 的 OTLP 导出路径
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf; // 使用 HTTP-Protobuf 协议
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation() // 它会自动跟踪所有经过 ASP.NET Core 应用的 HTTP 请求，包括请求的开始时间、结束时间、响应时间、状态码、请求方法（GET、POST 等）
        .AddHttpClientInstrumentation()  //这个选项是用来自动收集 HttpClient 的请求和响应的性能数据。它会捕捉所有通过 HttpClient 发出的 HTTP 请求（例如，API 调用）
        .AddSqlClientInstrumentation()  // 它会记录 SQL 查询的执行时间、SQL 查询的文本、参数信息等，帮助你监控数据库查询的性能。
        .AddInstrumentation(options =>   // 允许你添加自定义的指标收集逻辑。
        {
            var meter = new System.Diagnostics.Metrics.Meter("MyCustomMeter", "1.0");
            var requestDuration = meter.CreateHistogram<int>("request_duration");

            // 记录请求的持续时间（假设获取了一个处理时间值）
            requestDuration.Record(100);  // 假设是 100 毫秒

            return meter;
        })
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(_jaegerHostHTTP); // Jaeger 的 OTLP HTTP 端点
        }));


#endregion

#region  配置 OpenTelemetry 日志

builder.Services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
{
    // 设置资源构建器，定义 OpenTelemetry 服务的相关属性
    openTelemetryLoggerOptions.SetResourceBuilder(
        ResourceBuilder.CreateEmpty() // 创建一个空的资源构建器
            .AddService($"{serviceName}_Service") // 设置服务的名称，替换为你的应用程序的名字
            .AddAttributes(new Dictionary<string, object>
            {
                // 在资源中添加环境等额外属性
                ["deployment.environment"] = "development" // 添加部署环境属性，设置为 "development"
            }));

    // 配置一些重要选项来提升日志数据的质量
    openTelemetryLoggerOptions.IncludeScopes = true; // 启用日志范围，这样通过 `ILogger.BeginScope()` 附加的属性也会发送到 OTLP exporter
    openTelemetryLoggerOptions.IncludeFormattedMessage = true; // 确保日志消息可以被 Seq 正确识别为原始消息模板

    // 添加 OTLP 导出器，配置日志数据导出到 OTLP 端点
    openTelemetryLoggerOptions.AddOtlpExporter(exporter =>
    {
        // 配置 OTLP 导出器的端点，这里使用 HTTP 协议进行传输
        // 使用 `HttpProtobuf` 协议时需要指定完整的端点路径
        exporter.Endpoint = new Uri(_seqHostLog); // 设置 OTLP 服务器端点地址，指定将日志发送到 Seq
        exporter.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf; // 设置导出的协议为 HTTP Protobuf 格式
                                                                                    // 如果需要身份验证，可以通过设置 Header 来添加 API 密钥
                                                                                    //exporter.Headers = "X-Seq-ApiKey=knt01glGTtj8hLA85HDV"; // 可选的身份验证头，配置 Seq 的 API 密钥
    });
}));

#endregion

#region 配置 Consul 服务注册


var consulClient = new ConsulClient(config =>
{
    config.Address = new Uri(_consulHost);
});

// 注册 Consul 客户端（IConsulClient）为单例
builder.Services.AddSingleton<IConsulClient>(sp =>
{
    return consulClient;
});
var registration = new AgentServiceRegistration
{
    ID = $"{serviceName}-1",
    Name = $"{serviceName}",
    Address = _hostIP, // 让 Consul 自动识别 IP
    Port = _hostPort,
    Check = new AgentServiceCheck
    {
        HTTP = $"http://{_hostIP}:{_hostPort}/health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30)
    }
};

// 注册到 Consul
await consulClient.Agent.ServiceRegister(registration);

#endregion

#region 配置 Kestrel 服务器端口
// 配置 Kestrel 服务器端口
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5043);
});

#endregion

// 配置过滤器
builder.Services.AddControllers(options =>
{
    // 添加用户授权过滤器
    options.Filters.Add<UserAuthorizeFilter>();
    // 添加 AES 加密操作过滤器
    //options.Filters.Add<AESEncryptionActionFilter>();
}).AddNewtonsoftJson(options =>
{


    // 修改时间的序列化方式
    options.SerializerSettings.Converters.Add(new FixedDateTimeConverter());
    // 忽略循环引用
    // options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    // 我们将 NullValueHandling 设置为 Ignore，表示在序列化时忽略 null 值。如果您希望将 null 值序列化为 JSON 字符串 "null"，则可以将其设置为 Include。
    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
}); ;

var app = builder.Build();
app.UseCors("AppCors"); // 配置 CORS 策略
// 中间件配置
if (app.Environment.IsDevelopment())
{
    // app.UseOpenApi(); // 生成 OpenAPI 文档
    // app.UseSwaggerUi3(); // 使用 NSwag 的 Swagger UI
}

app.UseHttpsRedirection(); // 启用 HTTPS 重定向

app.UseAuthentication(); // 启用认证中间件
app.UseAuthorization(); // 启用授权中间件

app.MapControllers(); // 映射控制器路由

app.Run(); // 运行应用程序