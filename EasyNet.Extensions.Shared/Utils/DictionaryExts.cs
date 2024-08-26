using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：IntPtrExtn
// 创 建 者：lanwah
// 创建日期：2022/7/2 9:41:18
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// Dictionary类型 扩展方法
    /// </summary>
    public static class DictionaryExts
    {
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TKey">Key类型</typeparam>
        /// <typeparam name="TValue">Value类型</typeparam>
        /// <param name="this">字典对象</param>
        /// <param name="key">要获取的字典键值</param>
        /// <param name="defaultValue">默认值，用于对应的key没有取到值时，返回的是默认值</param>
        /// <returns>key（键）对应的值或默认值（key对应的值不存在时返回默认值）</returns>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue defaultValue = default)
        {
            var value = @this.TryGetValue(key, out var result) ? result : defaultValue;
            return value;
        }
        /// <summary>
        /// 获取值类型字典值，返回值类型的可空对象
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this">字典对象</param>
        /// <param name="key">要获取的字典键值</param>
        /// <returns><paramref name="key"/>键对应的值类型值。</returns>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TValue? GetValue<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key) where TValue : struct
        {
            //TValue? val = null;
            //if ((null != @this) && (@this.ContainsKey(key)))
            //{
            //    var value = @this[key];
            //    val = value;
            //}

            var val = @this.TryGetValue(key, out var value) ? value : (TValue?)null;
            return val;
        }
        ///// <summary>
        ///// 获取引用类型字典值，<see cref="GetValue{TKey, TValue}(Dictionary{TKey, TValue}, TKey, TValue)"/>中已经实现了此函数的功能，因此不开放此函数。
        ///// </summary>
        ///// <typeparam name="TKey"></typeparam>
        ///// <typeparam name="TValue"></typeparam>
        ///// <param name="this"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static TValue GetRTValue<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key) where TValue : class
        //{
        //    return (null == @this) ? null : (@this.ContainsKey(key) ? @this[key] : null);
        //}
    }
}
