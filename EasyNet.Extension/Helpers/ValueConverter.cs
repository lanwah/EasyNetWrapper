using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Extension
{
    /// <summary>
    /// 通过 TypeConverter 实现值与字符串值之间的转换
    /// </summary>
    public class ValueConverter
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
        /// <see cref="TypeConverter.ConvertFromString(ITypeDescriptorContext, System.Globalization.CultureInfo, string)"/>
        public static T ConvertFromString<T>(string value)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(value);
        }

        /// <summary>
        /// 通过类型转换器获取特定类型的字符串值
        /// </summary>
        /// <param name="value">类型值</param>
        /// <returns>字符串表示的类型值</returns>
        /// <see cref="TypeConverter.ConvertTo(ITypeDescriptorContext, System.Globalization.CultureInfo, object, Type)"/>
        public static string ConvertToString(object value)
        {
            value.NotNullCheck(nameof(value));

            return TypeDescriptor.GetConverter(value.GetType()).ConvertToString(value);
        }
    }
}
