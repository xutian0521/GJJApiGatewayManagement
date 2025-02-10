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

var builder = WebApplication.CreateBuilder(args);

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