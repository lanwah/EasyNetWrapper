#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using EasyNet.Core;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.WinForm.Shared.Gdi_.PaintItem
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：PaintItemBase.cs
// 创建用户：lanwah
// 创建日期：2024/8/26 17:25:23
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
    /// 绘制项基类
    /// </summary>
    public abstract class PaintItemBase : IDisposable
    {
        private Size _size = new Size(24, 24);
        /// <summary>
        /// 绘制项大小
        /// </summary>
        public Size Size
        {
            get => this._size;
            set
            {
                if (this.Size != value)
                {
                    this._size = value;
                    this.UpdateSize();
                }
            }
        }
        /// <summary>
        /// 绘制的宽度
        /// </summary>
        public int Width
        {
            get => this._size.Width;
            set
            {
                if (this.Width != value)
                {
                    this._size.Width = value;
                    this.UpdateSize();
                }
            }
        }
        /// <summary>
        /// 绘制的高度
        /// </summary>
        public int Height
        {
            get => this._size.Height;
            set
            {
                if (this.Height != value)
                {
                    this._size.Height = value;
                    this.UpdateSize();
                }
            }
        }
        private Rectangle _bounds = new Rectangle(0, 0, 24, 24);
        /// <summary>
        /// 绘制项区域与位置无关，只与绘制项大小有关
        /// </summary>
        public Rectangle Bounds { get => this._bounds; set => this._bounds = value; }
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
        /// 绘制项拥有者
        /// </summary>
        protected System.Windows.Forms.Control Owner { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="owner"></param>
        public PaintItemBase(System.Windows.Forms.Control owner)
        {
            this.Owner = owner;
        }

        /// <summary>
        /// 更新绘制项大小
        /// </summary>
        protected void UpdateSize()
        {
            this._bounds = new Rectangle(0, 0, this.Size.Width, this.Size.Height);
            this.OnNotifyInvalidate();
        }
        /// <summary>
        /// 通知重新绘制
        /// </summary>
        protected void OnNotifyInvalidate()
        {
            this.Owner?.Invalidate();
        }
        /// <summary>
        /// 测试点是否在绘制项区域内，请注意移除此项的位置信息
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool HitHoverTest(Point point)
        {
            return this.Bounds.Contains(point);
        }
        /// <summary>
        /// 更新项的鼠标状态
        /// </summary>
        /// <param name="currentPos">当前鼠标位置</param>
        /// <param name="location">项的位置</param>
        public void UpdateHoverStatus(Point currentPos, Point location)
        {
            var relativPos = currentPos;
            relativPos.Offset(-location.X, -location.Y);
            if (this.HitHoverTest(relativPos))
            {
                this.IsHover = true;
            }
            else
            {
                this.IsHover = false;
            }
        }

        /// <summary>
        /// 绘制项
        /// </summary>
        /// <param name="e"></param>
        public void Paint(PaintEventArgs e)
        {
            this.OnPaint(e);
        }
        /// <summary>
        /// 绘制项
        /// </summary>
        /// <param name="e"></param>
        protected abstract void OnPaint(PaintEventArgs e);

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            this.Owner = null;
        }
    }
}

#endif
