using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：DictionaryExtn
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

namespace EasyNet.Extension
{
    /// <summary>
    /// Dictionary 扩展方法
    /// </summary>
    public static class DictionaryExtn
    {
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TKey">Key类型</typeparam>
        /// <typeparam name="TValue">Value类型</typeparam>
        /// <param name="dictionary">字典对象</param>
        /// <param name="key">要获取的字典键值</param>
        /// <param name="defaultValue">默认值，用于对应的key没有取到值时，返回的是默认值</param>
        /// <returns>key（键）对应的值或默认值（key对应的值不存在时返回默认值）</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            var value = dictionary.TryGetValue(key, out var result) ? result : defaultValue;
            return value;
        }
    }
}
