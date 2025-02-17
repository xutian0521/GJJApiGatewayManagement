using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    public class SYS_USERINFO
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? NAME { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string? REALNAME { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? PASSWORD { get; set; }
        /// <summary>
        /// 盐
        /// </summary>
        public string? SALT { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public string? ROLEID { get; set; }
        /// <summary>
        /// 最后登录ip
        /// </summary>
        public string? LASTLOGINIP { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LASTLOGINTIME { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CREATETIME { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? REMARK { get; set; }
    }
}
