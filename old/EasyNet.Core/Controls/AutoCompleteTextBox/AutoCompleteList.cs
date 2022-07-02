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
// 文件名称：AutoCompleteList
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:27:08
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
    public class AutoCompleteList : ScrollableControl
    {
        private readonly ToolTip toolTip = new ToolTip();
        private int hoveredItemIndex = -1;
        private int oldItemCount;
        private int selectedItemIndex = -1;
        private IList<AutocompleteItem> visibleItems;

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
            ItemHeight = Font.Height + 2;
            VerticalScroll.SmallChange = ItemHeight;
            BackColor = Color.White;
        }

        private int itemHeight;

        public int ItemHeight
        {
            get { return itemHeight; }
            set
            {
                itemHeight = value;
                VerticalScroll.SmallChange = value;
                oldItemCount = -1;
                AdjustScroll();
            }
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                ItemHeight = Font.Height + 2;
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
            int y = 0;
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
                if (i == hoveredItemIndex)
                {
                    e.Graphics.DrawRectangle(Pens.Red, textRect);
                }

                var sf = new StringFormat();
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
                    IsHovered = i == hoveredItemIndex
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
        }

        private void OnItemSelected()
        {
            if (ItemSelected != null)
            {
                ItemSelected(this, EventArgs.Empty);
            }
        }

        private void OnViewClick()
        {
            if (ViewClick != null)
            {
                ViewClick(this, EventArgs.Empty);
            }
        }

        private void OnViewDoubleClick()
        {
            if (ViewDoubleClick != null)
            {
                ViewDoubleClick(this, EventArgs.Empty);
            }
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
