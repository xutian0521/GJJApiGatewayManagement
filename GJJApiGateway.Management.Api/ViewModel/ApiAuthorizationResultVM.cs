namespace GJJApiGateway.Management.Api.ViewModel
{
    public class ApiAuthorizationResultVM
    {
        public bool IsAuthorized { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
