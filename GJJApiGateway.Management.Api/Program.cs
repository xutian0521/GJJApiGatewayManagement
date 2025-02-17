using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;
using GJJApiGateway.Management.Application.Mapping;
using GJJApiGateway.Management.Application.Extensions;
using GJJApiGateway.Management.Application.Services;
using GJJApiGateway.Management.Infrastructure.Repositories;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Api.Mappings;
using Consul;
using System.Net;
using System.Net.Sockets;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using System.Data;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

#region ���� Apollo �ֲ�ʽ��������

// ���� Apollo �ֲ�ʽ��������
// Apollo ��һ���ֲ�ʽ�����ù���ϵͳ���ܹ���̬��ΪӦ�ó����ṩ�������ݣ�֧�����õ�ʵʱ���¡�
// ʹ�� AddApollo �������� Apollo ���ü��ɵ� ASP.NET Core ����ϵͳ�С�
// ͨ�� builder.Configuration.GetSection("apollo") ��ȡ���ýڵ㣬�����ݸ� AddApollo ������
builder.Configuration.AddApollo(builder.Configuration.GetSection("apollo"));
#endregion


// �������е�ַ���ʣ�֧�ֲ�ͬ����������
var hostIP = "localhost";//Dns.GetHostAddresses(Dns.GetHostName())
                //.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?
                //.ToString() ?? "127.0.0.1";

Console.WriteLine($"��ǰ���� IP: {hostIP}");

#region ���� CORS

// ��� CORS ���񲢶���һ�����ԣ��������ļ��ж�ȡ�������Դ
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

// ��ӿ�������������
builder.Services.AddControllers();

// ���� AutoMapper��ָ��ӳ�������ļ����ڵĳ���
builder.Services.AddAutoMapper(typeof(ApplicationMappingProfile));
builder.Services.AddAutoMapper(typeof(ControllerMappingProfile));



// ע��Ӧ�ò�ģ�飬������֤��������·������
builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddCommonModule();

// ע��ҵ������ݿ��
builder.Services.AddScoped<ApiManageService>();
builder.Services.AddScoped<IApiInfoRepository, ApiInfoRepository>();
builder.Services.AddScoped<IAccountService, AccountMockService>();
builder.Services.AddScoped<ISysUserInfoRepository, SysUserInfoRepository>();

// ���� JWT ��֤
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // ����Ĭ�ϵ���֤����
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // ����Ĭ�ϵ���ս����
})
.AddJwtBearer(options =>
{
    // ���� JWT ������֤����
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // ��֤������
        ValidateAudience = true, // ��֤����
        ValidateLifetime = true, // ��֤������Ч��
        ValidateIssuerSigningKey = true, // ��֤ǩ����Կ
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // ��Ч�ķ�����
        ValidAudience = builder.Configuration["Jwt:Audience"], // ��Ч������
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // ǩ����Կ
    };
});

// ���� OpenAPI��ʹ�� NSwag��
builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "API Gateway Management API"; // ���� API �ĵ�����
    configure.Version = "v1"; // ���� API �汾
    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http, // ��ȫ����Ϊ HTTP
        Scheme = "bearer", // ��֤����Ϊ bearer
        BearerFormat = "JWT", // ��֤��ʽΪ JWT
        Description = "���� JWT Token����ʽΪ Bearer {token}" // ��ȫ����
    });

    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT")); // ��Ӱ�ȫ������
});


