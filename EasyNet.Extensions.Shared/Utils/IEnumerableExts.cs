using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// IEnumerable类型 扩展方法
    /// </summary>
    public static class IEnumerableExts
    {
        /// <summary>
        /// 对象去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="this"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, V>(this IEnumerable<T> @this, Func<T, V> keySelector)
        {
            if (@this.IsNull())
            {
                //如果为空，则返回原集合
                return @this;
            }

            return @this.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
        internal class CommonEqualityComparer<T, V> : IEqualityComparer<T>
        {
            private readonly Func<T, V> keySelector;

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
        /// <returns>true - 有值；false - 无值</returns>
        public static bool HasData<T>(this IEnumerable<T> source)
        {
            if (source.IsNull())
            {
                return false;
            }

#if NET5_0_OR_GREATER
            return source.Any();
#else   
            return (source.Count() > 0);
#endif
        }
        /// <summary>
        /// 判断集合是否无值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>true - 无值；false - 有值</returns>
        public static bool HasNoData<T>(this IEnumerable<T> source)
        {
            return !HasData(source);
        }
    }
}
