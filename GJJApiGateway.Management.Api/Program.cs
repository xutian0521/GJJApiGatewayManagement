using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;
using GJJApiGateway.Management.Application.Mapping;
using GJJApiGateway.Management.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ��ӿ�������������
builder.Services.AddControllers();

// ���� AutoMapper��ָ��ӳ�������ļ����ڵĳ���
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ע��Ӧ�ò�ģ�飬������֤��������·������
builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddCommonModule();

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