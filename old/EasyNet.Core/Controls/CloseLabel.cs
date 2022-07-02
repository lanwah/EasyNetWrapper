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
// 项目名称：EasyNet.Core.Controls
// 文件名称：CloseLabel
// 创 建 者：lanwah
// 创建日期：2021/02/25 14:48:55
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
    /// 可关闭标签
    /// </summary>
    public class CloseLabel : Panel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CloseLabel() : base()
        {
            this.Size = new Size(100, 25);
            this.BackColor = System.Drawing.Color.LightGray;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoSize = true;
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        [Browsable(true)]
        public new string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                this.AdjustSize();
            }
        }
        /// <summary>
        /// 是否自动调整大小
        /// </summary>
        public override bool AutoSize
        {
            get => base.AutoSize;
            set
            {
                base.AutoSize = value;
                this.AdjustSize();
            }
        }
        private bool _isInBtnArea = false;
        /// <summary>
        /// 是否在按钮区域
        /// </summary>
        private bool IsInBtnArea
        {
            get => this._isInBtnArea;
            set
            {
                if (this._isInBtnArea != value)
                {
                    this._isInBtnArea = value;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// 关闭按钮正方形边框
        /// </summary>
        public int CloseWidth
        {
            get; set;
        } = 22;
        /// <summary>
        /// 关闭按钮区域
        /// </summary>
        public Rectangle CloseRect
        {
            get
            {
                Rectangle rect = new Rectangle(this.Width - CloseWidth - 4, (this.Height - CloseWidth) / 2 - 1, CloseWidth, CloseWidth);
                return rect;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;

            this.PaintBackColor(g, rect);
            this.PaintText(g, rect);
            this.PaintCloseButton(g);
        }

        /// <summary>
        /// 绘制背景，不控制边框，边框通过BorderStyle控制
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void PaintBackColor(Graphics g, Rectangle rect)
        {
            g.DrawRectangle(Pens.Gray, rect);
            if (this.BackColor != Color.Transparent)
            {
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    g.FillRectangle(brush, rect);
                }
            }
            //ControlPaint.DrawBorder3D(g, rect);
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        private void PaintText(Graphics g, Rectangle rect)
        {
            StringFormat sf = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
            using (SolidBrush brush = new SolidBrush(this.ForeColor))
            {
                g.DrawString(this.Text, this.Font, brush, rect, sf);
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
            //if (this.IsInBtnArea)
            //{
            //    penColor = Color.FromArgb(145, 205, 230);
            //    backColor = Color.FromArgb(145, 205, 230);
            //}
            if (this.IsInBtnArea)
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
        /// <summary>
        /// 调整大小
        /// </summary>
        private void AdjustSize()
        {
            if (!this.AutoSize)
            {
                return;
            }

            var g = this.CreateGraphics();
            var size = g.MeasureString(this.Text, this.Font);
            // 宽度
            var width = (int)Math.Ceiling(size.Width) + 10 + this.CloseWidth;
            // 高度
            var height = Math.Max((int)Math.Ceiling(size.Height) + 10, this.CloseWidth + 2);
            if ((this.Width != width) || (this.Height != height))
            {
                this.Width = width;
                this.Height = height;
                // 立马绘制
                this.Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (CloseRect.Contains(e.Location))
            {
                this.IsInBtnArea = true;
            }
            else
            {
                this.IsInBtnArea = false;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.IsInBtnArea = false;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (this.IsInBtnArea)
            {
                // 移除控件
                if (null != this.Parent)
                {
                    this.Parent.Controls.Remove(this);
                }

            }
        }
    }
}
