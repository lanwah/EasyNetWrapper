using System;
using System.Collections.Generic;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.Utils
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：StringIOExts.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 11:03:26
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    public static partial class StringExts
    {
        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="this">要测试的路径（输入参数）</param>
        /// <returns>true： 目录存在；false： 目录不存在</returns>
        public static bool IsDirectoryExists(this string @this)
        {
            return System.IO.Directory.Exists(@this);
        }
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="this">文件的完整路径（输入参数）</param>
        /// <returns>true： 文件存在；false： 文件不存在</returns>
        public static bool IsFileExists(this string @this)
        {
            return System.IO.File.Exists(@this);
        }
        /// <summary>
        /// 判断文件是否有扩展名
        /// </summary>
        /// <param name="this">文件完整路径（输入参数）</param>
        /// <returns>true： 文件有扩展名；false： 文件没有扩展名</returns>
        public static bool HasExtension(this string @this)
        {
            return System.IO.Path.HasExtension(@this);
        }
    }
}
