using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

namespace EasyNet.Extension
{
    /// <summary>
    /// String 扩展方法
    /// </summary>
    public static class StringExtn
    {
        /// <summary>
        /// 检查参数是否空字符，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        public static void NotNullOrEmptyCheck(this string argumentValue, string argumentName)
        {
            NotNullOrEmpty(argumentValue, argumentName);
        }
        /// <summary>
        /// 检查参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        /// <see cref="nameof"/>
        /// <exception cref="ArgumentNullException"></exception>
        internal static void NotNullOrEmpty(string argumentValue, string argumentName)
        {
            if (argumentValue.IsNullOrEmptyEx())
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyEx(this string value)
        {
            if ((value == null) || (value.Length == 0) || (value.Trim().Length == 0))
            {
                return true;
            }

            return false;
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
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileExist(this string filePath)
        {
            if (filePath.IsNullOrEmptyEx())
            {
                return false;
            }

            return File.Exists(filePath);
        }

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns><see cref="true"/> - 被占用，否则未被占用</returns>
        public static bool IsFileUsing(this string filePath)
        {
            if (!filePath.IsFileExist())
            {
                // 文件不存在
                return false;
            }

            var used = true;
            FileStream fs = null;

            try
            {

                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

                used = false;
            }
            catch
            {
                // file is using.
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return used;
        }

        /// <summary>
        /// 读取文件的二进制内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>文件内容byte数组</returns>
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

                reader.Close();
            }

            return bytes;
        }
    }
}
