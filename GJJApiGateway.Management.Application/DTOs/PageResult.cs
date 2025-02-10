using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    /// <summary>
    /// 业务层分页结果 DTO
    /// </summary>
    public class PageResult<T>
    {
        /// <summary>
        /// 分页数据列表
        /// </summary>
        public List<T> Items { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }
    }
}
