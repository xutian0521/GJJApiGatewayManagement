namespace GJJApiGateway.Management.Api.Controllers.Account.DTOs
{
    public class C_LoginRequestDto
    {
        public string? userName { get; set; }
        public string? password { get; set; }
        public string? code { get; set; }
        public string? codeKey { get; set; }
    }
}
