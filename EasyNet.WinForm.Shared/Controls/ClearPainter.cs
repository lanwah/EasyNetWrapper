#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using EasyNet.Core;
using System.Text;
using EasyNet.Extensions;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.WinForm.Shared
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：ClearButton.cs
// 创建用户：lanwah
// 创建日期：2024/8/25 22:28:40
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.WinForm.Controls
{
    /// <summary>
    /// 绘制清除图标
    /// </summary>
    public class ClearPainter
    {
        private int _width = 22;
        /// <summary>
        /// 清除图标大小
        /// </summary>
        public int Width
        {
            get => this._width;
            set
            {
                if (value != this.Width)
                {
                    this._width = value;
                    this.UpdateBounds();
                }
            }
        }
        private Rectangle _bounds = new Rectangle(0, 0, 22, 22);
        /// <summary>
        /// 清除图标区域
        /// </summary>
        private Rectangle Bounds { get => this._bounds; set => this._bounds = value; }
        /// <summary>
        /// X坐标
        /// </summary>
        public int X { get => this.Bounds.X; set => this._bounds.X = value; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y { get => this.Bounds.Y; set => this._bounds.Y = value; }
        /// <summary>
        /// 坐标位置
        /// </summary>
        public Point Location { get => this.Bounds.Location; set => this._bounds.Location = value; }
        private bool _isHover = false;
        /// <summary>
        /// 鼠标是否在按钮区域
        /// </summary>
        public bool IsHover
        {
            get => this._isHover;
            set
            {
                if (value != this.IsHover)
                {
                    this._isHover = value;
                    this.OnNotifyInvalidate();
                }
            }
        }
        /// <summary>
        /// 拥有者
        /// </summary>
        protected System.Windows.Forms.Control Owner { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner"></param>
        public ClearPainter(System.Windows.Forms.Control owner)
        {
            this.Owner = owner;
        }

        /// <summary>
        /// 更新清除图标区域
        /// </summary>
        protected void UpdateBounds()
        {
            this.Bounds = new Rectangle(this.X, this.Y, this.Width, this.Width);
            this.OnNotifyInvalidate();
        }
        /// <summary>
        /// 更新鼠标是否在按钮区域
        /// </summary>
        /// <param name="mousePos"></param>
        public void UpdateHoverStaus(Point mousePos)
        {
            this.IsHover = this.Bounds.Contains(mousePos);
        }
        /// <summary>
        /// 通知重新绘制
        /// </summary>
        protected void OnNotifyInvalidate()
        {
            this.Owner?.Invalidate();
        }

        /// <summary>
        /// 绘制清除图标
        /// </summary>
        /// <param name="g"></param>
        public void Paint(Graphics g)
        {
            this.OnPaint(g);
        }
        /// <summary>
        /// 绘制清除图标
        /// </summary>
        /// <param name="g"></param>
        protected virtual void OnPaint(Graphics g)
        {
            var penColor = Color.FromArgb(109, 175, 206);
            var backColor = Color.FromArgb(109, 175, 206);
            var lineColor = Color.White;
            //if (this.IsHover)
            //{
            //    penColor = Color.FromArgb(145, 205, 230);
            //    backColor = Color.FromArgb(145, 205, 230);
            //}
            if (this.IsHover)
            {
                penColor = Color.FromArgb(49, 156, 212);
                backColor = Color.FromArgb(49, 156, 212);
            }
            using (var antiG = new AntiAliasGraphics(g, SmoothingMode.HighQuality))
            using (Pen pen = new Pen(penColor))
            using (Pen linePen = new Pen(lineColor, 2.0f))
            using (Brush backBrush = new SolidBrush(backColor))
            {
                var clientRect = this.Bounds;
                clientRect.Inflate(-2, -2);
                g.DrawEllipse(pen, clientRect);
                g.FillEllipse(backBrush, clientRect);

                var pt1 = new Point(clientRect.X + clientRect.Width / 4, clientRect.Y + clientRect.Height / 4);
                var pt2 = new Point(clientRect.X + clientRect.Width / 4 * 3, clientRect.Y + clientRect.Height / 4);
                var pt3 = new Point(clientRect.X + clientRect.Width / 4, clientRect.Y + clientRect.Height / 4 * 3);
                var pt4 = new Point(clientRect.X + clientRect.Width / 4 * 3, clientRect.Y + clientRect.Height / 4 * 3);
                pt1.Offset(1, 1);
                pt2.Offset(1, 1);
                pt3.Offset(1, 1);
                pt4.Offset(1, 1);
                g.DrawLine(linePen, pt1, pt4);
                g.DrawLine(linePen, pt2, pt3);
            }
        }
    }
}
#endif
