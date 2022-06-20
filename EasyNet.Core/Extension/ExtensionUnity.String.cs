using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Extension
// 文件名称：ExtensionUnity
// 创 建 者：lanwah
// 创建日期：2022/3/19 11:26:03
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Extension
{
    /// <summary>
    /// String 扩展方法
    /// </summary>
    public static partial class ExtensionUnity
    {
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
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyEx(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return true;
            }

            return false;
        }

        #region // 文件路径相关

        /// <summary>
        /// 读取文件的二进制内容
        /// </summary>
        /// <param name="filePath">文件完整路径</param>
        /// <returns>二进制文件数据副本</returns>
        public static byte[] ReadFileBytes(this string filePath)
        {
            byte[] bytes = null;
            if (filePath.IsNullOrEmptyEx() || !File.Exists(filePath))
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
        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool IsFileUsing(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                // 文件不存在
                return true;
            }

            var used = true;
            FileStream fs = null;

            try
            {

                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

                Debug.WriteLine("file not used.");
                used = false;
            }
            catch
            {
                Debug.WriteLine("file is using.");
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
        #endregion
    }
}
