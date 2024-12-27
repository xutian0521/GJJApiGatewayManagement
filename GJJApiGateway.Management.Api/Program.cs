using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.Generation.Processors.Security;
using GJJApiGateway.Management.Application.Mapping;
using GJJApiGateway.Management.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// ��ӿ�����
builder.Services.AddControllers();

// ���� AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ע��Ӧ�ò�ģ��
builder.Services.AddApplicationModule();
builder.Services.AddInfrastructureModule(builder.Configuration);
builder.Services.AddCommonModule();

// ���� JWT ��֤
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// ���� OpenAPI��ʹ�� NSwag��
builder.Services.AddOpenApiDocument(configure =>
{
    configure.Title = "API Gateway Management API";
    configure.Version = "v1";
    configure.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "���� JWT Token����ʽΪ Bearer {token}"
    });

    configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

var app = builder.Build();

// �м������
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(); // ���� OpenAPI �ĵ�
    app.UseSwaggerUi3(); // ʹ�� NSwag �� Swagger UI
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
