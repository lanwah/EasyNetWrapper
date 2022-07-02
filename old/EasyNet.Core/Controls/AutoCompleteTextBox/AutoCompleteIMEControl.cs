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
    /// <summary>
    /// 检索下拉文本框（只支持从候选项中选择内容，不允许输入，输入只是检索的触发动作）
    /// </summary>
    public partial class AutoCompleteIMEControl : UserControl
    {
        internal AutocompleteHost Host { get; set; }

        public event EventHandler DataChanged;

        public AutoCompleteIMEControl()
        {
            InitializeComponent();
            Host = new AutocompleteHost(this);
            Host.Selecting += new EventHandler<SelectingEventArgs>(Host_Selecting);
            autoCompleteDataItem.Click += new EventHandler(autoCompleteDataItem_Click);
            autoCompleteDataItem.DataDeleted += new EventHandler(autoCompleteDataItem_DataDeleted);
        }

        private void autoCompleteDataItem_DataDeleted(object sender, EventArgs e)
        {
            if (DataChanged != null)
            {
                DataChanged(this, e);
            }
        }

        public IEnumerable<AutocompleteItem> SourceItems
        {
            get { return Host.SourceItems; }
            set { Host.SourceItems = value; }
        }

        /// <summary>
        /// 检索回调函数
        /// </summary>
        public Func<string, AutocompleteItem[]> SearchCallback
        {
            get { return Host.SearchCallback; }
            set { Host.SearchCallback = value; }
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

        public void Host_Selecting(object sender, SelectingEventArgs e)
        {
            autoCompleteDataItem.SetDataItem(e.Item);
            if (DataChanged != null)
            {
                DataChanged(this, e);
            }
        }

        public AutocompleteItem SelectedItem
        {
            get
            {
                return autoCompleteDataItem.DataItem;
            }
            set
            {
                autoCompleteDataItem.SetDataItem(value);

                if (DataChanged != null)
                {
                    DataChanged(this, EventArgs.Empty);
                }
            }
        }

        private void autoCompleteDataItem_Click(object sender, EventArgs e)
        {
            if (autoCompleteDataItem.DataItem == null)
            {
                Host.Show(PointToScreen(autoCompleteDataItem.Location));
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        public void FocusEx()
        {
            if (!Host.Visible)
            {
                autoCompleteDataItem_Click(null, null);
            }
        }
    }
}
