namespace GJJApiGateway.Management.Application.AdminService.DTOs
{
    public class A_SysMenuDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
    
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string? Name { get; set; }
    
        /// <summary>
        /// 别名
        /// </summary>
        public string? Alias { get; set; }
    
        /// <summary>
        /// 父级节点 ID
        /// </summary>
        public int PId { get; set; }
    
        /// <summary>
        /// 路由
        /// </summary>
        public string? Path { get; set; }
    
        /// <summary>
        /// 菜单标识（图标）
        /// </summary>
        public string? Icon { get; set; }
    
        /// <summary>
        /// 排序
        /// </summary>
        public int SortId { get; set; }
    
        /// <summary>
        /// 是否可用
        /// </summary>
        public int IsEnable { get; set; }
    
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    
        /// <summary>
        /// 子菜单集合
        /// </summary>
        public List<A_SysMenuDto> Subs { get; set; } = new List<A_SysMenuDto>();
    }

}
