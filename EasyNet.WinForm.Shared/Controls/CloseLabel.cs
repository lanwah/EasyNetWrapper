#if NETFRAMEWORK
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using EasyNet.Core;
using System.Drawing.Drawing2D;
using EasyNet.Extensions;

namespace EasyNet.WinForm.Controls
{
    /// <summary>
    /// 可关闭标签
    /// </summary>
    public class CloseLabel : Control
    {
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color? BorderColor
        {
            get; set;
        }
        /// <summary>
        /// 清除按钮绘制器
        /// </summary>
        protected ClearPaintItem ClearButtonPainter
        {
            get; set;
        }
        /// <summary>
        /// 清除按钮宽度
        /// </summary>
        private int ClearButtonWidth => this.ClearButtonPainter.Width;
        /// <summary>
        /// 清除按钮坐标
        /// </summary>
        private Point ClearButtonLocation
        {
            get
            {
                var clearWidth = this.ClearButtonWidth;
                return new Point(this.Width - clearWidth - 4, (this.Height - clearWidth) / 2 - 1);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CloseLabel() : base()
        {
            this.ClearButtonPainter = new ClearPaintItem(this);
            this.Size = new Size(100, 25);
            this.BackColor = System.Drawing.Color.LightGray;
            this.BorderColor = Color.DarkGray;
            this.AutoSize = true;

            // 双缓冲
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.ResizeRedraw, true);
            //this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
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
                if (this.Text != value)
                {
                    base.Text = value;
                    this.AdjustSize();
                }
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
                if (this.AutoSize != value)
                {
                    base.AutoSize = value;
                    this.AdjustSize();
                }
            }
        }

        /// <summary>
        /// 绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            var rect = e.ClipRectangle;

            this.PaintBackColor(g, rect);
            // 绘制边框
            this.PaintBorder(g, rect);
            this.PaintText(g, rect);
            this.PaintClearButton(g);
        }

        private void PaintClearButton(Graphics g)
        {
            // 绘制清除按钮
            var painter = this.ClearButtonPainter;
            // 绘制
            using (new SaveStateGraphics(g))
            {
                // 更改坐标原点
                var location = this.ClearButtonLocation;
                g.TranslateTransform(location.X, location.Y);
                painter.Paint(new PaintEventArgs(g, Rectangle.Empty));
            }
        }

        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        protected virtual void PaintBorder(Graphics g, Rectangle rect)
        {
            if (this.BorderColor.HasValue)
            {
                using (Pen pen = new Pen(this.BorderColor.Value))
                {
                    g.DrawRectangle(pen, new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1));
                }
            }
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
            var clearWidth = this.ClearButtonWidth;
            // 宽度
            var width = (int)Math.Ceiling(size.Width) + 10 + clearWidth;
            // 高度
            var height = Math.Max((int)Math.Ceiling(size.Height) + 10, clearWidth + 2);
            if ((this.Width != width) || (this.Height != height))
            {
                this.Width = width;
                this.Height = height;
                // 立马绘制
                this.Invalidate();
            }
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            this.ClearButtonPainter.UpdateHoverStatus(e.Location, this.ClearButtonLocation);
        }
        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.ClearButtonPainter.IsHover = false;
        }
        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (this.IsDesignMode())
            {
                return;
            }

            if (this.ClearButtonPainter.IsHover)
            {
                // 移除控件
                if (this.Parent.IsNotNull())
                {
                    this.Parent.Controls.Remove(this);
                }
            }
        }
    }
}
#endif
