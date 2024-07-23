using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.Net.WebRequestMethods;

namespace EasyNet.Win32
{
    /// <summary>
    /// Kernel32 相关方法封装
    /// </summary>
    public partial class Kernel32
    {
        [System.Security.SecurityCritical]  // auto-generated
        internal static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            // GetModuleHandle does not increment the module's ref count, so we don't need to call FreeLibrary.
            var hModule = GetModuleHandle(moduleName);
            if (hModule == IntPtr.Zero)
            {
                Debug.Assert(hModule != IntPtr.Zero, "GetModuleHandle failed.  Dll isn't loaded?");
                return false;
            }
            var functionPointer = GetProcAddress(hModule, methodName);
            return functionPointer != IntPtr.Zero;
        }

        /// <summary>
        /// 判断是否为64位操作系统，.NET4.0以下可以用此方法进行判断，.NET4.0以上可以用 Environment.Is64BitOperatingSystem 属性。
        /// <see href="https://referencesource.microsoft.com/#mscorlib/system/environment.cs,1360"/>
        /// <see langword="C#如何判断操作系统位数是32位还是64位" href="https://www.cnblogs.com/czzju/articles/2482474.html"/>
        /// </summary>
        public static bool Is64BitOperatingSystem
        {
            [System.Security.SecuritySafeCritical]
            get
            {
#if WIN32
                bool isWow64; // WinXP SP2+ and Win2k3 SP1+
                return DoesWin32MethodExist(KERNEL32, "IsWow64Process")
                    && IsWow64Process(GetCurrentProcess(), out isWow64)
                    && isWow64;
#else
                // 64-bit programs run only on 64-bit
                //<
                return true;
#endif
            }
        }
    }
}
