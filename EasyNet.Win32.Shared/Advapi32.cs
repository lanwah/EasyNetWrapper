using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Win32.Shared
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：Advapi32.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 10:48:21
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Win32.Shared
{
    /// <summary>
    /// Advapi32 相关方法
    /// </summary>
    public partial class Advapi32
    {
        /// <summary>
        /// Advapi32.dll
        /// </summary>
        internal const string ADVAPI32 = "advapi32.dll";

        #region Q

        /// <summary>
        /// 查询服务相关信息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="infoLevel"></param>
        /// <param name="buffer"></param>
        /// <param name="bufSize"></param>
        /// <param name="bytesNeeded"></param>
        /// <link>https://msdn.microsoft.com/en-us/library/windows/desktop/ms684935(v=vs.85).aspx</link>
        /// <returns></returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(ADVAPI32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool QueryServiceConfig2(SafeHandle service, int infoLevel, IntPtr buffer, int bufSize, ref int bytesNeeded);
        #endregion
    }
}
