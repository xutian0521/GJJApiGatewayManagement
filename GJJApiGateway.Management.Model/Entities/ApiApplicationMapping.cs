using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    /// <summary>
    /// Api应用映射关系
    /// </summary>
    public class ApiApplicationMapping
    {
        /// <summary>
        /// 主键，自增
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 外键，指向 ApiInfo 表中的 Id，表示关联的 API
        /// </summary>
        public int ApiId { get; set; }
        /// <summary>
        /// 外键，指向 ApiApplication 表中的 Id，表示关联的应用
        /// </summary>
        public int ApplicationId { get; set; }
        /// <summary>
        /// 授权时长（可选），单位为分钟
        /// </summary>
        public int? AuthorizationDuration { get; set; }

    }
}
