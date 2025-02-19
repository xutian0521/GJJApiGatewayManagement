using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    public class SysRoleMenu
    {
        public int ID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int ROLEID { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MENUID { get; set; }

        /// <summary>
        /// 是否有添加权限
        /// </summary>
        public int CANADD { get; set; }

        /// <summary>
        /// 是否有编辑权限
        /// </summary>
        public int CANEDIT { get; set; }

        /// <summary>
        /// 是否有删除权限
        /// </summary>
        public int CANDELETE { get; set; }

        /// <summary>
        /// 是否有查看权限
        /// </summary>
        public int CANAUDIT { get; set; }
    }
}
