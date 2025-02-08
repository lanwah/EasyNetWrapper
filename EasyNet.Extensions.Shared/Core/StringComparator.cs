using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// 字符串比较器
    /// </summary>
#if NET8_0_OR_GREATER
    public class StringComparator(string source, string target) : IStringComparator
    {
        /// <summary>
        /// 源字符串
        /// </summary>
        public string Source { get => source; set => source = value; }
        /// <summary>
        /// 目标字符串
        /// </summary>
        public string Target { get => target; set => target = value; }

#else
    public class StringComparator : IStringComparator
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public StringComparator(string source, string target)
        {
            this.Source = source;
            this.Target = target;
        }
        /// <summary>
        /// 源字符串
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 目标字符串
        /// </summary>
        public string Target { get; set; }
#endif

        /// <summary>
        /// 比较两个字符串是否相同
        /// </summary>
        public virtual bool IsMatch => (this.Source.Compare(this.Target));
    }
}
