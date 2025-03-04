namespace GJJApiGateway.Management.Api.Controllers.APIAuth.DTOs
{
    public class C_ApiAuthorizationCheckRequestDto
    {
        public string? JwtToken { get; set; }
        public string? ApiPath { get; set; }
    }
}
