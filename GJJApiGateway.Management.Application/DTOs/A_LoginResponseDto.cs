using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_LoginResponseDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string? role_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? user_name { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string? user_id { get; set; }

        /// <summary>
        /// jwt token
        /// </summary>
        public string? access_token { get; set; }

        /// <summary>
        /// jwt过期时间
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string? real_name { get; set; }
    }

}
