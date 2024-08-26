#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.WinForms
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：ControlExts.cs
// 创建用户：lanwah
// 创建日期：2024/8/23 18:48:08
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions.Shared.WinForms
{
    /// <summary>
    /// Control 扩展方法
    /// </summary>
    public static class ControlExts
    {
        /// <summary>
        /// 更新UI，非UI线程调用
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        public static void UpdateUI(this Control control, Action action)
        {
            if (control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action?.Invoke();
            }
        }
        /// <summary>
        /// 更新UI，非UI线程调用
        /// </summary>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <param name="args"></param>
        public static void UpdateUI(this Control control, Delegate action, params object[] args)
        {
            if (control.IsDisposed || !control.IsHandleCreated)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(action, args);
            }
            else
            {
                action?.DynamicInvoke(args);
            }
        }
    }
}
#endif
