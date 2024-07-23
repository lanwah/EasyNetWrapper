using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions
// 文件名称：ByteExts
// 创 建 者：lanwah
// 创建日期：2024年7月23日
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
    /// Byte类型 扩展方法
    /// </summary>
    public static class ByteExts
    {
        /// <summary>
        /// 比较两个List&lt;byte&gt;的内容是否相同，此函数的效率比<see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>快
        /// </summary>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <benchmark>\Comparer\SequenceEqual.cs</benchmark>
        /// <returns></returns>
        public static bool IsSequenceEqual(this List<byte> @this, List<byte> other)
        {
            if (@this.Count != other.Count)
            {
                return false;
            }

            for (var i = 0; i < @this.Count; i++)
            {
                if (@this[i] == (other[i]))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 比较两个byte[]的内容是否相同，此函数的效率比<see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>快
        /// </summary>
        /// <param name="this"></param>
        /// <param name="other"></param>
        /// <benchmark>\Comparer\SequenceEqual.cs</benchmark>
        /// <returns></returns>
        public static bool IsSequenceEqual(this byte[] @this, byte[] other)
        {
            if (@this.Length != other.Length)
            {
                return false;
            }

            for (var i = 0; i < @this.Length; i++)
            {
                if (@this[i] == (other[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
