using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Common.DTOs
{
    /// <summary>
    /// 通用 API 注册信息 DTO，由 Common 层提供，不依赖数据层实体。
    /// </summary>
    public class CommonApiInfoDto
    {
        public string? ApiPath { get; set; }
        public string? ApiChineseName { get; set; }
        public string? ApiType { get; set; }
        public string? Body { get; set; }
        public string? Parameter { get; set; }
        public string? Description { get; set; }
        public string? ResultStruct { get; set; }
        public DateTime CreateTime { get; set; }
        public int ServiceStatus { get; set; }
    }
}
