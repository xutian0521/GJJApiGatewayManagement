namespace GJJApiGateway.Management.Application.APIAuthService.DTOs
{
    public class A_ApiAuthorizationCheckRequestDto
    {
        public string? JwtToken { get; set; }
        public string? ApiPath { get; set; }
    }
}
