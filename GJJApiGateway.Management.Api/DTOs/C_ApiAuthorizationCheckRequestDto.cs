namespace GJJApiGateway.Management.Api.DTOs
{
    public class C_ApiAuthorizationCheckRequestDto
    {
        public string? JwtToken { get; set; }
        public string? ApiPath { get; set; }
    }
}
