#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using EasyNet.Core;
using System.Text;
using EasyNet.Extensions;
using System.Windows.Forms;

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
    /// 清除按钮样式项
    /// </summary>
    public class ClearPaintItem : PaintItemBase
    {
        /// <summary>
        /// 正常情况下的颜色
        /// </summary>
        public Color NormalColor { get; set; } = Color.FromArgb(109, 175, 206);
        /// <summary>
        /// 鼠标悬停情况下的颜色
        /// </summary>
        public Color HoverColor { get; set; } = Color.FromArgb(49, 156, 212);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner"></param>
        public ClearPaintItem(System.Windows.Forms.Control owner) : base(owner)
        {
            this.Size = new Size(22, 22);
        }
        /// <summary>
        /// 绘制项
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var penColor = this.NormalColor;
            var backgroundColor = this.NormalColor;
            var lineColor = Color.White;
            //if (this.IsHover)
            //{
            //    penColor = Color.FromArgb(145, 205, 230);
            //    backColor = Color.FromArgb(145, 205, 230);
            //}
            if (this.IsHover)
            {
                penColor = this.HoverColor;
                backgroundColor = this.HoverColor;
            }

            using (new AntiAliasGraphics(g, SmoothingMode.HighQuality))
            using (var pen = new Pen(penColor))
            using (var linePen = new Pen(lineColor, 2.0f))
            using (var backBrush = new SolidBrush(backgroundColor))
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
