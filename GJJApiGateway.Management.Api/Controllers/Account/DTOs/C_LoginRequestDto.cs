namespace GJJApiGateway.Management.Api.Controllers.Account.DTOs
{
    public class C_LoginRequestDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? code { get; set; }
        public string? codeKey { get; set; }
    }
}
