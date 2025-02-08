#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace EasyNet.Core
{
    /// <summary>
    /// 抗锯齿画刷
    /// </summary>
    public class AntiAliasGraphics : SaveStateGraphics
    {
        /// <summary>
        /// 构造函数，保存状态并设置抗锯齿模式
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="mode"></param>
        public AntiAliasGraphics(Graphics graphics, SmoothingMode mode): base(graphics)
        {
            this.Graphics.SmoothingMode = mode;
        }
    }
}
#endif
