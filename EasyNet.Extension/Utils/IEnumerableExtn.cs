using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：IEnumerableExtn.cs
// 创 建 者：lanwah
// 创建日期：2022/7/2 9:23:21
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extension
{
    /// <summary>
    /// IEnumerable 扩展方法
    /// </summary>
    public static class IEnumerableExtn
    {
        /// <summary>
        /// 对象去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            source.NotNullCheck(nameof(source));

            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
        internal class CommonEqualityComparer<T, V> : IEqualityComparer<T>
        {
            private Func<T, V> keySelector;

            public CommonEqualityComparer(Func<T, V> keySelector)
            {
                this.keySelector = keySelector;
            }

            public bool Equals(T x, T y)
            {
                return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
            }
        }

        /// <summary>
        /// 判断集合是否有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasData<T>(this IEnumerable<T> source)
        {
            if (source.IsNull())
            {
                return false;
            }

            return (source.Count() > 0);
        }
    }
}
