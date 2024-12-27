namespace GJJApiGateway.Management.Api.DTOs
{
    /// <summary>
    /// 创建授权规则的请求数据传输对象。
    /// </summary>
    public class CreateAuthorizationDto
    {
        /// <summary>
        /// 角色名称，例如 Admin、User 等。
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 允许访问的 API 端点列表。
        /// </summary>
        public List<string> AllowedEndpoints { get; set; }
    }
}
