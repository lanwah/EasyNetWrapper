using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace EasyNet.Win32
{
    /// <summary>
    /// Kernel32 相关方法
    /// </summary>
    public partial class Kernel32
    {
        /// <summary>
        /// kernel32.dll
        /// </summary>
        internal const String KERNEL32 = "kernel32.dll";


        #region G
        [DllImport(KERNEL32, CharSet = CharSet.Auto, SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport(KERNEL32, CharSet = CharSet.Auto, BestFitMapping = false, SetLastError = true)]
        [ResourceExposure(ResourceScope.Process)]  // Is your module side-by-side?
        internal static extern IntPtr GetModuleHandle(String moduleName);

        // Note - do NOT use this to call methods.  Use P/Invoke, which will
        // do much better things w.r.t. marshaling, pinning memory, security 
        // stuff, better interactions with thread aborts, etc.  This is used
        // solely by DoesWin32MethodExist for avoiding try/catch EntryPointNotFoundException
        // in scenarios where an OS Version check is insufficient
        [DllImport(KERNEL32, CharSet = CharSet.Ansi, BestFitMapping = false, SetLastError = true, ExactSpelling = true)]
        [ResourceExposure(ResourceScope.None)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, String methodName);
        /// <summary>
        /// 检索调用线程的最后错误代码值。 最后一个错误代码按线程进行维护。 多个线程不会覆盖彼此的最后错误代码。
        /// <see langword="GetLastError 函数" href="https://learn.microsoft.com/zh-cn/windows/win32/api/errhandlingapi/nf-errhandlingapi-getlasterror"/>
        /// <see langword="系统错误代码" href="https://learn.microsoft.com/zh-cn/windows/win32/debug/system-error-codes--0-499-"/>
        /// <see langword="百度百科：GetLastError" href="https://baike.baidu.com/item/GetLastError/4278820?fr=ge_ala#ref_1_1730168"/>
        /// </summary>
        /// <returns></returns>
        [DllImport(KERNEL32)]
        internal static extern uint GetLastError();
        #endregion

        #region I
        [DllImport(KERNEL32, SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [ResourceExposure(ResourceScope.Machine)]
        internal static extern bool IsWow64Process([In] IntPtr hSourceProcessHandle, [Out, MarshalAs(UnmanagedType.Bool)] out bool isWow64);
        #endregion
    }
}
