using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// 字符串包含比较器
    /// </summary>

#if NET8_0_OR_GREATER
    public class StringContainsComparator(string source, string target) : StringComparator(source, target)
    {

#else
    public class StringContainsComparator : StringComparator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public StringContainsComparator(string source, string target) : base(source, target)
        {
        }

#endif
        /// <summary>
        /// 判断 Source 中是否包含 Target
        /// </summary>
        public override bool IsMatch => (this.Source ?? "").Contains(this.Target);
    }
}
