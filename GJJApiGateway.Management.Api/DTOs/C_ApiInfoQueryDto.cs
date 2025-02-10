namespace GJJApiGateway.Management.Api.DTOs
{
    public class C_ApiInfoQueryDto
    {
        public string? ApiChineseName { get; set; }
        public string? Description { get; set; }
        public string? BusinessIdentifier { get; set; }
        public string? ApiSource { get; set; }
        public string? ApiPath { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public string? Sort { get; set; }  // 改为 string 类型
    }
}
