using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutoCompleteEvent
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:23:41
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
    public delegate bool DialogKeyProcessor(Keys keyData);

    public class SelectingEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public bool Cancel { get; set; }
        public int SelectedIndex { get; set; }
        public bool Handled { get; set; }
    }
    public class SelectedEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public Control Control { get; set; }
    }
    public class PaintItemEventArgs : PaintEventArgs
    {
        public RectangleF TextRect { get; internal set; }
        public StringFormat StringFormat { get; internal set; }
        public Font Font { get; internal set; }
        public bool IsSelected { get; internal set; }
        public bool IsHovered { get; internal set; }

        public PaintItemEventArgs(Graphics graphics, Rectangle clipRect)
            : base(graphics, clipRect)
        {
        }
    }
}
