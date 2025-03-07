namespace GJJApiGateway.Management.Application.Shared.DTOs
{
    /// <summary>
    /// 业务层分页结果 DTO
    /// </summary>
    public class PageResult<T>
    {
        /// <summary>
        /// 分页数据列表
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
    }
}
