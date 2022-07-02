using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutoCompleteDataLabel
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:30:13
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
    /// <summary>
    /// 数据标签（显示的数据文本）
    /// </summary>
    internal class AutoCompleteDataLabel
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text
        {
            get;
            set;
        }
        /// <summary>
        /// 显示区域
        /// </summary>
        public Rectangle Bound
        {
            get
            {
                Rectangle rect = Owner.DataRect;
                int height = Owner.Font.Height + 6;
                return new Rectangle(rect.X + 3, rect.Y + (rect.Height - height) / 2, rect.Width - 7, height);
            }
        }

        private AutocompleteItem _data;
        public AutocompleteItem Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Text = Data.DisplayText;
            }
        }

        public AutoCompleteDataItem Owner { get; private set; }

        public AutoCompleteDataLabel(AutoCompleteDataItem parent)
        {
            Owner = parent;
        }

        /// <summary>
        /// 绘制显示文本
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        public void OnPaint(Graphics g, Rectangle rect)
        {
            using (Brush strBrush = new SolidBrush(Owner.ForeColor))
            using (Pen borderPen = new Pen(Color.FromArgb(225, 195, 101)))
            using (Brush backBrush = new SolidBrush(Color.FromArgb(253, 244, 191)))
            using (StringFormat sf = new StringFormat())
            {
                g.FillRectangle(backBrush, Bound);
                g.DrawRectangle(borderPen, Bound);
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.LineAlignment = StringAlignment.Near;
                rect = Bound;
                rect.Height = Owner.Font.Height;
                rect.Y = Bound.Y + (Bound.Height - Owner.Font.Height) / 2;
                g.DrawString(Text, Owner.Font, strBrush, rect, sf);
            }
        }
    }
}
