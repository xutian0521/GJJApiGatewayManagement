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
        public string ROLE_ID { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string USER_NAME { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string USER_ID { get; set; }
        /// <summary>
        /// jwt token
        /// </summary>
        public string ACCESS_TOKEN { get; set; }
        /// <summary>
        /// jwt过期时间
        /// </summary>
        public int EXPIRES_IN { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string REAL_NAME { get; set; }

    }
}
