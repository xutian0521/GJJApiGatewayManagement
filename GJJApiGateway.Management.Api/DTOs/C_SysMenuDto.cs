namespace GJJApiGateway.Management.Api.DTOs
{
    /// <summary>
    /// 添加或修改菜单 Dto
    /// </summary>
    public class C_SysMenuDto
    {
        /// <summary>
        /// 菜单id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string? path { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string? icon { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sortId { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool isEnable { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? remark { get; set; }
    }
}
