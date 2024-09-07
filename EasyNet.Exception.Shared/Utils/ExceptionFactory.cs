using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Exception.Utils
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：ExceptionFactory.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 11:22:00
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core
{
    /// <summary>
    /// 异常工厂，用于创建统一的异常信息
    /// </summary>
    public static class ExceptionFactory
    {
        /// <summary>
        /// 目录不存在异常
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static Exception NewDirectoryNotFoundException(string directory)
        {
            var message = $"本地目录 '{directory}' 不存在！";
            return new DirectoryNotFoundException(message);
        }
    }
}
