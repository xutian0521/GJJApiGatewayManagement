using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Common.Attributes
{
    /// <summary>
    /// 用于标记不需要进行ApplicationId验证的控制器或方法。
    /// 当此特性应用于一个控制器或方法上时，ApplicationIdMiddleware中间件
    /// 将跳过对该控制器或方法的ApplicationId验证。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SkipApplicationIdValidationAttribute : Attribute
    {
        // 此类作为一个标记特性，不需要添加成员
    }
}
