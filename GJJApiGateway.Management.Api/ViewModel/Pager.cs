namespace GJJApiGateway.Management.Api.ViewModel
{
    public class Pager<T>
    {
        public List<T> List { get; set; }
        public int Total { get; set; }
    }
}
