using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Extension
// 文件名称：ObjectExtension
// 创 建 者：lanwah
// 创建日期：2021/05/10 14:26:51
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Extension
{
    /// <summary>
    /// TypeConverter 类型值与类型值字符串之间转换相关
    /// </summary>
    public static class ValueConverterExtension
    {
        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ConvertFromString<T>(this string value)
        {
            return ValueConverter.ConvertFromString<T>(value);
        }
        /// <summary>
        /// 对象转对应的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToString(this object value)
        {
            return ValueConverter.ConvertToString(value);
        }

        //// 调用例子
        //var linkWard = new LinkWard() { WardName = "骨科病区" };
        //var linkWardString = linkWard.ConvertToString();
        //var newLinkWard = linkWardString.ConvertFromString<LinkWard>();
    }

    /// <summary>
    /// 通过 TypeConverter 实现值与字符串值之间的转换
    /// </summary>
    internal class ValueConverter
    {
        /// <summary>
        /// 获取提示信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNotSupportedMessage(Type type)
        {
            return $"不支持转换，可能原因是未找到{type}类型的转换器，请实现转换器类（继承TypeConverter类并重写相关接口）。";
        }

        /// <summary>
        /// 通过类型转换器获取特定类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">字符串类型值</param>
        /// <returns>类型值</returns>
        public static T ConvertFromString<T>(string value)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }
        /// <summary>
        /// 通过类型转换器获取特定类型的字符串值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">类型值</param>
        /// <returns>字符串类型值</returns>
        public static string ConvertToString(object value)
        {
            return TypeDescriptor.GetConverter(value.GetType()).ConvertToString(value);
        }
    }


    #region // 自定义TypeConverter举例
    /*
    
    [TypeConverter(typeof(LinkWardConverter))]
    public class LinkWard
    {
        /// <summary>
        /// 是否全院模板
        /// </summary>
        public bool IsGlobal
        {
            get; set;
        }
        /// <summary>
        /// 关联科室名称
        /// </summary>
        public string WardName { get; set; }
    }

    public class LinkWardConverter : TypeConverter
    {
        /// <summary>
        /// LinkWard 转字符串
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }

            if (value is LinkWard)
            {
                if (destinationType == typeof(string))
                {
                    LinkWard linkWard = (LinkWard)value;
                    if (culture == null)
                    {
                        culture = CultureInfo.CurrentCulture;
                    }
                    if (linkWard.IsGlobal)
                    {
                        return "[全院]";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(linkWard.WardName))
                        {
                            return "(未设置)";
                        }

                        //string separator = culture.TextInfo.ListSeparator + " ";
                        //return string.Join(separator, Array.ConvertAll<WSDepartment, string>(linkWard.Wards, a => a.name));
                        return linkWard.WardName;
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// 字符串转LinkWard
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if ((value is string val) && (!string.IsNullOrEmpty(val)))
            {
                val = val.Trim().Replace(" ", "");
                if (val.Length == 0)
                {
                    return null;
                }
                if (culture == null)
                {
                    culture = CultureInfo.CurrentCulture;
                }

                //char separator = culture.TextInfo.ListSeparator[0];
                var isGlobal = false;
                var wardName = string.Empty;
                if (val.Equals("[全院]"))
                {
                    isGlobal = true;
                }
                else if (val.Equals("(未设置)"))
                {
                    wardName = null;
                }
                else
                {
                    wardName = val;
                }

                return new LinkWard() { IsGlobal = isGlobal, WardName = wardName };
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    */
    #endregion
}
