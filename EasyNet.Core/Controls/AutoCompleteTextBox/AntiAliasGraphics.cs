using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AntiAliasGraphics
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:19:46
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Controls
{
    internal class AntiAliasGraphics : IDisposable
    {
        private GraphicsState _state;
        private Graphics _graphics;

        public AntiAliasGraphics(Graphics graphics, SmoothingMode mode)
        {
            _state = graphics.Save();
            _graphics = graphics;
            graphics.SmoothingMode = mode;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _graphics.Restore(_state);
        }

        #endregion
    }
}