#region ���� OpenTelemetry
string serviceName = "GJJApiGatewayManagement.Api";
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService($"{serviceName}_Service")) // ���÷�����
    .WithTracing(tracing => tracing
        .AddSource($"{serviceName}_Source")
        .AddAspNetCoreInstrumentation(options =>  // �ռ� HTTP ��������
        {
            options.RecordException = true; // ȷ����¼ HTTP �����е��쳣
            options.Filter = (httpContext) =>
            {
                // ���˵�����Ҫ��¼�� URL ������·��
                var path = httpContext.Request.Path.Value;
                return !path.StartsWith("/v1/health/service/") &&
                       !path.StartsWith("/v1/catalog/nodes") &&
                       !path.StartsWith("/notifications/v2") &&
                       !path.StartsWith("/health") &&
                       !path.StartsWith("/v1/agent");
            };
        })
        .AddHttpClientInstrumentation(options =>  // �ռ� HttpClient ����
        {
            options.RecordException = true; // ȷ����¼ HTTP �ͻ���������쳣
                                            // ���˵�����Ҫ�� HTTP ����
            options.FilterHttpRequestMessage = httpRequestMessage =>
            {
                // ����Ҫ�ų�ĳЩ·�����罡�������ص�·��
                return !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/v1/health/service/") &&
                       !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/v1/catalog/nodes") &&
                       !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/notifications/v2") &&
                       !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/health") &&
                        !httpRequestMessage.RequestUri.AbsoluteUri.Contains("/v1/agent");
            };
        })
        .AddSqlClientInstrumentation(options =>
        {
            options.SetDbStatementForText = true; // ��¼ SQL ����ı�
            options.RecordException = true; // ��¼���ݿ��쳣
            options.EnableConnectionLevelAttributes = true; // ��¼���ݿ����Ӽ��������
            options.Enrich = (activity, eventName, command) =>  //�������һЩ��Ϣ
            {
                if (command is SqlCommand sqlCommand)
                {
                    activity.SetTag("db.query_type", sqlCommand.CommandType.ToString());
                    activity.SetTag("db.timeout", sqlCommand.CommandTimeout);
                    // ���� command.Parameters����¼ÿ�����������ƺ�ֵ
                    foreach (IDataParameter param in sqlCommand.Parameters)
                    {
                        string parameterName = param.ParameterName;
                        string parameterValue = param.Value?.ToString() ?? "NULL"; // ��ֹ��ֵ
                        activity.SetTag($"db.command_parameter.{parameterName}", parameterValue);
                    }
                }
            };
        })
        //.AddEntityFrameworkCoreInstrumentation(options =>
        //{
        //    options.SetDbStatementForStoredProcedure = true; // ��¼�洢����
        //    options.SetDbStatementForText = true; // ��¼ EF Core ���ɵ� SQL
        //    options.EnrichWithIDbCommand = (activity, command) => // �������һЩ��Ϣ
        //    {
        //        activity.SetTag("db.command_type", command.CommandType.ToString());
        //        activity.SetTag("db.command_timeout", command.CommandTimeout);
        //        // ���� command.Parameters����¼ÿ�����������ƺ�ֵ
        //        foreach (IDataParameter param in command.Parameters)
        //        {
        //            string parameterName = param.ParameterName;
        //            string parameterValue = param.Value?.ToString() ?? "NULL"; // ��ֹ��ֵ
        //            activity.SetTag($"db.command_parameter.{parameterName}", parameterValue);
        //        }

        //    };
        //    options.Filter = (providerName, command) =>
        //    {
        //        return !command.CommandText.Contains("SensitiveTable"); // ���˵��������ݱ�
        //    };

        //})
        .AddOtlpExporter(options =>
        {
            // ���͵� Jaeger
            options.Endpoint = new Uri("http://localhost:4317"); // Jaeger �� OTLP gRPC �˵�
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc; // ʹ�� gRPC Э��
        })
        .AddOtlpExporter(options =>
        {
            // ���͵� Seq
            options.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces"); // Seq �� OTLP ����·��
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf; // ʹ�� HTTP-Protobuf Э��
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation() // �����Զ��������о��� ASP.NET Core Ӧ�õ� HTTP ���󣬰�������Ŀ�ʼʱ�䡢����ʱ�䡢��Ӧʱ�䡢״̬�롢���󷽷���GET��POST �ȣ�
        .AddHttpClientInstrumentation()  //���ѡ���������Զ��ռ� HttpClient ���������Ӧ���������ݡ����Ჶ׽����ͨ�� HttpClient ������ HTTP �������磬API ���ã�
        .AddSqlClientInstrumentation()  // �����¼ SQL ��ѯ��ִ��ʱ�䡢SQL ��ѯ���ı���������Ϣ�ȣ������������ݿ��ѯ�����ܡ�
        .AddInstrumentation(options =>   // ����������Զ����ָ���ռ��߼���
        {
            var meter = new System.Diagnostics.Metrics.Meter("MyCustomMeter", "1.0");
            var requestDuration = meter.CreateHistogram<int>("request_duration");

            // ��¼����ĳ���ʱ�䣨�����ȡ��һ������ʱ��ֵ��
            requestDuration.Record(100);  // ������ 100 ����

            return meter;
        })
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri("http://localhost:4318"); // Jaeger �� OTLP HTTP �˵�
        }));


