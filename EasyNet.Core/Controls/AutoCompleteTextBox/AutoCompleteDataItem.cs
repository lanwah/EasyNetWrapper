using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutoCompleteDataItem
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:30:58
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
    [ToolboxItem(false)]
    internal class AutoCompleteDataItem : Panel
    {
        private readonly ToolTip toolTip = new ToolTip();
        /// <summary>
        /// 清除标签显示内容事件
        /// </summary>
        public event EventHandler DataDeleted;

        public enum MouseState
        {
            Normal = 0,
            Hover,
            Pressed,
        }

        protected MouseState closeMouseState;
        protected MouseState CloseState
        {
            get { return closeMouseState; }
            set
            {
                if (value != closeMouseState)
                {
                    closeMouseState = value;
                    Invalidate(CloseRect);
                }
            }
        }
        /// <summary>
        /// 关闭按钮正方形边框
        /// </summary>
        private readonly int CloseWidth = 22;

        private AutoCompleteDataLabel DataLabel { get; set; }

        public Rectangle CloseRect
        {
            get
            {
                Rectangle rect = new Rectangle(this.Bounds.Right - CloseWidth - 4, (this.Height - CloseWidth) / 2, CloseWidth, CloseWidth);
                return rect;
            }
        }

        public Rectangle DataRect
        {
            get
            {
                Rectangle rect = new Rectangle(this.Bounds.Left, this.Bounds.Top, this.Width - CloseWidth - 2, this.Height);
                return rect;
            }
        }

        public void SetDataItem(AutocompleteItem oItem)
        {
            if (oItem == null)
            {
                DataLabel = null;
            }
            else
            {
                DataLabel = new AutoCompleteDataLabel(this) { Data = oItem };
            }
            SetToolTip(oItem);
            this.Invalidate();
        }

        public AutocompleteItem DataItem
        {
            get
            {
                if (DataLabel == null)
                {
                    return null;
                }

                return DataLabel.Data;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Rectangle borderRect = this.Bounds;
            g.DrawRectangle(Pens.Black, borderRect);
            g.FillRectangle(Brushes.White, borderRect);
            ControlPaint.DrawBorder3D(g, borderRect);
            if (DataLabel != null)
            {
                // 绘制显示的文本
                this.DataLabel.OnPaint(g, e.ClipRectangle);
                this.PaintCloseButton(g);
            }
        }
        /// <summary>
        /// 绘制关闭图标
        /// </summary>
        /// <param name="g"></param>
        private void PaintCloseButton(Graphics g)
        {
            Color penColor = Color.FromArgb(109, 175, 206);
            Color backColor = Color.FromArgb(109, 175, 206);
            Color lineColor = Color.White;
            if (CloseState == MouseState.Hover)
            {
                penColor = Color.FromArgb(145, 205, 230);
                backColor = Color.FromArgb(145, 205, 230);
            }
            else if (CloseState == MouseState.Pressed)
            {
                penColor = Color.FromArgb(49, 156, 212);
                backColor = Color.FromArgb(49, 156, 212);
            }
            using (AntiAliasGraphics antiG = new AntiAliasGraphics(g, SmoothingMode.HighQuality))
            using (Pen pen = new Pen(penColor))
            using (Pen linePen = new Pen(lineColor, 2.0f))
            using (Brush backBrush = new SolidBrush(backColor))
            {
                Rectangle closeRect = CloseRect;
                closeRect.Inflate(-2, -2);
                g.DrawEllipse(pen, closeRect);
                g.FillEllipse(backBrush, closeRect);

                Point pt1 = new Point(closeRect.X + closeRect.Width / 4, closeRect.Y + closeRect.Height / 4);
                Point pt2 = new Point(closeRect.X + closeRect.Width / 4 * 3, closeRect.Y + closeRect.Height / 4);
                Point pt3 = new Point(closeRect.X + closeRect.Width / 4, closeRect.Y + closeRect.Height / 4 * 3);
                Point pt4 = new Point(closeRect.X + closeRect.Width / 4 * 3, closeRect.Y + closeRect.Height / 4 * 3);
                pt1.Offset(1, 1);
                pt2.Offset(1, 1);
                pt3.Offset(1, 1);
                pt4.Offset(1, 1);
                g.DrawLine(linePen, pt1, pt4);
                g.DrawLine(linePen, pt2, pt3);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (DataLabel != null)
            {
                if (CloseRect.Contains(e.Location))
                {
                    // 进入关闭按钮区域
                    if (CloseState != MouseState.Pressed)
                    {
                        CloseState = MouseState.Hover;
                    }

                    toolTip.Hide(this);
                }
                else
                {
                    CloseState = MouseState.Normal;
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DataLabel != null)
            {
                CloseState = MouseState.Normal;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (CloseRect.Contains(e.Location))
            {
                if (DataLabel != null)
                {
                    CloseState = MouseState.Pressed;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (DataLabel != null)
            {
                if (CloseRect.Contains(e.Location))
                {
                    CloseState = MouseState.Hover;
                }
                else
                {
                    CloseState = MouseState.Normal;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (DataLabel != null)
            {
                if (CloseRect.Contains(e.Location))
                {
                    DataLabel = null;
                    Invalidate();
                    if (DataDeleted != null)
                    {
                        DataDeleted(this, new EventArgs());
                    }
                }
            }
        }

        public void SetToolTip(AutocompleteItem autocompleteItem)
        {
            if (autocompleteItem == null)
            {
                toolTip.SetToolTip(this, null);
                return;
            }

            string title = autocompleteItem.ToolTipTitle;
            string text = autocompleteItem.ToolTipText;

            //if (string.IsNullOrEmpty(title))
            {
                toolTip.ToolTipTitle = title;
                toolTip.SetToolTip(this, text);
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                toolTip.ToolTipTitle = null;
                toolTip.Show(title, this, 3000);
            }
            else
            {
                toolTip.ToolTipTitle = title;
                toolTip.Show(text, this, 3000);
            }
        }
    }
}
