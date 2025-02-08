#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.WinForm.Shared.Controls.AutoCompleteTextBox
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：IAutoCompleteConverter.cs
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
    /// 自动填充数据源转换器接口
    /// </summary>
    public interface IAutoCompleteConverter
    {
        /// <summary>
        /// 自动填充数据源转换器
        /// </summary>
        /// <returns></returns>
        AutocompleteItem AutocompleteItemBuilder();
    }

    /// <summary>
    /// 检索委托
    /// </summary>
    public interface IAutoCompleteSearch
    {
        /// <summary>
        /// 检索函数
        /// </summary>
        Func<string, AutocompleteItem[]> Search
        {
            get;
        }
    }

    /// <summary>
    /// 检索参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoCompleteSearchArgs<T> : IAutoCompleteSearch
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="callBack">检索实现委托，返回T类型数组</param>
        /// <param name="converter">转换器，实现把T类型转换成AutocompleteItem类型<see cref="AutocompleteItem"/></param>
        public AutoCompleteSearchArgs(Func<string, T[]> callBack, Converter<T, AutocompleteItem> converter)
        {
            this.SearchCallBack = callBack;
            this.Converter = converter;

            this.Search = this.BuilderTargetCallBack;
        }

        /// <summary>
        /// string - 输入的内容
        /// </summary>
        private readonly Func<string, T[]> SearchCallBack;
        /// <summary>
        /// 对象转换器
        /// </summary>
        private readonly Converter<T, AutocompleteItem> Converter;

        /// <summary>
        /// 查询及转换
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private AutocompleteItem[] BuilderTargetCallBack(string input)
        {
            var records = this.SearchCallBack?.Invoke(input);
            IEnumerable<AutocompleteItem> targets = records.Select(c =>
            {
                return this.Converter(c);
            });
            return targets.ToArray();
        }
        /// <summary>
        /// 目标检索委托
        /// </summary>
        public Func<string, AutocompleteItem[]> Search
        {
            get; private set;
        }
    }

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
        /// 是否为自定义新增的项
        /// </summary>
        public bool IsCustomItem
        {
            get;
            internal set;
        }

        /// <summary>
        /// 绘制项
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnPaint(PaintItemEventArgs e)
        {
            e.Graphics.DrawString(ItemText, e.Font, Brushes.Black, e.TextRect, e.StringFormat);
        }

        /// <summary>
        /// 用Key与fragmentText比较
        /// </summary>
        /// <param name="fragmentText"></param>
        /// <returns></returns>
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

    /// <summary>
    /// 检索项扩展方法
    /// </summary>
    public static class AutocompleteItemExts
    {
        /// <summary>
        /// 字符串转换成AutocompleteItem
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static AutocompleteItem ToAutocompleteItem(this string text)
        {
            return new AutocompleteItem()
            {
                Key = text,
                ItemText = text,
                DisplayText = text,
                ToolTipTitle = null,
                ToolTipText = null,
                Tag = text
            };
        }
    }

    internal delegate bool DialogKeyProcessor(Keys keyData);

    internal class SelectingEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public bool Cancel { get; set; }
        public int SelectedIndex { get; set; }
        public bool Handled { get; set; }
    }
    internal class SelectedEventArgs : EventArgs
    {
        public AutocompleteItem Item { get; internal set; }
        public Control Control { get; set; }
    }
    internal class PaintItemEventArgs : PaintEventArgs
    {
        public RectangleF TextRect { get; internal set; }
        public StringFormat StringFormat { get; internal set; }
        public Font Font { get; internal set; }
        public bool IsSelected { get; internal set; }
        public bool IsHovered { get; internal set; }

        public PaintItemEventArgs(Graphics graphics, Rectangle clipRect)
            : base(graphics, clipRect)
        {
        }
    }

    internal enum CompareResult
    {
        /// <summary>
        /// Item do not appears
        /// </summary>
        Hidden,
        /// <summary>
        /// Item appears
        /// </summary>
        Visible,
        /// <summary>
        /// Item appears and will selected
        /// </summary>
        VisibleAndSelected
    }
}
#endif
