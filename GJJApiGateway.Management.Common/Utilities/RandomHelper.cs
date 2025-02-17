using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Common.Utilities
{
    public static class RandomHelper
    {
        /// <summary>
        /// 创建一个 4 位数的随机数字字符串
        /// </summary>
        /// <returns>返回 0000-9999 之间的随机数（字符串格式）</returns>
        public static string CreateRandomNumber()
        {
            Random ran = new Random();
            int number = ran.Next(0, 9999);
            return number.ToString("D4"); // 直接用 "D4" 让数字始终保持 4 位
        }
    }
}
