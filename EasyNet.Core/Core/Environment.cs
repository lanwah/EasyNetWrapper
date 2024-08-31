using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Core
// CLR版本：4.0.30319.42000
// 运行要求：3.5
// 文件名称：Env.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 17:24:28
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
    public static partial class Environment
    {
        /// <summary>
        /// 获取当前线程的ManagedThreadId
        /// </summary>
        public static int ManagedThreadId
        {
            get
            {
#if NET8_0_OR_GREATER
                return System.Environment.CurrentManagedThreadId;
#else
                return System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            }
        }
    }
}
