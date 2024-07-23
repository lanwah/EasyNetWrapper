using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Win32
{
    /// <summary>
    /// 鼠标事件标志
    /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event"/>
    /// </summary>
    public struct MouseEventFlag
    {
        /// <summary>
        /// 移动鼠标
        /// </summary>
        public const uint MOUSEEVENTF_MOVE = 0x0001;
        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        /// <summary>
        /// 鼠标左键弹起
        /// </summary>
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        /// <summary>
        /// 鼠标右键弹起
        /// </summary>
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        /// <summary>
        /// 鼠标中键按下
        /// </summary>
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        /// <summary>
        /// 鼠标中键弹起
        /// </summary>
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        /// <summary>
        /// XDOWN
        /// </summary>
        public const uint MOUSEEVENTF_XDOWN = 0x0080;
        /// <summary>
        /// XUP
        /// </summary>
        public const uint MOUSEEVENTF_XUP = 0x0100;
        /// <summary>
        /// WHEEL
        /// </summary>
        public const uint MOUSEEVENTF_WHEEL = 0x0800;
        /// <summary>
        /// HWHEEL
        /// </summary>
        public const uint MOUSEEVENTF_HWHEEL = 0x01000;
        /// <summary>
        /// 标示是否采用绝对坐标
        /// </summary>
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
    }
}
