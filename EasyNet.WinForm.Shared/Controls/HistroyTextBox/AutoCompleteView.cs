#if NETFRAMEWORK
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
// 文件名称：AutoCompleteView.cs
// 创建用户：lanwah
// 创建日期：2024/8/23 10:39:16
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
    [ToolboxItem(false)]
    internal partial class AutoCompleteView : UserControl
    {
        internal AutocompleteHost Host { get; set; }
        /// <summary>
        /// 是否支持自定义输入
        /// </summary>
        public bool IsCustomInput { get; set; } = false;
        /// <summary>
        /// 有匹配项是否自动选中
        /// </summary>
        public bool IsFoundSelected
        {
            get; set;
        }

        private IEnumerable<AutocompleteItem> SourceItems
        {
            get
            {
                if (Host == null)
                {
                    return new List<AutocompleteItem>();
                }
                else
                {
                    return Host.SourceItems;
                }
            }
        }


        public AutoCompleteView()
        {
            InitializeComponent();

            this.tbxInput.MouseWheel += new MouseEventHandler(TbxInput_MouseWheel);
            this.Height = pnlTop.Height;

            autoCompleteList.ItemSelected += new EventHandler(AutoCompleteList_ItemSelected);
        }

        private void AutoCompleteList_ItemSelected(object sender, EventArgs e)
        {
            OnSelecting();
        }

        private void TbxInput_TextChanged(object sender, EventArgs e)
        {
            bool foundSelected = false;
            int selectedIndex = -1;

            string text = tbxInput.Text;
            var visibleItems = new List<AutocompleteItem>();

            if (Host.SearchCallback != null)
            {
                autoCompleteList.VisibleItems = Host.SearchCallback(text).ToList();
            }
            else if (SourceItems != null)
            {
                foreach (AutocompleteItem item in SourceItems)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    if (item.Key == null)
                    {
                        continue;
                    }

                    CompareResult res = item.Compare(text);
                    if (res != CompareResult.Hidden)
                    {
                        visibleItems.Add(item);
                    }

                    if (res == CompareResult.VisibleAndSelected && !foundSelected)
                    {
                        foundSelected = true;
                        if (this.IsFoundSelected)
                        {
                            // 找到了则设置选中的index为 0 
                            selectedIndex = visibleItems.Count - 1;
                        }
                    }
                }

                autoCompleteList.VisibleItems = visibleItems;
            }

            if ((null != autoCompleteList.VisibleItems) && (autoCompleteList.VisibleItems.Count > 0))
            {
                if (foundSelected)
                {
                    SelectedItemIndex = selectedIndex;
                }
                else
                {
                    SelectedItemIndex = 0;
                }
            }

            CalcSize();
        }

        public int SelectedItemIndex
        {
            get { return autoCompleteList.SelectedItemIndex; }
            internal set { autoCompleteList.SelectedItemIndex = value; }
        }

        public IList<AutocompleteItem> VisibleItems
        {
            get { return autoCompleteList.VisibleItems; }
            private set { autoCompleteList.VisibleItems = value; }
        }

        public void SelectNext(int shift)
        {
            SelectedItemIndex = Math.Max(0, Math.Min(SelectedItemIndex + shift, VisibleItems.Count - 1));
            //
            autoCompleteList.Invalidate();
        }

        public bool ProcessKey(Keys c, Keys keyModifiers)
        {
            var page = autoCompleteList.Height / (Font.Height + 4);
            if (keyModifiers == Keys.None)
            {
                switch (c)
                {
                    case Keys.Down:
                        SelectNext(+1);
                        return true;
                    case Keys.PageDown:
                        SelectNext(+page);
                        return true;
                    case Keys.Up:
                        SelectNext(-1);
                        return true;
                    case Keys.PageUp:
                        SelectNext(-page);
                        return true;
                    case Keys.Enter:
                    case Keys.Tab:
                    case Keys.Space:
                        OnSelecting();
                        return true;
                    case Keys.Escape:
                        Close();
                        return true;
                }
            }

            return false;
        }

        public void Close()
        {
            Host.Close();
        }

        internal virtual void OnSelecting()
        {
            if (SelectedItemIndex < 0 || SelectedItemIndex >= VisibleItems.Count)
            {
                Close();
                var text = this.tbxInput.Text;
                if (this.IsCustomInput && !text.IsNullOrEmptyEx())
                {
                    Host.OnSelecting(new SelectingEventArgs()
                    {
                        SelectedIndex = -1,
                        Item = new AutocompleteItem()
                        {
                            DisplayText = text,
                            Key = text,
                            ItemText = text,
                            Tag = text,
                        }
                    });
                }
                return;
            }

            AutocompleteItem item = VisibleItems[SelectedItemIndex];
            var args = new SelectingEventArgs
            {
                Item = item,
                SelectedIndex = SelectedItemIndex
            };

            Host.OnSelecting(args);
            autoCompleteList.CloseToolTip();
            if (args.Cancel)
            {
                SelectedItemIndex = args.SelectedIndex;
                (Host.ListView as Control).Invalidate(true);
                return;
            }

            Close();
            //
            var args2 = new SelectedEventArgs
            {
                Item = item,
            };
            //item.OnSelected(args2);
            Host.OnSelected(args2);
        }

        internal void CalcSize()
        {
            var viewHeight = ((autoCompleteList.Height >= autoCompleteList.ItemHeight) ? autoCompleteList.Height : 0);
            this.Height = (pnlTop.Height + viewHeight + (pnlBottom.Visible ? pnlBottom.Height : 0));
            autoCompleteList.MaximumSize = new Size(pnlTop.Width - 0, autoCompleteList.MaximumSize.Height);
            autoCompleteList.Width = pnlTop.Width - 0;
            Host.CalcSize();

            //System.Diagnostics.Trace.WriteLine($"Height = {this.Height}");
            //System.Diagnostics.Trace.WriteLine($"pnlTop.Height = {pnlTop.Height}");
            //System.Diagnostics.Trace.WriteLine($"autoCompleteList.Height = {autoCompleteList.Height}{Environment.NewLine}");
            //System.Diagnostics.Trace.WriteLine($"autoCompleteList.MaximumSize = {autoCompleteList.MaximumSize.ToString()}");
            //System.Diagnostics.Trace.WriteLine($"{Environment.NewLine}");
        }

        private bool TbxInput_DoProcessDialogKey(Keys keyData)
        {
            return ProcessKey(keyData, Keys.None);
        }

        private void TbxInput_MouseWheel(object sender, MouseEventArgs e)
        {
            autoCompleteList.SetMouseWheel(e);
        }

        public void Init()
        {
            tbxInput.TextChanged -= new EventHandler(TbxInput_TextChanged);
            tbxInput.Text = string.Empty;
            autoCompleteList.Clear();
            CalcSize();
            tbxInput.TextChanged += new EventHandler(TbxInput_TextChanged);
        }
    }

    [ToolboxItem(false)]
    internal class AutoCompleteTextBox : TextBox
    {
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return ExecuteDialogKey(keyData) || base.ProcessDialogKey(keyData);
        }

        public bool ExecuteDialogKey(Keys keyData)
        {
            if (DoProcessDialogKey != null && DoProcessDialogKey(keyData))
            {
                return true;
            }

            return false;
        }

        public event DialogKeyProcessor DoProcessDialogKey;
    }

    [ToolboxItem(false)]
    internal class AutoCompleteList : ScrollableControl
    {
        private readonly ToolTip toolTip = new ToolTip();
        private int HoveredItemIndex { get; set; } = -1;
        private int oldItemCount;
        private int selectedItemIndex = -1;
        private IList<AutocompleteItem> visibleItems;
        /// <summary>
        /// 项目间距
        /// </summary>
        private Padding ItemPadding { get; set; } = new Padding(0, 5, 0, 5);

        public event EventHandler ItemSelected;

        public event EventHandler ViewDoubleClick;

        public event EventHandler ViewClick;

        public AutoCompleteList()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, false);
            base.Font = new Font(FontFamily.GenericSansSerif, 9);
            this.SetItemHeight(Font.Height);
            VerticalScroll.SmallChange = ItemHeight;
            BackColor = Color.White;
        }

        private int itemHeight = 25;

        public int ItemHeight
        {
            get { return itemHeight; }
            private set
            {
                itemHeight = value;
                VerticalScroll.SmallChange = value;
                oldItemCount = -1;
                AdjustScroll();
            }
        }
        /// <summary>
        /// 设置项目条目高度
        /// </summary>
        /// <param name="value"></param>
        public void SetItemHeight(int value)
        {
            this.ItemHeight = value + this.ItemPadding.Vertical;
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                this.SetItemHeight(Font.Height);
            }
        }
        /// <summary>
        /// 项图标列表
        /// </summary>
        public ImageList ImageList { get; set; }

        public IList<AutocompleteItem> VisibleItems
        {
            get { return visibleItems; }
            set
            {
                visibleItems = value;
                SelectedItemIndex = -1;
                AdjustScroll();
                Invalidate();
            }
        }

        public int SelectedItemIndex
        {
            get { return selectedItemIndex; }
            set
            {
                selectedItemIndex = value;
                if (SelectedItemIndex >= 0 && SelectedItemIndex < VisibleItems.Count)
                {
                    SetToolTip(VisibleItems[SelectedItemIndex]);
                    ScrollToSelected();
                }

                Invalidate();
            }
        }

        private void AdjustScroll()
        {
            if (VisibleItems == null)
            {
                return;
            }

            if (oldItemCount == VisibleItems.Count)
            {
                return;
            }

            int needHeight = ItemHeight * VisibleItems.Count + 1;
            Height = Math.Min(needHeight, MaximumSize.Height);
            AutoScrollMinSize = new Size(0, needHeight - 2);
            oldItemCount = VisibleItems.Count;
        }

        private void ScrollToSelected()
        {
            int y = SelectedItemIndex * ItemHeight - VerticalScroll.Value;
            if (y < 0)
            {
                VerticalScroll.Value = SelectedItemIndex * ItemHeight;
            }

            if (y > ClientSize.Height - ItemHeight)
            {
                VerticalScroll.Value = Math.Min(VerticalScroll.Maximum,
                                                SelectedItemIndex * ItemHeight - ClientSize.Height + ItemHeight);
            }
            //some magic for update scrolls
            AutoScrollMinSize -= new Size(1, 0);
            AutoScrollMinSize += new Size(1, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (VisibleItems == null)
            {
                return;
            }

            bool rtl = RightToLeft == RightToLeft.Yes;
            AdjustScroll();
            int startI = VerticalScroll.Value / ItemHeight - 1;
            int finishI = (VerticalScroll.Value + ClientSize.Height) / ItemHeight + 1;
            startI = Math.Max(startI, 0);
            finishI = Math.Min(finishI, VisibleItems.Count);
            int y;
            int leftPadding = 1;
            for (int i = startI; i < finishI; i++)
            {
                y = i * ItemHeight - VerticalScroll.Value;

                if (ImageList != null && VisibleItems[i].ImageIndex >= 0)
                {
                    if (rtl)
                    {
                        e.Graphics.DrawImage(ImageList.Images[VisibleItems[i].ImageIndex], Width - 1 - leftPadding, y);
                    }
                    else
                    {
                        e.Graphics.DrawImage(ImageList.Images[VisibleItems[i].ImageIndex], 1, y);
                    }
                }

                var textRect = new Rectangle(leftPadding, y, ClientSize.Width - 1 - leftPadding, ItemHeight - 1);
                if (rtl)
                {
                    textRect = new Rectangle(1, y, ClientSize.Width - 1 - leftPadding, ItemHeight - 1);
                }

                if (i == SelectedItemIndex)
                {
                    Brush selectedBrush = new LinearGradientBrush(new Point(0, y - 3), new Point(0, y + ItemHeight),
                                                                  Color.White, Color.Orange);
                    e.Graphics.FillRectangle(selectedBrush, textRect);
                    e.Graphics.DrawRectangle(Pens.Orange, textRect);
                }
                if (i == HoveredItemIndex)
                {
                    e.Graphics.DrawRectangle(Pens.Red, textRect);
                }

                var sf = new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    //Alignment = StringAlignment.Near
                };
                if (rtl)
                {
                    sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                }

                var args = new PaintItemEventArgs(e.Graphics, e.ClipRectangle)
                {
                    Font = Font,
                    TextRect = new RectangleF(textRect.Location, textRect.Size),
                    StringFormat = sf,
                    IsSelected = i == SelectedItemIndex,
                    IsHovered = i == HoveredItemIndex
                };
                //call drawing
                VisibleItems[i].OnPaint(args);
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate(true);
            OnViewClick();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                SelectedItemIndex = PointToItemIndex(e.Location);
                ScrollToSelected();
                Invalidate();
            }
            OnViewClick();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            SelectedItemIndex = PointToItemIndex(e.Location);
            Invalidate();
            OnItemSelected();
            this.OnViewDoubleClick();
        }

        private void OnItemSelected()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);
        }

        private void OnViewClick()
        {
            ViewClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnViewDoubleClick()
        {
            ViewDoubleClick?.Invoke(this, EventArgs.Empty);
        }

        private const int WM_MOUSEACTIVATE = 0x21;
        private const int MA_NOACTIVATE = 3;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_MOUSEACTIVATE)
            {
                m.Result = new IntPtr(MA_NOACTIVATE);
                return;
            }

            base.WndProc(ref m);
        }

        private int PointToItemIndex(Point p)
        {
            return (p.Y + VerticalScroll.Value) / ItemHeight;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //var host = Parent as AutocompleteMenuHost;
            //if (host != null)
            //    if (host.Owner.ProcessKey((char)keyData, Keys.None))
            //        return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SelectItem(int itemIndex)
        {
            SelectedItemIndex = itemIndex;
            ScrollToSelected();
            Invalidate();
        }

        public void SetItems(List<AutocompleteItem> items)
        {
            VisibleItems = items;
            SelectedItemIndex = -1;
            AdjustScroll();
            Invalidate();
        }

        public void SetToolTip(AutocompleteItem autocompleteItem)
        {
            string title = autocompleteItem.ToolTipTitle;
            string text = autocompleteItem.ToolTipText;

            if (string.IsNullOrEmpty(title))
            {
                toolTip.ToolTipTitle = null;
                toolTip.SetToolTip(this, null);
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                toolTip.ToolTipTitle = null;
                toolTip.Show(title, this, Width + 3, 0, 3000);
            }
            else
            {
                toolTip.ToolTipTitle = title;
                toolTip.Show(text, this, Width + 3, 0, 3000);
            }
        }

        public void SetMouseWheel(MouseEventArgs e)
        {
            OnMouseWheel(e);
        }

        public void Clear()
        {
            if (visibleItems != null)
            {
                this.visibleItems.Clear();
            }

            oldItemCount = 0;
            this.Height = 0;
        }

        public void CloseToolTip()
        {
            this.toolTip.Hide(this);
        }
    }
}
#endif
