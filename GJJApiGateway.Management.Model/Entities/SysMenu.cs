using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    public class SysMenu
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string? ALIAS { get; set; }
        /// <summary>
        /// 父级节点id
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string? PATH { get; set; }
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string? ICON { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SORTID { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public int ISENABLE { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? REMARK { get; set; }
        /// <summary>
        /// 城市网点编号
        /// </summary>
        public string? CITYCENTNO { get; set; }
    }
}
