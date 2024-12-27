namespace GJJApiGateway.Management.Api.DTOs
{
    public class CreateAuthorizationDto
    {
        public string Role { get; set; }
        public List<string> AllowedEndpoints { get; set; }
    }
}
