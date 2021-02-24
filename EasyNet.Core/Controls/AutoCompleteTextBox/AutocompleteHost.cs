using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutocompleteHost
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:25:47
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
        public readonly AutoCompleteIMEControl Owner;
        public AutocompleteHost(AutoCompleteIMEControl owner)
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
            if (Selecting != null)
            {
                Selecting(this, args);
            }
        }

        internal void OnSelected(SelectedEventArgs args)
        {
            if (Selected != null)
            {
                Selected(this, args);
            }
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // AutocompleteHost
            // 
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Size = new System.Drawing.Size(0, 0);
            this.ResumeLayout(false);

        }
    }
}
