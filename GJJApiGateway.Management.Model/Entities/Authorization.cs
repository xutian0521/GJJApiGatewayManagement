using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    /// <summary>
    /// 授权规则实体，描述授权规则的数据结构。
    /// </summary>
    public class Authorization
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
        /// 允许访问的 API 端点列表，使用逗号分隔或 JSON 格式存储。
        /// </summary>
        public string AllowedEndpoints { get; set; } // JSON 或其他格式

        /// <summary>
        /// 授权规则创建的时间戳。
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 授权规则最后更新的时间戳。
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
