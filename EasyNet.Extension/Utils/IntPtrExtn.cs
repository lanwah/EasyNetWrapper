using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// IntPtr 扩展方法
    /// </summary>
    public static class IntPtrExtn
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
