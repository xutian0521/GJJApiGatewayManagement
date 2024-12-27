using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;
using GJJApiGateway.Management.Application.Mapping;
using GJJApiGateway.Management.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 添加控制器服务到容器
builder.Services.AddControllers();

// 配置 AutoMapper，指定映射配置文件所在的程序集
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 注册应用层模块，包括认证、限流和路径管理
builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddCommonModule();

// 配置 JWT 认证
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // 设置默认的认证方案
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // 设置默认的挑战方案
})
.AddJwtBearer(options =>
{
    // 配置 JWT 令牌验证参数
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // 验证发行者
        ValidateAudience = true, // 验证受众
        ValidateLifetime = true, // 验证令牌有效期
        ValidateIssuerSigningKey = true, // 验证签名密钥
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // 有效的发行者
        ValidAudience = builder.Configuration["Jwt:Audience"], // 有效的受众
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // 签名密钥
    };
});

// 配置 OpenAPI（使用 NSwag）
builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "API Gateway Management API"; // 设置 API 文档标题
    configure.Version = "v1"; // 设置 API 版本
    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http, // 安全类型为 HTTP
        Scheme = "bearer", // 认证方案为 bearer
        BearerFormat = "JWT", // 认证格式为 JWT
        Description = "输入 JWT Token，格式为 Bearer {token}" // 安全描述
    });

    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT")); // 添加安全处理器
});

var app = builder.Build();

// 中间件配置
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(); // 生成 OpenAPI 文档
    app.UseSwaggerUi3(); // 使用 NSwag 的 Swagger UI
}

app.UseHttpsRedirection(); // 启用 HTTPS 重定向

app.UseAuthentication(); // 启用认证中间件
app.UseAuthorization(); // 启用授权中间件

app.MapControllers(); // 映射控制器路由

app.Run(); // 运行应用程序