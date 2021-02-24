using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：AutocompleteItem
// 创 建 者：lanwah
// 创建日期：2021/02/23 15:24:38
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
    /// <summary>
    /// 检索项
    /// </summary>
    public class AutocompleteItem
    {
        /// <summary>
        /// 选中后的显示文本
        /// </summary>
        public virtual string DisplayText { get; set; }
        /// <summary>
        /// 项文本（项显示内容）
        /// </summary>
        public virtual string ItemText { get; set; }
        /// <summary>
        /// 悬浮提示标题
        /// </summary>
        public virtual string ToolTipTitle { get; set; }
        /// <summary>
        /// 悬浮提示内容
        /// </summary>
        public virtual string ToolTipText { get; set; }
        /// <summary>
        /// 项匹配字段（输入的字段与此字段中的值从头进行匹配，只有在使用SourceItems进行检索时有效，在使用SearchCallback时无效）
        /// </summary>
        public virtual string Key { get; set; }
        /// <summary>
        /// 关联的自定义数据
        /// </summary>
        public virtual object Tag { get; set; }
        /// <summary>
        /// 关联的图标索引
        /// </summary>
        public virtual int ImageIndex { get; set; }
        /// <summary>
        /// 绘制项
        /// </summary>
        /// <param name="e"></param>
        public virtual void OnPaint(PaintItemEventArgs e)
        {
            e.Graphics.DrawString(ItemText, e.Font, Brushes.Black, e.TextRect, e.StringFormat);
        }

        internal virtual CompareResult Compare(string fragmentText)
        {
            if (string.IsNullOrEmpty(fragmentText))
            {
                return CompareResult.Hidden;
            }

            if (Key.StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase))
            {
                return CompareResult.VisibleAndSelected;
            }

            return CompareResult.Hidden;
        }
    }
}
