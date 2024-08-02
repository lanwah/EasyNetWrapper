using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Extensions.Shared.Utils
{
    /// <summary>
    /// Char类型 扩展方法
    /// </summary>
    public static partial class CharExts
    {
        /// <summary>
        /// 把char数组转成字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns>char数组对应的字符串</returns>
        public static string ItemsToString(this char[] @this)
        {
            // 把chars的内容转成字符串
            return new string(@this);
        }
    }
}
