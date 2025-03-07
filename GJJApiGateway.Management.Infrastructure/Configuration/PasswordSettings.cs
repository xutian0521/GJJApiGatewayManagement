using Microsoft.Extensions.Configuration;

namespace GJJApiGateway.Management.Infrastructure.Configuration;

public class PasswordSettings
{
    private readonly IConfiguration _config;

    public PasswordSettings(IConfiguration config)
    {
        _config = config;
    }

    public string Salt => _config.GetSection("Salt").Value;
}