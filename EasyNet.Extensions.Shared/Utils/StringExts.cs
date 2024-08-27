using EasyNet.Extensions.Shared.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：StringExts
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
    /// String类型 扩展方法
    /// </summary>
    public static partial class StringExts
    {
        /// <summary>
        /// 判断字符串是否为空或者空字符，可以判断连续的空字符
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <benchmark>\Comparer\StringNull.cs</benchmark>
        /// <returns>true - 为空，否则有值</returns>
        public static bool IsNullOrEmptyEx(this string @this)
        {
            if (@this.IsNullOrEmpty())
            {
                return true;
            }

            // @this有值时，@this?.Length大于0，此时(@this?.Length > 0)返回true，然后继续判断是否都为空字符
            // 只包含空格的情况
            for (int i = 0; i < @this.Length; i++)
            {
                if (!char.IsWhiteSpace(@this[i]))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 判断字符串是否为空，不可判断多个连续的空格，需要判断连续的空字符请用<see cref="IsNullOrEmptyEx"/>
        /// </summary>
        /// <param name="this"></param>
        /// <benchmark>\Comparer\StringNull.cs</benchmark>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string @this)
        {
            // 判断 Null和string.Empty的情况
            // @this为null时@this?.Length为null，(null > 0) = false
            // @this为string.Empty时@this?.Length为0，(0 > 0) = false
            return !(@this?.Length > 0);
        }
        /// <summary>
        /// 判断字符串是否非空，连续的空格此函数返回的是true
        /// </summary>
        /// <param name="this"></param>
        /// <returns>true - 有值，否则为空</returns>
        public static bool IsNotNullOrEmpty(this string @this)
        {
            return !@this.IsNullOrEmpty();
        }


        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath">文件的完整路径</param>
        /// <returns>true - 文件存在，否则不存在</returns>
        public static bool IsFileExist(this string filePath)
        {
            if (filePath.IsNullOrEmpty())
            {
                return false;
            }

            return File.Exists(filePath);
        }
        /// <summary>
        /// 读取文件的二进制内容
        /// </summary>
        /// <param name="filePath">文件的完整路径</param>
        /// <returns>文件内容的 byte[]</returns>
        public static byte[] ReadFileBytes(this string filePath)
        {
            byte[] bytes = null;
            if (!filePath.IsFileExist())
            {
                return bytes;
            }

            using (var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                bytes = reader.ToBytes();
            }

            return bytes;
        }

#if NET40_OR_GREATER
        /// <summary>
        /// 判断是否为目录
        /// </summary>
        /// <param name="filePath">文件的完整路径</param>
        /// <returns>true - 是目录，否则为文件</returns>
        public static bool IsDirectory(this string filePath)
        {
            if (filePath.IsNullOrEmpty())
            {
                return false;
            }

            // get the file attributes for file or directory
            var attr = File.GetAttributes(filePath);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
#endif
        /// <summary>
        /// 获取路径中最后一部分的名称（文件名或文件夹名）。
        /// </summary>
        /// <param name="filePath">文件的完整路径</param>
        /// <returns>文件名或文件夹名</returns>
        public static string GetLastNameOfPath(this string filePath)
        {
            if (filePath.IsNullOrEmpty())
            {
                return string.Empty;
            }

            // 正则说明：
            // [^/\\]   ：表示匹配除了斜杠(/)和反斜杠(\)以外的任意字符，双反斜杠用于转义
            // +        ：表示匹配前面的表达式一次或多次
            // [/\\]    ：表示匹配斜杠(/)或反斜杠(\)
            // *        ：表示匹配零次或多次
            // $        ：表示从后向前匹配

            // 截取最后一部分名称，名称的末尾可能带有多个斜杠(/)或反斜杠(\)
            var pattern = @"[^/\\]+[/\\]*$";
            var match = System.Text.RegularExpressions.Regex.Match(filePath, pattern);
            var name = match.Value;

            // 截取名称中不带斜杠(/)或反斜杠(\)的部分
            pattern = @"[^/\\]+";
            match = System.Text.RegularExpressions.Regex.Match(name, pattern);
            name = match.Value;

            return name;
        }
        /// <summary>
        /// 获取目录下的文件和文件夹
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static List<string> GetFilesAndDirectories(this string directory)
        {
            var fileList = new List<string>();
            var files = Directory.GetFiles(directory);
            if (files.IsNotNull())
            {
                fileList.AddRange(files);
            }
            var direcotries = Directory.GetDirectories(directory);
            if (direcotries.IsNotNull())
            {
                fileList.AddRange(direcotries);
            }

            return fileList;
        }

        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this string value)
        {
            return ValueConverter.ConvertFromString<T>(value);
        }

        /// <summary>
        /// 通过 TypeConverter 实现值与字符串值之间的转换
        /// </summary>
        internal class ValueConverter
        {
            /// <summary>
            /// 获取提示信息
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static string GetNotSupportedMessage(Type type)
            {
                return $"不支持转换，可能原因是未找到{type}类型的转换器，请实现转换器类（继承TypeConverter类并重写相关接口）。";
            }

            /// <summary>
            /// 字符串转对象
            /// </summary>
            /// <see cref="TypeConverter.ConvertFrom(ITypeDescriptorContext, System.Globalization.CultureInfo, object)"/>
            /// <typeparam name="T">类型</typeparam>
            /// <param name="value">字符串类型值</param>
            /// <returns>类型值</returns>
            public static T ConvertFromString<T>(string value)
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
            }
            /// <summary>
            /// 对象转换成字符串
            /// </summary>
            /// <see cref="TypeConverter.ConvertTo(ITypeDescriptorContext, System.Globalization.CultureInfo, object, Type)"/>
            /// <see cref="Int32Converter"/>
            /// <see cref="BaseNumberConverter"/>
            /// <param name="value">类型值</param>
            /// <returns>对象的字符串表示形式</returns>
            public static string ConvertToString(object value)
            {
                return TypeDescriptor.GetConverter(value.GetType()).ConvertToString(value);
            }
        }

        /// <summary>
        /// 将字符串转换为bool类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string @this, bool defaultValue = default)
        {
            if (bool.TryParse(@this, out bool val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为sbyte类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static sbyte ToSByte(this string @this, sbyte defaultValue = default)
        {
            if (sbyte.TryParse(@this, out sbyte val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为byte类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte ToByte(this string @this, byte defaultValue = default)
        {
            if (byte.TryParse(@this, out byte val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为char类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static char ToChar(this string @this, char defaultValue = default)
        {
            if (@this.Length == 1)
            {
                return @this[0];
            }
            else if (char.TryParse(@this, out char val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为short类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short ToInt16(this string @this, short defaultValue = default)
        {
            if (short.TryParse(@this, out short val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为ushort类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ushort ToUInt16(this string @this, ushort defaultValue = default)
        {
            if (ushort.TryParse(@this, out ushort val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为int类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt32(this string @this, int defaultValue = default)
        {
            if (int.TryParse(@this, out int val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为uint类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static uint ToUInt32(this string @this, uint defaultValue = default)
        {
            if (uint.TryParse(@this, out uint val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为long类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToInt64(this string @this, long defaultValue = default)
        {
            if (long.TryParse(@this, out long val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为ulong类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ToUInt64(this string @this, ulong defaultValue = default)
        {
            if (ulong.TryParse(@this, out ulong val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为float类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToSingle(this string @this, float defaultValue = default)
        {
            if (float.TryParse(@this, out float val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为double类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string @this, double defaultValue = default)
        {
            if (double.TryParse(@this, out double val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为decimal类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string @this, decimal defaultValue = default)
        {
            if (decimal.TryParse(@this, out decimal val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为DateTime类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string @this, DateTime defaultValue = default)
        {
            if (DateTime.TryParse(@this, out DateTime val))
            {
                return val;
            }
            return defaultValue;
        }
        /// <summary>
        /// 将字符串转换为TimeSpan类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string @this, TimeSpan defaultValue = default)
        {
            if (TimeSpan.TryParse(@this, out TimeSpan val))
            {
                return val;
            }
            return defaultValue;
        }
#if NET40_OR_GREATER
        /// <summary>
        /// 将字符串转换为Guid类型
        /// </summary>
        /// <param name="this"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string @this, Guid defaultValue = default)
        {
            if (Guid.TryParse(@this, out Guid val))
            {
                return val;
            }
            return defaultValue;
        }
#endif

        /// <summary>
        /// 去除字符串末尾的换行符
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string TrimEndNewLine(this string @this)
        {
            var trimChars = ConstVar.CRLF;
            if (@this.IsNullOrEmpty())
            {
                return @this;
            }

            return @this.TrimEnd(trimChars);
        }
    }
}
