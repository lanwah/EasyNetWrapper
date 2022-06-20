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
    public static partial class ExtensionUnity
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
            var attributes = member.GetCustomAttributes(typeof(T), inherit) as T[];

            return attributes.FirstOrDefault();
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

        #region // Type 扩展        
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
        /// <summary>
        /// 获取Description属性值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string Description(this PropertyInfo property)
        {
            var attr = property.GetAttribute<DescriptionAttribute>();
            return attr?.Description;
        }
        /// <summary>
        /// 获取DisplayName属性值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string DisplayName(this PropertyInfo property)
        {
            var attr = property.GetAttribute<DisplayNameAttribute>();
            return attr?.DisplayName;
        }
        /// <summary>
        /// 类型是否能够转为指定基类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType">基类</param>
        /// <returns></returns>
        public static bool As(this Type type, Type baseType)
        {
            if (type == null)
            {
                return false;
            }

            // 如果基类是泛型定义
            if (baseType.IsGenericTypeDefinition && type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                type = type.GetGenericTypeDefinition();
            }

            if (type == baseType)
            {
                return true;
            }

            if (baseType.IsAssignableFrom(type))
            {
                return true;
            }

            var rs = false;

            // 接口
            if (baseType.IsInterface)
            {
                if (type.GetInterface(baseType.FullName) != null)
                {
                    rs = true;
                }
                else if (type.GetInterfaces().Any(e => e.IsGenericType && baseType.IsGenericTypeDefinition ? e.GetGenericTypeDefinition() == baseType : e == baseType))
                {
                    rs = true;
                }
            }

            // 判断是否子类时，支持只反射加载的程序集
            if (!rs && type.Assembly.ReflectionOnly)
            {
                // 反射加载时，需要特殊处理接口
                while (!rs && type != typeof(object))
                {
                    if (type == null)
                    {
                        continue;
                    }

                    if (type.FullName == baseType.FullName && type.AssemblyQualifiedName == baseType.AssemblyQualifiedName)
                    {
                        rs = true;
                    }

                    type = type.BaseType;
                }
            }

            return rs;
        }
        /// <summary>
        /// 是否为List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(this Type type)
        {
            return ((type != null) && type.IsGenericType && type.As(typeof(IList<>)));
        }
        /// <summary>
        /// 判断是否为字典
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type type)
        {
            return ((type != null) && type.IsGenericType && type.As(typeof(IDictionary<,>)));
        }
        #endregion

        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            if (null == obj)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断集合是否有值
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasData<T>(this IEnumerable<T> source)
        {
            if (null == source)
            {
                return false;
            }

            return (source.Count() > 0);
        }
    }
}
