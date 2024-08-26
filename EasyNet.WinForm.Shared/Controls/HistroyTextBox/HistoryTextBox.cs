#if NETFRAMEWORK
using EasyNet.Core;
using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.WinForm.Shared
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：HistroyTextBox.cs
// 创建用户：lanwah
// 创建日期：2024/8/23 10:16:39
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
    /// 检索下拉文本框，支持两种模式：
    /// 1、支持从候选项中选择内容，不允许输入，输入只是检索的触发动作；
    /// 2、支持自定义输入和带历史记录；
    /// </summary>
    public partial class HistoryTextBox : UserControl
    {
        private AutocompleteHost Host { get; set; }

        /// <summary>
        /// 是否支持自定义输入
        /// </summary>
        public bool IsCustomInput
        {
            get; set;
        } = false;
        /// <summary>
        /// 选择项发生改变
        /// </summary>
        public event EventHandler DataChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HistoryTextBox()
        {
            InitializeComponent();

            autoCompleteDataItem.Click += new EventHandler(AutoCompleteDataItem_Click);
            autoCompleteDataItem.DataDeleted += new EventHandler(AutoCompleteDataItem_DataDeleted);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;
        }


        private void InitHost()
        {
            if (this.Host.IsNull())
            {
                Host = new AutocompleteHost(this)
                {
                    SourceItems = this.SourceItems,
                    SearchCallback = SearchCallback
                };
                Host.Selecting += new EventHandler<SelectingEventArgs>(Host_Selecting);
                Host.SetCustomInput(this.IsCustomInput);
            }
        }

        private void AutoCompleteDataItem_DataDeleted(object sender, EventArgs e)
        {
            DataChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 源数据项
        /// </summary>
        public IEnumerable<AutocompleteItem> SourceItems
        {
            get; set;
        }
        /// <summary>
        /// 检索回调函数
        /// </summary>
        public Func<string, AutocompleteItem[]> SearchCallback
        {
            get; set;
        }
        /// <summary>
        /// 设置检索参数
        /// </summary>
        /// <typeparam name="T">源数据类型</typeparam>
        /// <param name="callBack">检索回调函数</param>
        /// <param name="converter">转换器，负责把源数据类型转换成AutocompleteItem类型</param>
        public void SetSearchCallbackArgs<T>(Func<string, T[]> callBack, Converter<T, AutocompleteItem> converter)
        {
            this.SearchCallback = (input) =>
            {
                var records = callBack.Invoke(input);
                IEnumerable<AutocompleteItem> targets = records.Select(c =>
                {
                    return converter(c);
                });
                return targets.ToArray();

            };
        }
        /// <summary>
        /// 设置检索回调参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        public void SetSearchCallbackArgs<T>(AutoCompleteSearchArgs<T> args)
        {
            if (null != args)
            {
                this.SearchCallback = args.Search;
            }
        }

        internal void Host_Selecting(object sender, SelectingEventArgs e)
        {
            autoCompleteDataItem.SetDataItem(e.Item);
            DataChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 获取或设置选中的项
        /// </summary>
        public AutocompleteItem SelectedItem
        {
            get
            {
                return autoCompleteDataItem.DataItem;
            }
            set
            {
                autoCompleteDataItem.SetDataItem(value);

                DataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AutoCompleteDataItem_Click(object sender, EventArgs e)
        {
            this.InitHost();

            if (autoCompleteDataItem.DataItem == null)
            {
                Host.Show(PointToScreen(autoCompleteDataItem.Location));
            }
        }

        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }
        /// <summary>
        /// 
        /// </summary>
        public void FocusEx()
        {
            if (!Host.Visible)
            {
                AutoCompleteDataItem_Click(null, null);
            }
        }
    }


    [ToolboxItem(false)]
    internal class AutocompleteHost : ToolStripDropDown
    {
        /// <summary>
        /// User selects item
        /// </summary>
        public event EventHandler<SelectingEventArgs> Selecting;

        /// <summary>
        /// It fires after item inserting
        /// </summary>
        public event EventHandler<SelectedEventArgs> Selected;

        private AutoCompleteView listView;
        public ToolStripControlHost Host { get; set; }
        public AutoCompleteView ListView
        {
            get { return listView; }
            set
            {
                if (value == null)
                {
                    listView = new AutoCompleteView();
                }
                else
                {
                    if (!(value is Control))
                    {
                        throw new Exception("ListView must be derived from Control class");
                    }

                    listView = value;
                }
                this.Host = new ToolStripControlHost(ListView as Control);
                Host.Margin = new Padding(2, 2, 2, 2);
                Host.Padding = Padding.Empty;
                Host.AutoSize = false;
                Host.AutoToolTip = false;
                listView.Host = this;
                CalcSize();
                base.Items.Clear();
                base.Items.Add(Host);
            }
        }
        public readonly HistoryTextBox Owner;
        public AutocompleteHost(HistoryTextBox owner)
        {
            AutoClose = true;
            AutoSize = false;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
            this.DropShadowEnabled = false;

            Owner = owner;
            ListView = new AutoCompleteView();
        }

        public IEnumerable<AutocompleteItem> SourceItems { get; set; }

        public Func<string, AutocompleteItem[]> SearchCallback
        {
            get; set;
        }

        internal void CalcSize()
        {
            var viewSize = (ListView as Control).Size;
            Host.Size = viewSize;
            Size = new System.Drawing.Size(viewSize.Width - 0, viewSize.Height + 4);
            //Size = new System.Drawing.Size((ListView as Control).Size.Width + 12, (ListView as Control).Size.Height + 16);
        }
        public override RightToLeft RightToLeft
        {
            get
            {
                return base.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
                (ListView as Control).RightToLeft = value;
            }
        }

        internal void OnSelecting(SelectingEventArgs args)
        {
            Selecting?.Invoke(this, args);
        }

        internal void OnSelected(SelectedEventArgs args)
        {
            Selected?.Invoke(this, args);
        }

        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            ListView.Init();
            listView.Width = Owner.Width;
            CalcSize();
            base.OnOpening(e);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            ListView.Focus();
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            base.OnClosed(e);
            Owner.Focus();
        }
        /// <summary>
        /// 是否支持自定义输入
        /// </summary>
        /// <param name="customInput"></param>
        public void SetCustomInput(bool customInput)
        {
            if (this.listView is null)
            {
                return;
            }

            this.listView.IsCustomInput = customInput;
        }
    }


    [ToolboxItem(false)]
    internal class AutoCompleteDataItem : Panel
    {
        private readonly ToolTip toolTip = new ToolTip();
        /// <summary>
        /// 清除标签显示内容事件
        /// </summary>
        public event EventHandler DataDeleted;

        private AutoCompleteDataLabel DataLabel { get; set; }
        public Rectangle DataRect
        {
            get
            {
                var rect = new Rectangle(this.Bounds.Left, this.Bounds.Top, this.Width - this.ClearButtonWidth - 2, this.Height);
                return rect;
            }
        }
        /// <summary>
        /// 清除按钮绘制器
        /// </summary>
        protected ClearPainter ClearButtonPainter
        {
            get; set;
        }
        /// <summary>
        /// 清除按钮宽度
        /// </summary>
        private int ClearButtonWidth => this.ClearButtonPainter.Width;

        public AutoCompleteDataItem()
        {
            this.ClearButtonPainter = new ClearPainter(this);
        }

        public void SetDataItem(AutocompleteItem item)
        {
            if (item == null)
            {
                DataLabel = null;
            }
            else
            {
                DataLabel = new AutoCompleteDataLabel(this) { Data = item };
            }
            SetToolTip(item);
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
                //this.DataLabel.OnPaint(g, e.ClipRectangle); 
                this.DataLabel.OnPaint(g);
                this.PaintClearButton(g);
            }
        }
        /// <summary>
        /// 绘制关闭图标
        /// </summary>
        /// <param name="g"></param>
        private void PaintClearButton(Graphics g)
        {
            // 绘制清除按钮
            var clearWidth = this.ClearButtonWidth;
            var painter = this.ClearButtonPainter;
            // 更新坐标
            painter.Location = new Point(this.Width - clearWidth - 4, (this.Height - clearWidth) / 2 - 1);
            // 绘制
            painter.Paint(g);
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
                this.ClearButtonPainter.UpdateHoverStaus(e.Location);

                if (this.ClearButtonPainter.IsHover)
                {
                    toolTip.Hide(this);
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DataLabel != null)
            {
                this.ClearButtonPainter.IsHover = false;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (DataLabel != null)
            {
                if (this.ClearButtonPainter.IsHover)
                {
                    DataLabel = null;
                    Invalidate();
                    DataDeleted?.Invoke(this, new EventArgs());
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

            //if (string.IsNullOrEmpty(text))
            //{
            //    toolTip.ToolTipTitle = null;
            //    toolTip.Show(title, this, 3000);
            //}
            //else
            //{
            //    toolTip.ToolTipTitle = title;
            //    toolTip.Show(text, this, 3000);
            //}
        }
    }

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
                var rect = Owner.DataRect;
                int height = Owner.Font.Height + 6;
                return new Rectangle(rect.X + 3, rect.Y + (rect.Height - height) / 2 - 1, rect.Width - 7, height);
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
        /// <summary>
        /// 是否绘制边框和背景
        /// </summary>
        public bool DrawBorder { get; set; } = false;

        public AutoCompleteDataItem Owner { get; private set; }

        public AutoCompleteDataLabel(AutoCompleteDataItem parent)
        {
            Owner = parent;
        }

        /// <summary>
        /// 绘制显示文本
        /// </summary>
        /// <param name="g"></param>
        public void OnPaint(Graphics g)
        {
            var drawBound = Bound;
            using (Brush strBrush = new SolidBrush(Owner.ForeColor))
            using (StringFormat sf = new StringFormat())
            {
                if (this.DrawBorder)
                {
                    using (Brush backBrush = new SolidBrush(Color.FromArgb(253, 244, 191)))
                    using (Pen borderPen = new Pen(Color.FromArgb(225, 195, 101)))
                    {
                        g.FillRectangle(backBrush, drawBound);
                        g.DrawRectangle(borderPen, drawBound);
                    }
                }

                sf.Alignment = StringAlignment.Near;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.LineAlignment = StringAlignment.Far;
                var rect = drawBound;
                rect.Height = Owner.Font.Height;
                rect.Y = drawBound.Y + (drawBound.Height - Owner.Font.Height) / 2;
                g.DrawString(Text, Owner.Font, strBrush, rect, sf);
            }
        }
    }
}
#endif
