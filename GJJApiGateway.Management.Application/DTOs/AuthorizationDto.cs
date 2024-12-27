namespace GJJApiGateway.Management.Api.DTOs
{
    public class AuthorizationDto
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public List<string> AllowedEndpoints { get; set; }
    }
}
