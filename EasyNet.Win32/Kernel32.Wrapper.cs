using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
    }
}
