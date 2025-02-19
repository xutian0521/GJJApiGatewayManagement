using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Api.ViewModel
{
    /// <summary>
    /// 系统菜单vm
    /// </summary>
    public class SysMenuVM
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string NAME { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        public string ALIAS { get; set; }
        /// <summary>
        /// 父级节点id
        /// </summary>
        public int PID { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string PATH { get; set; }
        /// <summary>
        /// 菜单标识
        /// </summary>
        public string ICON { get; set; }
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
        public string REMARK { get; set; }
        /// <summary>
        /// 字集合
        /// </summary>
        public List<SysMenuVM> Subs { get; set; } = new List<SysMenuVM>();
    }
}
