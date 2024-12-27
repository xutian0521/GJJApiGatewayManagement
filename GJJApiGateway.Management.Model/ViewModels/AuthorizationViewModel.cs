using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.ViewModels
{
    /// <summary>
    /// 授权规则的视图模型，供前端展示使用。
    /// </summary>
    public class AuthorizationViewModel
    {
        /// <summary>
        /// 授权规则的唯一标识符。
        /// </summary>
        public int Id { get; set; }

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