#endregion

#region  ���� OpenTelemetry ��־

builder.Services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>
{
    // ������Դ������������ OpenTelemetry ������������
    openTelemetryLoggerOptions.SetResourceBuilder(
        ResourceBuilder.CreateEmpty() // ����һ���յ���Դ������
            .AddService($"{serviceName}_Service") // ���÷�������ƣ��滻Ϊ���Ӧ�ó��������
            .AddAttributes(new Dictionary<string, object>
            {
                // ����Դ����ӻ����ȶ�������
                ["deployment.environment"] = "development" // ��Ӳ��𻷾����ԣ�����Ϊ "development"
            }));

    // ����һЩ��Ҫѡ����������־���ݵ�����
    openTelemetryLoggerOptions.IncludeScopes = true; // ������־��Χ������ͨ�� `ILogger.BeginScope()` ���ӵ�����Ҳ�ᷢ�͵� OTLP exporter
    openTelemetryLoggerOptions.IncludeFormattedMessage = true; // ȷ����־��Ϣ���Ա� Seq ��ȷʶ��Ϊԭʼ��Ϣģ��

    // ��� OTLP ��������������־���ݵ����� OTLP �˵�
    openTelemetryLoggerOptions.AddOtlpExporter(exporter =>
    {
        // ���� OTLP �������Ķ˵㣬����ʹ�� HTTP Э����д���
        // ʹ�� `HttpProtobuf` Э��ʱ��Ҫָ�������Ķ˵�·��
        exporter.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs"); // ���� OTLP �������˵��ַ��ָ������־���͵� Seq
        exporter.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf; // ���õ�����Э��Ϊ HTTP Protobuf ��ʽ
                                                                                    // �����Ҫ�����֤������ͨ������ Header ����� API ��Կ
                                                                                    //exporter.Headers = "X-Seq-ApiKey=knt01glGTtj8hLA85HDV"; // ��ѡ�������֤ͷ������ Seq �� API ��Կ
    });
}));

#endregion

#region ���� Consul ����ע��

var consulClient = new ConsulClient(config =>
{
    config.Address = new Uri("http://localhost:8500");
});

// ע�� Consul �ͻ��ˣ�IConsulClient��Ϊ����
builder.Services.AddSingleton<IConsulClient>(sp =>
{
    return consulClient;
});
var registration = new AgentServiceRegistration
{
    ID = $"{serviceName}-1",
    Name = $"{serviceName}",
    Address = hostIP, // �� Consul �Զ�ʶ�� IP
    Port = 5043,
    Check = new AgentServiceCheck
    {
        HTTP = $"http://{hostIP}:5043/health",
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(30)
    }
};

// ע�ᵽ Consul
await consulClient.Agent.ServiceRegister(registration);

#endregion

#region ���� Kestrel �������˿�
// ���� Kestrel �������˿�
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5043);
});

#endregion

var app = builder.Build();
app.UseCors("AppCors"); // ���� CORS ����
// �м������
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(); // ���� OpenAPI �ĵ�
    app.UseSwaggerUi3(); // ʹ�� NSwag �� Swagger UI
}

app.UseHttpsRedirection(); // ���� HTTPS �ض���

app.UseAuthentication(); // ������֤�м��
app.UseAuthorization(); // ������Ȩ�м��

app.MapControllers(); // ӳ�������·��

app.Run(); // ����Ӧ�ó���