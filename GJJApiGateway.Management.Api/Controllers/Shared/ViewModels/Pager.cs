namespace GJJApiGateway.Management.Api.Controllers.Shared.ViewModels
{
    public class Pager<T>
    {
        public List<T> List { get; set; }
        public int Total { get; set; }
    }
}
