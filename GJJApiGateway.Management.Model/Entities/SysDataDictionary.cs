using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    /// <summary>
    /// 数据字典表
    /// </summary>
    public class SysDataDictionary
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 枚举类型
        /// </summary>
        public string? TYPEKEY { get; set; }

        /// <summary>
        /// 枚举类型名称
        /// </summary>
        public string? TYPELABEL { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string? VALUE { get; set; }

        /// <summary>
        /// 值对应的解释
        /// </summary>
        public string? VALUESTR { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SORTID { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? DESCRIPTION { get; set; }

        /// <summary>
        /// 前端是否可获取到 0:获取不到 1:可以获取到
        /// </summary>
        public int QDSHOW { get; set; }

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
