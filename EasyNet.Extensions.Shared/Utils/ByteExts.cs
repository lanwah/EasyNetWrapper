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
        /// <summary>
        /// 将指定的字节数组的每个元素的数值转换为它的等效十六进制字符串表示形式
        /// </summary>
        /// <param name="this">字节数组。（输入参数）</param>
        /// <param name="separator">分隔符。（可选参数，默认为“-”）</param>
        /// <returns>十六进制对构成的 System.String，其中每一对表示 value 中对应的元素；例如“7F2C4A”。</returns>
        public static string ToHexString(this byte[] @this, string separator = "-")
        {
            var hexString = System.BitConverter.ToString(@this);
            if (separator == "-")
            {
                return hexString;
            }

            return hexString.Replace("-", separator);
        }
        /// <summary>
        /// 将指定的十六进制字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static byte[] FromHexString(this string hexString, string separator = "-")
        {
#if NET5_0_OR_GREATER

            if (separator.IsNullOrEmpty())
            {
                return Enumerable.Range(0, hexString.Length)
                   .Where(x => x % 2 == 0)
                   .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                   .ToArray();
            }
            else if (separator == "-")
            {
                return System.Convert.FromHexString(hexString);
            }

            return System.Convert.FromHexString(hexString.Replace(separator, "-"));
#else
            if (separator.IsNullOrEmpty())
            {
                return Enumerable.Range(0, hexString.Length)
                   .Where(x => x % 2 == 0)
                   .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                   .ToArray();
            }
            else
            {
                if (separator != "-")
                {
                    hexString = hexString.Replace(separator, "-");
                }

                return Enumerable.Range(0, hexString.Length)
                   .Where(x => x % 3 == 0)
                   .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                   .ToArray();
            }
#endif
        }

        /// <summary>
        /// byte[] 转 字符串
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetString(this byte[] @this, Encoding encoding = null)
        {
            if (@this.IsNull())
            {
                return string.Empty;
            }

            if (encoding.IsNull())
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetString(@this);
        }
        /// <summary>
        /// 字符串转 byte[]
        /// </summary>
        /// <param name="this"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string @this, Encoding encoding = null)
        {
            if (@this.IsNullOrEmpty())
            {
#if NET8_0_OR_GREATER
                return [];
#elif NET5_0_OR_GREATER
                return Array.Empty<byte>();
#else
                return new byte[0];
#endif
            }

            if (encoding.IsNull())
            {
                encoding = Encoding.UTF8;
            }

            return encoding.GetBytes(@this);
        }

        /// <summary>
        /// 将指定的字节数组转换为其等效的 Base64 字符串表示形式。
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] @this)
        {
            if (@this.IsNull())
            {
                return string.Empty;
            }

            return Convert.ToBase64String(@this);
        }
        /// <summary>
        /// 将指定的 Base64 字符串转换为等效的字节数组。
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(this string @this)
        {
            if (@this.IsNullOrEmpty())
            {
#if NET8_0_OR_GREATER
                return [];
#elif NET5_0_OR_GREATER
                return Array.Empty<byte>();
#else
                return new byte[0];
#endif
            }

            return Convert.FromBase64String(@this);
        }
    }
}
