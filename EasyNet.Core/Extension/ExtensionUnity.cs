using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Extension
// 文件名称：ExtensionUnity
// 创 建 者：lanwah
// 创建日期：2021/05/31 17:28:43
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
    /// 扩展方法
    /// </summary>
    public static class ExtensionUnity
    {
        #region // IEnumerable 扩展
        /// <summary>
        /// 去重
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> DistinctBy<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
        private class CommonEqualityComparer<T, V> : IEqualityComparer<T>
        {
            private Func<T, V> keySelector;

            public CommonEqualityComparer(Func<T, V> keySelector)
            {
                this.keySelector = keySelector;
            }

            public bool Equals(T x, T y)
            {
                return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
            }

            public int GetHashCode(T obj)
            {
                return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
            }
        }
        #endregion

        #region // IntPtr 扩展
        /// <summary>
        /// IntPtr 转 Ansi 字符串
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ToAnsiString(this IntPtr handle, string defaultValue = "")
        {
            if ((null == handle) || (IntPtr.Zero == handle))
            {
                return defaultValue;
            }

            return Marshal.PtrToStringAnsi(handle);
        }
        #endregion

        #region // ICustomAttributeProvider 扩展
        /// <summary>
        /// 获取成员特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否集成</param>
        /// <returns>返回元数据</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            member.NotNullCheck(nameof(member));
            T[] attributes = member.GetCustomAttributes(typeof(T), inherit) as T[];

            if ((attributes == null) || (attributes.Length == 0))
            {
                return null;
            }

            return attributes[0];
        }
        /// <summary>
        /// 得到成员特性数组
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否集成</param>
        /// <returns>返回成员元数据数组</returns>
        public static T[] GetAttributes<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            member.NotNullCheck(nameof(member));
            return member.GetCustomAttributes(typeof(T), inherit) as T[];
        }
        /// <summary>
        /// 判断成员是否包含特点的特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否继承</param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            member.NotNullCheck(nameof(member));
            return member.IsDefined(typeof(T), inherit);
        }
        #endregion

        #region // string 扩展
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
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmptyEx(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region // object 扩展
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
        /// Object 转对应的类型
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue As<TValue>(this object value, TValue defaultValue = default(TValue))
        {
            if (value != null && !(value is DBNull))
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

        // Type 相关
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
            string[] properties = propertyName.Split('.');
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
        /// <summary>
        /// 获取参数类型
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Type[] GetParameterTypes(this MethodBase method)
        {
            method.NotNullCheck(nameof(method));
            return method.GetParameters().GetParameterTypes();
        }
        /// <summary>
        /// 获取参数类型
        /// </summary>
        /// <param name="parameterInfos"></param>
        /// <returns></returns>
        public static Type[] GetParameterTypes(this ParameterInfo[] parameterInfos)
        {
            parameterInfos.NotNullCheck(nameof(parameterInfos));
            return (from p in parameterInfos select p.ParameterType).ToArray();
        }
        #endregion
    }
}
