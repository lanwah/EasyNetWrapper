using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyNet.Controls
{
    [ToolboxItem(false)]
    internal partial class AutoCompleteView : UserControl
    {
        internal AutocompleteHost Host { get; set; }

        private IEnumerable<AutocompleteItem> sourceItems
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

            tbxInput.MouseWheel += new MouseEventHandler(tbxInput_MouseWheel);
            this.Height = pnlTop.Height;

            autoCompleteList.ItemSelected += new EventHandler(autoCompleteList_ItemSelected);
        }

        private void autoCompleteList_ItemSelected(object sender, EventArgs e)
        {
            OnSelecting();
        }

        private void tbxInput_TextChanged(object sender, EventArgs e)
        {
            bool foundSelected = false;
            int selectedIndex = -1;

            string text = tbxInput.Text;
            var visibleItems = new List<AutocompleteItem>();

            if (Host.SearchCallback != null)
            {
                autoCompleteList.VisibleItems = Host.SearchCallback(text).ToList();
            }
            else if (sourceItems != null)
            {
                foreach (AutocompleteItem item in sourceItems)
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
                        selectedIndex = visibleItems.Count - 1;
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

        private bool tbxInput_DoProcessDialogKey(Keys keyData)
        {
            return ProcessKey(keyData, Keys.None);
        }

        private void tbxInput_MouseWheel(object sender, MouseEventArgs e)
        {
            autoCompleteList.SetMouseWheel(e);
        }

        public void Init()
        {
            tbxInput.TextChanged -= new EventHandler(tbxInput_TextChanged);
            tbxInput.Text = string.Empty;
            autoCompleteList.Clear();
            CalcSize();
            tbxInput.TextChanged += new EventHandler(tbxInput_TextChanged);
        }
    }
}
