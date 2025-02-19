using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    public class SysRole
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string? ROLENAME { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public string? SORTID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? REMARK { get; set; }
    }
}
