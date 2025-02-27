using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_ApiApplicationDto
    {
        /// <summary>
        /// API应用程序的唯一标识符。
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// API应用程序的名称。
        /// </summary>
        public string? ApplicationName { get; set; }

        /// <summary>
        /// JwtToken
        /// </summary>
        public string? JwtToken { get; set; }

        /// <summary>
        /// API应用程序的认证方法。
        /// </summary>
        public string? AuthMethod { get; set; }

        /// <summary>
        /// API来源信息，描述API是如何被该应用程序使用。
        /// </summary>
        public string? ApiSource { get; set; }

        /// <summary>
        /// API应用程序的最后修改时间。
        /// </summary>
        public DateTime LastModifiedTime { get; set; }

        /// <summary>
        /// API应用程序的创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 当前JWT版本号
        /// </summary>
        public int TokenVersion { get; set; }
    }
}
