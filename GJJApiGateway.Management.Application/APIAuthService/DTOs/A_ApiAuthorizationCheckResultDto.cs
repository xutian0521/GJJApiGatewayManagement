namespace GJJApiGateway.Management.Application.APIAuthService.DTOs
{
    public class A_ApiAuthorizationCheckResultDto
    {
        public bool IsAuthorized { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }

    }
}
