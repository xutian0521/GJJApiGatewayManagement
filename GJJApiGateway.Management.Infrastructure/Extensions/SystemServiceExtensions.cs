using System.Data;
using System.Net;
using Consul;
using GJJApiGateway.Management.Common.Utilities;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace GJJApiGateway.Management.Infrastructure.Extensions;

/// <summary>
/// 系统基础设施服务扩展类，提供项目中通用基础设施（如配置中心、日志、监控、服务注册、跨域策略等）的统一注册方法。
/// </summary>
public static class SystemServiceExtensions
{
    // 默认地址和配置
    private static string _consulHost = "http://192.168.2.188:8500";
    private static string _jaegerHostgRPC = "http://192.168.2.188:4317";
    private static string _jaegerHostHTTP = "http://192.168.2.188:4318";
    private static string _seqHostHTTP = "http://192.168.2.188:5341/ingest/otlp/v1/traces";
    private static string _seqHostLog = "http://192.168.2.188:5341/ingest/otlp/v1/logs";
    private static string _hostIP = NetworkHelper.GetLocalIPAddress();
    private static int _hostPort = 6002;
    
    /// <summary>
    /// 注册所有系统通用基础设施
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configurationBuilder"></param>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    public static IServiceCollection AddSystemInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IConfigurationBuilder configurationBuilder,
        string serviceName)
    {
        // 先注册apollo 不然读去不到apollo的配置
        services.AddApolloConfiguration(configuration, configurationBuilder);
        
        // 配置覆盖默认地址
        _consulHost = configuration["ConsulHost"] ?? _consulHost;
        _jaegerHostgRPC = configuration["JaegerHostgRPC"] ?? _jaegerHostgRPC;
        _jaegerHostHTTP = configuration["JaegerHostHTTP"] ?? _jaegerHostHTTP;
        _seqHostHTTP = configuration["SeqHostHTTP"] ?? _seqHostHTTP;
        _seqHostLog = configuration["SeqHostLog"] ?? _seqHostLog;
        _hostPort = configuration.GetValue<int?>("HostPort") ?? _hostPort;
        _hostIP = configuration["LocalIP"] ?? _hostIP;
        
        services.AddCorsPolicy(configuration)
                .AddOpenTelemetryTracing(configuration, serviceName)
                .AddOpenTelemetryLogging(configuration, serviceName)
                .AddConsulServiceRegistration(configuration, serviceName)
                .ConfigureKestrelServer(configuration);
        
        Console.WriteLine($"jaegerHostgRPC: {_jaegerHostgRPC}");
        Console.WriteLine($"jaegerHostHTTP: {_jaegerHostHTTP}");
        Console.WriteLine($"seqHostHTTP: {_seqHostHTTP}");
        Console.WriteLine($"seqHostLog: {_seqHostLog}");
        Console.WriteLine($"当前主机 IP: {_hostIP}:{_hostPort}");
        
        return services;
    }

    /// <summary>
    /// 配置 Apollo 分布式配置中心
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configurationBuilder"></param>
    /// <returns></returns>
    private static IServiceCollection AddApolloConfiguration(this IServiceCollection services, IConfiguration configuration, IConfigurationBuilder configurationBuilder)
    {
        // Apollo 是一个分布式的配置管理系统，能够动态地为应用程序提供配置数据，支持配置的实时更新。
        // 使用 AddApollo 方法，将 Apollo 配置集成到 ASP.NET Core 配置系统中。
        // 通过 builder.Configuration.GetSection("apollo") 获取配置节点，并传递给 AddApollo 方法。
        configurationBuilder.AddApollo(configuration.GetSection("apollo"));
        return services;
    }

    /// <summary>
    /// 配置跨域
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options => options.AddPolicy("AppCors", policy =>
        {
            var hosts = configuration.GetValue<string>("AppHosts");
            policy.WithOrigins(hosts.Split(','))
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }));

        return services;
    }

    /// <summary>
    /// 配置 OpenTelemetry 追踪
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    private static IServiceCollection AddOpenTelemetryTracing(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        services.AddOpenTelemetry()
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

        return services;
    }

    /// <summary>
    /// 配置 OpenTelemetry 日志
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    private static IServiceCollection AddOpenTelemetryLogging(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
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

        return services;
    }

    /// <summary>
    /// 配置 Consul 服务注册
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    private static IServiceCollection AddConsulServiceRegistration(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        var consulClient = new ConsulClient(config => config.Address = new Uri(_consulHost));

        services.AddSingleton<IConsulClient>(consulClient);

        var registration = new AgentServiceRegistration
        {
            ID = $"{serviceName}-{_hostIP}:{_hostPort}",
            Name = serviceName,
            Address = _hostIP,
            Port = _hostPort,
            Tags = new[] { "描述: 这是网关管理后台API服务", "版本: v1.0" }, // 可以添加中文标签
            Check = new AgentServiceCheck
            {
                HTTP = $"http://{_hostIP}:{_hostPort}/health",
                Interval = TimeSpan.FromSeconds(10),
                Timeout = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30)
            }
        };

        consulClient.Agent.ServiceRegister(registration).Wait();

        return services;
    }

    /// <summary>
    /// 配置 Kestrel 服务器端口
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static IServiceCollection ConfigureKestrelServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Listen(IPAddress.Any, _hostPort);
        });

        return services;
    }
}