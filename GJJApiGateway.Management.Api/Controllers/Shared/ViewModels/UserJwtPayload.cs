namespace GJJApiGateway.Management.Api.Controllers.Shared.ViewModels
{
    public class UserJwtPayload
    {
        /// <summary>
        /// 会员编号
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// roleId
        /// </summary>
        public string roleId { get; set; }
        /// <summary>
        /// 用户凭证过期时间
        /// </summary>
        public double exp { get; set; }
    }
}
