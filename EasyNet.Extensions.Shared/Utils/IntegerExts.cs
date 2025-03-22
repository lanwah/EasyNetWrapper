using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// Integer类型 扩展方法
    /// </summary>
    public static class IntegerExts
    {
        /// <summary>
        /// 把小于16的数字转换成等效的16进制字符
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static char GetHexValue(this int @this)
        {
            if (@this > 15)
            {
                throw new ArgumentException("The number is greater than 15.");
            }

            if (@this < 10)
            {
                return (char)(@this + 48);
            }

            return (char)(@this - 10 + 65);
        }
    }
}
