using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Common.Utilities
{
    public static class EncryptionHelper
    {
        /// <summary>
        /// 使用 MD5 进行加密
        /// </summary>
        /// <param name="rawPass">原始密码</param>
        /// <param name="salt">盐值</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encoding(string rawPass, string salt)
        {
            if (salt == null) return rawPass;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bs = Encoding.UTF8.GetBytes(rawPass + "{" + salt + "}");
                byte[] hs = md5.ComputeHash(bs);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hs)
                {
                    sb.Append(b.ToString("x2")); // 16进制格式
                }
                return sb.ToString();
            }
        }
    }
}
