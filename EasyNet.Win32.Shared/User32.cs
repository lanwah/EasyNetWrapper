using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EasyNet.Win32
{
    /// <summary>
    /// user32 相关方法
    /// </summary>
    public partial class User32
    {
        /// <summary>
        /// user32.dll
        /// </summary>
        internal const string USER32 = "user32.dll";


        #region M
        /// <summary>
        /// 鼠标事件，此函数综合了鼠标移动和按钮点击，此函数已被取代。改用<see cref="SendInput"/>。
        /// </summary>
        /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event"/>
        /// <param name="dwFlags"><see cref="MouseEventFlag"/>事件标志，可以是几个事件的组合，如 MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP</param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport(USER32)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, UIntPtr dwExtraInfo);
        #endregion

        #region S
        /// <summary>
        /// 该函数合成键盘事件和鼠标事件，用来模拟鼠标或者键盘操作。事件将被插入在鼠标处理队列里面。
        /// </summary>
        /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendinput"/>
        /// <param name="nInputs">指定ninput 数组中元素的个数。就是插入事件的个数。</param>
        /// <param name="pInputs">指向一个类型为<see cref="INPUT"/>的数组变量，该数组中的每个元素代表一个将要插入到线程事件中去的键盘或鼠标事件。</param>
        /// <param name="cbSize">指定INPUT结构的大小。如果cbSize不是INPUT结构的大小，则函数将失败返回。</param>
        /// <returns>成功插入了多少个操作事件。如果插入出错可以利用GetLastError来查看错误类型。</returns>
        [DllImport(USER32)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
        #endregion
    }
}
