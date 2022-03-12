using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Extension
// 文件名称：Object
// 创 建 者：lanwah
// 创建日期：2022/2/10 17:11:02
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
    /// Object 扩展
    /// </summary>
    public static partial class ExtensionUnity
    {
        /// <summary>
        /// 对象转对应的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToString(this object value)
        {
            return ValueConverter.ConvertToString(value);
        }
        /// <summary>
        /// 对象判空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNull(this object value)
        {
            if (null == value)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// 判断对象是否非空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object value)
        {
            return (!IsNull(value));
        }
        /// <summary>
        /// Object 转对应的类型
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue As<TValue>(this object value, TValue defaultValue = default(TValue))
        {
            if ((value != null) && !(value is DBNull))
            {
                try
                {
                    var typeFromHandle = typeof(TValue);
                    var type = value.GetType();
                    if (typeFromHandle.IsAssignableFrom(type))
                    {
                        return (TValue)value;
                    }
                    //if (value is JToken)
                    //{
                    //// 需要 Newtonsoft.Json.dll
                    //    return ((JToken)value).Value<TValue>();
                    //}
                    if (typeFromHandle.IsEnum)
                    {
                        if (type == typeof(int))
                        {
                            return (TValue)Enum.ToObject(typeFromHandle, value);
                        }
                        return (TValue)Enum.Parse(typeFromHandle, value.ToString());
                    }
                    var converter = TypeDescriptor.GetConverter(typeFromHandle);
                    if (converter.CanConvertFrom(value.GetType()))
                    {
                        return (TValue)converter.ConvertFrom(value);
                    }
                    converter = TypeDescriptor.GetConverter(value.GetType());
                    if (converter.CanConvertTo(typeFromHandle))
                    {
                        return (TValue)converter.ConvertTo(value, typeFromHandle);
                    }
                    return defaultValue;
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// 获取成员属性值，包括私有属性
        /// （带get/set）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object value, string propertyName)
        {
            var retValue = value;
            var properties = propertyName.Split('.');
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (string property in properties)
            {
                if (retValue == null)
                {
                    break;
                }

                Type type = retValue.GetType();
                var propertyInfo = type.GetProperty(property, bindFlags);
                if (propertyInfo == null)
                {
                    retValue = null;
                    break;
                }
                else
                {
                    retValue = propertyInfo.GetValue(retValue, null);
                }
            }

            return retValue;
        }
        /// <summary>
        /// 获取成员字段值，包括私有字段
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static object GetFieldValue(this object value, string fieldName)
        {
            var retValue = value;
            string[] fields = fieldName.Split('.');
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (string field in fields)
            {
                if (retValue == null)
                {
                    break;
                }

                Type type = retValue.GetType();
                var fieldInfo = type.GetField(field, bindFlags);
                if (fieldInfo == null)
                {
                    retValue = null;
                    break;
                }
                else
                {
                    retValue = fieldInfo.GetValue(retValue);
                }
            }

            return retValue;
        }
    }
}
