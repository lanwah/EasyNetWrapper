using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Controls.AutoCompleteTextBox
// 文件名称：IAutoCompleteConverter
// 创 建 者：lanwah
// 创建日期：2021/02/23 16:09:21
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
}
