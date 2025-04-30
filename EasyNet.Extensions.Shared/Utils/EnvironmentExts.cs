using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.WinForm.Shared
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：Env.cs
// 创建用户：lanwah
// 创建日期：2024/8/23 18:12:34
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
    /// 系统Environment类扩展
    /// </summary>
    public static partial class Environment
    {
        /// <summary>
        /// 判断是否处于设计模式
        /// </summary>
        /// <param name="component"></param>
        /// <returns>true - 处于设计模式；false - 运行模式</returns>
        public static bool IsDesignMode(this Component component)
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                //design mode
                return true;
            }
            else if (Process.GetCurrentProcess().ProcessName == "devenv")
            {
                //design mode
                return true;
            }

            if (null != component)
            {
                object result = null;
                var methodInfo = component.GetType().GetMethod("GetService", BindingFlags.Instance | BindingFlags.NonPublic);
                if (null != methodInfo)
                {
#if NET8_0_OR_GREATER
                    result = methodInfo.Invoke(component, [typeof(IDesignerHost)]);
#else

                    result = methodInfo.Invoke(component, new object[] { typeof(IDesignerHost) });
#endif
                }
                if (result != null)
                {
                    //design mode
                    return true;
                }
            }

            //runtime mode
            return false;
        }
        /// <summary>
        /// 判断程序是否处于调试模式
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsDebugMode(this object @this)
        {
            return Environment.IsDebuggerAttached;
        }
        /// <summary>
        /// 判断程序是否处于调试模式
        /// </summary>
        public static bool IsDebuggerAttached
        {
            get
            {
                var isDebug = false;
                try
                {
                    isDebug = System.Diagnostics.Debugger.IsAttached;
                }
                catch
                {
                    // ignored
                }
                return isDebug;
            }
        }
    }
}
