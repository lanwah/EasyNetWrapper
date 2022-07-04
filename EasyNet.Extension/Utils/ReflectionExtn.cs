using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：IntPtrExtn
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

namespace EasyNet.Extension
{
    /// <summary>
    /// Type/Reflection 扩展方法
    /// </summary>
    public static class ReflectionExtn
    {
        /// <summary>
        /// 判断类型是否能够转为指定基类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType">基类</param>
        /// <returns></returns>
        public static bool As(this Type type, Type baseType)
        {
            if (type.IsNull() || baseType.IsNull())
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
        /// 判断是否为字典
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type type)
        {
            return (type.IsNotNull() && type.IsGenericType && type.As(typeof(IDictionary<,>)));
        }

        /// <summary>
        /// 判断是否为List
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsList(this Type type)
        {
            return (type.IsNotNull() && type.IsGenericType && type.As(typeof(IList<>)));
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
        internal static Type[] GetParameterTypes(this ParameterInfo[] parameterInfos)
        {
            parameterInfos.NotNullCheck(nameof(parameterInfos));

            return parameterInfos.Select(parameterInfo => parameterInfo.ParameterType).ToArray();
        }

        /// <summary>
        /// 获取Description特性值
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static string Description(this MemberInfo member, string memberName = "")
        {
            member.NotNullCheck(nameof(member));

            if (memberName.IsNullOrEmptyEx())
            {
                var attr = member.GetAttribute<DescriptionAttribute>();
                return attr?.Description;
            }
            else
            {
                if (member is Type type)
                {
                    return type.GetMember(memberName).FirstOrDefault()?.Description();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取DisplayName特性值
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static string DisplayName(this MemberInfo member, string memberName = "")
        {
            member.NotNullCheck(nameof(member));

            if (memberName.IsNullOrEmptyEx())
            {
                var attr = member.GetAttribute<DisplayNameAttribute>();
                return attr?.DisplayName;
            }
            else
            {
                if (member is Type type)
                {
                    return type.GetMember(memberName).FirstOrDefault()?.DisplayName();
                }
            }

            return null;
        }


        // ---------------------------------------------------------------------------------------------- //

        /// <summary>
        /// 判断成员是否包含特定的特性
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

        /// <summary>
        /// 获取指定的特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否集成</param>
        /// <returns>返回元数据</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            var attributes = member.GetAttributes<T>(inherit);

            return attributes.FirstOrDefault();
        }
        /// <summary>
        /// 获取成员特性数组
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否继承</param>
        /// <returns>返回成员特性数组</returns>
        internal static T[] GetAttributes<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            member.NotNullCheck(nameof(member));

            return member.GetCustomAttributes(typeof(T), inherit) as T[];
        }

    }
}
