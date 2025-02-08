using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// 字符串比较器接口
    /// </summary>
    public interface IStringComparator
    {
        /// <summary>
        /// 源字符串
        /// </summary>
        string Source { get; set; }
        /// <summary>
        /// 目标字符串
        /// </summary>
        string Target { get; set; }

        /// <summary>
        /// 比较两个字符串是否相同
        /// </summary>
        bool IsMatch { get; }
    }  
}
