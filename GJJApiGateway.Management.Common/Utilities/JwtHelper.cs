using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using static System.Net.Mime.MediaTypeNames;

namespace GJJApiGateway.Management.Common.Utilities
{
    /// <summary>
    /// jwt帮助类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 使用JWT进行加密，生成包含授权信息的令牌。
        /// </summary>
        /// <param name="applicationId">应用程序的唯一标识符，用于识别授权的应用程序。</param>
        /// <param name="applicationName">应用程序名称</param>
        /// <param name="apiIds">逗号分隔的API标识符列表，代表这些API被授权给应用程序使用。</param>
        /// <param name="authorizationDurationDays">授权的时长，单位为天，用于计算令牌的过期时间。</param>
        /// <param name="concatenatedApiPaths">逗号分隔的API路径列表，描述了所有被授权的API的具体访问路径。</param>
        /// <param name="authMethod">认证模式</param>
        /// <param name="exp">令牌的过期时间，表示为从1970年1月1日以来的秒数。</param>
        /// <param name="tokenVersion">令牌的版本号，用于确保令牌的唯一性和有效性。</param>
        /// <returns>生成的JWT令牌字符串，包含所有授权信息和过期时间。</returns>
        public static string EncryptApi(int applicationId, string applicationName,
            string apiIds, string authorizationDurationDays, string concatenatedApiPaths, string authMethod, double exp, int tokenVersion)
        {
            #region 1) 加密
            var payload = new Dictionary<string, object>
    {
        { "applicationId", applicationId },
        { "applicationName", applicationName },
        { "apiIds", apiIds },
        { "authorizationDurationDays", authorizationDurationDays },
        { "concatenatedApiPaths", concatenatedApiPaths },
        { "authMethod", authMethod },
        { "exp", exp },
        { "tokenVersion", tokenVersion } // 添加tokenVersion到payload
    };
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk"; // 这是秘钥，用于JWT的加密过程，不应外泄
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // 使用HMAC SHA256算法进行加密
            IJsonSerializer serializer = new JsonNetSerializer(); // 使用Json.NET进行序列化
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder(); // 使用基于URL的Base64编码器
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
            #endregion
        }

        /// <summary>
        /// jwt加密
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="roleId">角色id</param>
        /// <param name="exp">过期时间</param>
        /// <returns></returns>
        public static string Encrypt(string userId, string userName, string roleId, double exp)
        {
            #region 1) 加密
            var payload = new Dictionary<string, object>
            {
                { "applicationId", 1 },
                { "userId", userId },
                { "userName", userName },
                { "roleId", roleId },
                { "exp", exp }
            };
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";//不要泄露
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
            #endregion

        }
        /// <summary>
        /// jwt加密
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="userName">用户名</param>
        /// <param name="roleId">角色id</param>
        /// <param name="exp">过期时间</param>
        /// <returns></returns>
        public static string SecondaryAuthEncrypt(string userId, string userName, int roleId, double exp)
        {
            #region 1) 加密
            var payload = new Dictionary<string, object>
            {
                { "userId", userId },
                { "userName", userName },
                { "roleId", roleId },
                { "exp", exp }
            };
            var secret = "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBALow9";//不要泄露
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);
            return token;
            #endregion

        }

        /// <summary>
        /// 使用JWT进行加密，生成包含授权信息的令牌。
        /// </summary>
        /// <param name="payload">负载数据</param>
        /// <param name="secret">秘钥</param>
        /// <returns>生成的JWT令牌</returns>
        public static string Encode(IDictionary<string, object> payload, string secret)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // 使用HMAC SHA256算法进行加密
            IJsonSerializer serializer = new JsonNetSerializer(); // 使用Json.NET进行序列化
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder(); // 使用基于URL的Base64编码器
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);
            return token;
        }

        /// <summary>
        /// 解码JWT并验证签名
        /// </summary>
        /// <param name="jwtToken">JWT令牌</param>
        /// <param name="secret">秘钥</param>
        /// <returns>解码后的JWT负载</returns>
        public static IDictionary<string, object> Decode(string jwtToken, string secret)
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

            return decoder.DecodeToObject(jwtToken, secret, verify: true);
        }

        public  static bool ValidateJwtToken(string jwtToken, string secret, out string json)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                json = decoder.Decode(jwtToken, secret, verify: true);

                return true;
            }
            catch (Exception)
            {
                json = "";
                return false;
            }
        }
    }
}
