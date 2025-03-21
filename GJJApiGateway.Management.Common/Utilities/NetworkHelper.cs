using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
        
        /// <summary>
        /// 获取本机IPv4地址（跨平台：Windows、macOS、Linux）
        /// </summary>
        /// <returns>返回第一个有效的IPv4地址，如果未找到则返回null</returns>
        public static string GetLocalIPAddress()
        {
            // 尝试通过 NetworkInterface 获取正在运行的网络适配器的IPv4地址
            var ip = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
                .SelectMany(ni => ni.GetIPProperties().UnicastAddresses)
                .Where(ua => ua.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ua.Address))
                .Select(ua => ua.Address.ToString())
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }
        
            // 如果上面方式未找到，则回退使用 DNS 查询方式（有时可能返回127.0.0.1，需要结合实际情况判断）
            // 回退方式：通过 DNS 获取主机IP地址并过滤掉无效地址
            ip = Dns.GetHostAddresses(Dns.GetHostName())
                .Where(addr => addr.AddressFamily == AddressFamily.InterNetwork)
                .Select(addr => addr.ToString())
                .FirstOrDefault(ipAddress =>
                    !ipAddress.StartsWith("127.") &&     // 排除回环地址
                    ipAddress != "0.0.0.0" &&              // 排除无效地址
                    !ipAddress.StartsWith("169.254."));   // 排除链路本地地址

            return ip;
        }
    }
}
