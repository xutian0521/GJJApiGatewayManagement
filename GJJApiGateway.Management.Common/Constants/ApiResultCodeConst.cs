using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Common.Constants
{
    /// <summary>
    /// 定义 API 返回结果代码的常量。
    /// </summary>
    public class ApiResultCodeConst
    {
        /// <summary>
        /// 表示 API 调用成功的结果代码常量。
        /// </summary>
        public const int SUCCESS = 1;

        /// <summary>
        /// 表示 API 调用发生错误的结果代码常量。
        /// </summary>
        public const int ERROR = 500;
        /// <summary>
        /// 表示 API 调用发生失败的结果代码常量。
        /// </summary>
        public const int FAIL = -1;
    }
}
