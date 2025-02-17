using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Common.Utilities
{
    public static class NetworkHelper
    {
        /// <summary>
        /// 获取客户端 IP 地址（支持代理头部）
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>IP 地址字符串</returns>
        public static string GetIP(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            string ip = null;

            // 1. 获取 X-Forwarded-For（代理服务器可能设置多个 IP）
            string xForwardedFor = httpContext.Request.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(xForwardedFor))
            {
                ip = xForwardedFor.Split(',').Select(x => x.Trim()).FirstOrDefault();
                if (!string.IsNullOrEmpty(ip) && ip.Length > 7) return ip;
            }

            // 2. 尝试获取 RemoteIp
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Request.Headers["RemoteIp"];
            }

            // 3. 获取直接的连接 IP
            if (string.IsNullOrEmpty(ip) && httpContext.Connection.RemoteIpAddress != null)
            {
                ip = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }

            return string.IsNullOrEmpty(ip) ? "0.0.0.0" : ip.Replace("'", "");
        }
    }
}
