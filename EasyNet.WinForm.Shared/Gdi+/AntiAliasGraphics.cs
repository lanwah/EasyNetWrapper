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

    /// <summary>
    /// 保存状态的画刷
    /// </summary>
    public class SaveStateGraphics : IDisposable
    {
        /// <summary>
        /// 保存的状态，用于恢复
        /// </summary>
        protected GraphicsState State
        {
            get; set;
        }
        /// <summary>
        /// 画刷
        /// </summary>
        protected Graphics Graphics
        {
            get; set;
        }

        /// <summary>
        /// 构造函数，保存状态并设置抗锯齿模式
        /// </summary>
        /// <param name="graphics"></param>
        public SaveStateGraphics(Graphics graphics)
        {
            this.State = graphics.Save();
            this.Graphics = graphics;
        }

        #region IDisposable 成员

        /// <summary>
        /// 恢复状态
        /// </summary>
        public void Dispose()
        {
            this.Graphics.Restore(this.State);
        }

        #endregion
    }
}
#endif
