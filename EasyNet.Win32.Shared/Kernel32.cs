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
        /// <see keyword="GetLastError 函数" href="https://learn.microsoft.com/zh-cn/windows/win32/api/errhandlingapi/nf-errhandlingapi-getlasterror"/>
        /// <see keyword="系统错误代码" href="https://learn.microsoft.com/zh-cn/windows/win32/debug/system-error-codes--0-499-"/>
        /// <see keyword="百度百科：GetLastError" href="https://baike.baidu.com/item/GetLastError/4278820?fr=ge_ala#ref_1_1730168"/>
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

        #region Q

        /// <summary>
        /// 用于得到高精度计时器的值(如果存在这样的计时器)
        /// </summary>
        /// <param name="performanceCount">指向现时计数器的值.如果安装的硬件不支持高精度计时器,该参数将返回0（输出参数）</param>
        /// <link>https://msdn.microsoft.com/en-us/library/windows/desktop/ms644904(v=vs.85).aspx</link>
        /// <linkalso>http://baike.baidu.com/link?url=orq-Nu0ORaUmxrPg4Gcnc7gJ0A1KcVvs7gQ2szdUy9Ej1cHelq3D792B23Xd_P1CwmRH_L4YgplCcIIt-zNOza</linkalso>
        /// <returns>true： 硬件支持高精度计数器；false： 硬件不支持，读取失败</returns>
        [DllImport("Kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
        public static extern bool QueryPerformanceCounter(out Int64 performanceCount);
        /// <summary>
        /// 返回硬件支持的高精度计数器的频率
        /// </summary>
        /// <param name="frequency">高精度计数器每秒的计数值（输出参数）</param>
        /// <link>https://msdn.microsoft.com/en-us/library/windows/desktop/ms644905(v=vs.85).aspx</link>
        /// <linkalso>http://www.baike.com/wiki/QueryPerformanceFrequency()</linkalso>
        /// <returns>true： 硬件支持高精度计数器；false： 硬件不支持，读取失败</returns>
        [DllImport("Kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out Int64 frequency);
        #endregion
    }
}
