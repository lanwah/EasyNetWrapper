using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// 判断字符串是否为空，包含空格
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 为空，否则有值</returns>
        public static bool IsNullOrEmptyEx(this string @this)
        {
            if ((@this.IsNullOrEmpty()) || (@this.Trim().Length == 0))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 同 <see cref="string.IsNullOrEmpty"/>
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
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
        public static T ConvertFromString<T>(this string value)
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
    }
}
