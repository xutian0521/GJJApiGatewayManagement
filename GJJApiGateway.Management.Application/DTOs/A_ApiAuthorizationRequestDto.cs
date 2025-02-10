using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_ApiAuthorizationRequestDto
    {
        /// <summary>
        /// 要授权的API的ID集合。
        /// </summary>
        public List<int>? ApiIds { get; set; }

        /// <summary>
        /// 要授权给的应用程序的ID集合。
        /// </summary>
        public List<int>? ApplicationIds { get; set; }

        /// <summary>
        /// 授权持续时间，单位为天。如果未提供，则表示永久授权。
        /// </summary>
        public int? Days { get; set; }
    }
}
