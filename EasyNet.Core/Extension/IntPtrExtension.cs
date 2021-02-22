using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Extension
// 文件名称：IntPtrExtension
// 创 建 者：lanwah
// 创建日期：2020/11/21 19:12:19
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
    /// IntPtr类型扩展类
    /// </summary>
    public static class IntPtrExtension
    {
        /// <summary>
        /// IntPtr 转 Ansi 字符串
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ToAnsiString(this IntPtr handle, string defaultValue = "")
        {
            if ((null == handle) || (IntPtr.Zero == handle))
            {
                return defaultValue;
            }

            return Marshal.PtrToStringAnsi(handle);
        }
    }
}
