using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：ObjectExts
// 创 建 者：lanwah
// 创建日期：2022/7/2 9:41:18
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// Object类型 扩展方法
    /// </summary>
    public static class ObjectExts
    {
        /// <summary>
        /// 判断对象是否为Null
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNull(this object @this)
        {
            return (@this is null);
        }
        /// <summary>
        /// 判断对象是否不为Null
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object @this)
        {
            return !@this.IsNull();
        }

        /// <summary>
        /// 检查对象是为空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="this">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        /// <param name="message">自定义错误信息</param>
        /// <exception cref="ArgumentNullException"></exception>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void ThrowIfNull(this object @this, string argumentName, string message = "")
        {
            //// "值不能为 null。\r\n参数名: Test"
            //throw new ArgumentNullException("Test");

            //// string类型参数值不能为 null。\r\n参数名: Test
            //throw new ArgumentNullException("Test", "string类型参数值不能为 null。");

            //// "string类型参数值不能为 null。"
            //throw new ArgumentNullException("", "string类型参数值不能为 null。");

            ThrowIfNullInternal(@this, argumentName, message);
        }
        /// <summary>
        /// 检查对象是为空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="this">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        /// <param name="message">自定义错误信息</param>
        /// <exception cref="ArgumentNullException"></exception>
#if NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [DebuggerStepThrough]
        internal static void ThrowIfNullInternal(object @this, string argumentName, string message = "")
        {
            if (@this.IsNull())
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }
        /// <summary>
        /// 通过反射获取字段值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fieldName">字段名称，支持多层关系，用.分隔多层</param>
        /// <returns></returns>
        public static object GetValue(this object @this, string fieldName)
        {
            if (@this.IsNull() || fieldName.IsNullOrEmpty())
            {
                return null;
            }

            var value = @this;
            var fields = fieldName.Split('.');
            foreach (var field in fields)
            {
                value = @this.GetValueInternal(field);
                if (value.IsNull())
                {
                    break;
                }
            }

            return value;
        }
        /// <summary>
        /// 通过反射获取字段值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="fieldName">字段名称，不支持多层关系</param>
        /// <returns></returns>
        internal static object GetValueInternal(this object @this, string fieldName)
        {
            if (@this.IsNull() || fieldName.IsNullOrEmpty())
            {
                return null;
            }

            var type = @this.GetType();
            var propertyInfo = type.GetProperty(fieldName);
            if (propertyInfo.IsNull())
            {
                return null;
            }

            return propertyInfo.GetValue(@this, null);
        }
        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <param name="this"></param>
        /// <benchmark>\ObjectConverter\ObjectToString.cs</benchmark>
        /// <returns>对象对应的字符串</returns>
        public static string ToStringEx(this object @this)
        {
            if (@this.IsNull())
            {
                return string.Empty;
            }

            return @this.ToString();
        }
        /// <summary>
        /// 返回一个指定类型的对象，转换失败不抛出异常返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="defaultValue">转换失败时的默认值</param>
        /// <returns></returns>
        public static T Cast<T>(this object @this, T defaultValue = default)
        {
            try
            {
                return @this.Cast<T>();
            }
            catch (InvalidCastException ex)
            {
                Debug.WriteLine(ex);
                return defaultValue;
            }
        }
        /// <summary>
        /// 返回一个指定类型的对象，转换失败会抛出<see cref="InvalidCastException"/>异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <exception cref="InvalidCastException"></exception>
        /// <benchmark>\ObjectConverter\ObjectToInt.cs</benchmark>
        /// <returns></returns>
        public static T Cast<T>(this object @this)
        {
            return (T)@this;
        }
    }
}
