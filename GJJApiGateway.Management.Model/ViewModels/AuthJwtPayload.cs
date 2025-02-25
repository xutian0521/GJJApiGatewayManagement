using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.ViewModels
{
    /// <summary>
    /// 表示授权JWT的负载数据。
    /// 用于生成JWT时包含所有必要的授权信息。
    /// </summary>
    public class AuthJwtPayload
    {
        /// <summary>
        /// 应用程序的唯一标识符。
        /// </summary>
        public int applicationId { get; set; }
        /// <summary>
        /// 授权的持续时间，以天为单位。
        /// </summary>
        public string? authorizationDurationDays { get; set; }
        /// <summary>
        /// API的认证方式（例如：免认证，jwt认证，应用程序认证）。
        /// </summary>
        public string? authMethod { get; set; }

        /// <summary>
        /// JWT的过期时间，Unix时间戳格式。
        /// </summary>
        public double exp { get; set; }
        /// <summary>
        /// jwt版本号
        /// </summary>
        public int tokenVersion { get; set; }
    }
}

