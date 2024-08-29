using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using static System.Net.WebRequestMethods;
using EasyNet.Core;

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

namespace EasyNet.Extensions
{
    /// <summary>
    /// 默认无参构造函数委托
    /// </summary>
    /// <returns></returns>
    public delegate object DefaultConstructorHandler();

    /// <summary>
    /// Type/MethodBase/ParameterInfo/MemberInfo类型 扩展方法
    /// </summary>
    public static class ReflectionExts
    {
        internal const string MemberInfoNullMsg = "MemberInfo类型参数值为空，请检查！";
        internal const string MethodBaseNullMsg = "MethodBase类型参数值为空，请检查！";
        internal const string MethodInfoNullMsg = "MethodInfo类型参数值为空，请检查！";
        internal const string ParameterInfosNullMsg = "ParameterInfo[]类型参数值为空，请检查！";
        internal const string ICustomAttributeProviderNullMsg = "ICustomAttributeProvider类型参数值为空，请检查！";
        internal const string ConstructorInfoNullMsg = "ConstructorInfo类型参数值为空，请检查！";
        internal const string TypeNullMsg = "Type类型参数值为空，请检查！";


        /// <summary>
        /// 判断类型是否能够转为指定基类
        /// </summary>
        /// <param name="this"></param>
        /// <param name="baseType">基类</param>
        /// <returns></returns>
        public static bool As(this Type @this, Type baseType)
        {
            if (@this.IsNull() || baseType.IsNull())
            {
                return false;
            }

            // 如果基类是泛型定义
            if (baseType.IsGenericTypeDefinition && @this.IsGenericType && !@this.IsGenericTypeDefinition)
            {
                @this = @this.GetGenericTypeDefinition();
            }

            if (@this == baseType)
            {
                return true;
            }

            if (baseType.IsAssignableFrom(@this))
            {
                return true;
            }

            var rs = false;

            // 接口
            if (baseType.IsInterface)
            {
                if (@this.GetInterface(baseType.FullName) != null)
                {
                    rs = true;
                }
                else if (@this.GetInterfaces().Any(e => e.IsGenericType && baseType.IsGenericTypeDefinition ? e.GetGenericTypeDefinition() == baseType : e == baseType))
                {
                    rs = true;
                }
            }

            // 判断是否子类时，支持只反射加载的程序集
            if (!rs && @this.Assembly.ReflectionOnly)
            {
                // 反射加载时，需要特殊处理接口
                while (!rs && @this != typeof(object))
                {
                    if (@this == null)
                    {
                        continue;
                    }

                    if (@this.FullName == baseType.FullName && @this.AssemblyQualifiedName == baseType.AssemblyQualifiedName)
                    {
                        rs = true;
                    }

                    @this = @this.BaseType;
                }
            }

            return rs;
        }
        /// <summary>
        /// 判断是否为字典
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsDictionary(this Type @this)
        {
            return (@this.IsNotNull() && @this.IsGenericType && @this.As(typeof(IDictionary<,>)));
        }
        /// <summary>
        /// 判断是否为List
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsList(this Type @this)
        {
            return (@this.IsNotNull() && @this.IsGenericType && @this.As(typeof(IList<>)));
        }
        /// <summary>
        /// 通过反射调用静态方法
        /// </summary>
        /// <param name="this"></param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">方法参数</param>
        /// <returns>方法返回值</returns>
        public static object Invoke(this Type @this, string methodName, object[] parameters)
        {
            var methodInfo = @this.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (methodInfo.IsNotNull())
            {
                return methodInfo.Invoke(@this, parameters);
            }

            return null;
        }
        /// <summary>
        /// 得到默认无参构造函数委托
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static DefaultConstructorHandler GetDefaultConstructorHandler(this Type @this)
        {
            @this.ThrowIfNull(string.Empty, TypeNullMsg);
            var ctor = @this.CreateDefaultConstructorDelegate();

            object handler()
            {
                try
                {
                    return ctor();
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            };

            return handler;
        }
        /// <summary>
        /// 创建默认无参构造函数委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static DefaultConstructorHandler CreateDefaultConstructorDelegate(this Type type)
        {
            if (type == Types.String)
            {
#if NET5_0_OR_GREATER
                static object s()
                {
                    return null;
                }
#else
                object s()
                {
                    return null;
                }
#endif

                return s;
            }

            var ctorExpression = Expression.Lambda<DefaultConstructorHandler>(Expression.Convert(Expression.New(type), typeof(object)));
            return ctorExpression.Compile();
        }

        /// <summary>
        /// 获取方法参数类型
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static List<Type> GetParameterTypes(this MethodBase @this)
        {
            @this.ThrowIfNull(string.Empty, MethodBaseNullMsg);

            return @this.GetParameters().GetParameterTypes();
        }
        /// <summary>
        /// 获取参数类型
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        internal static List<Type> GetParameterTypes(this ParameterInfo[] @this)
        {
            @this.ThrowIfNull(string.Empty, ParameterInfosNullMsg);

            return @this.Select(parameterInfo => parameterInfo.ParameterType).ToList();
        }
        /// <summary>
        /// 获取Description特性值
        /// </summary>
        /// <param name="this"></param>
        /// <param name="memberName">调用方为Type类型时，此字段为成员名称。</param>
        /// <returns>Description特性值</returns>
        public static string Description(this MemberInfo @this, string memberName = "")
        {
            if (@this.IsNull())
            {
                return string.Empty;
            }

            if (memberName.IsNullOrEmpty())
            {
                var attr = @this.GetAttribute<DescriptionAttribute>();
                return attr?.Description;
            }
            else
            {
                if (@this is Type type)
                {
                    return type.GetMember(memberName).FirstOrDefault()?.Description();
                }
            }

            return null;
        }
        /// <summary>
        /// 获取DisplayName特性值
        /// <see href="https://learn.microsoft.com/zh-cn/dotnet/api/system.componentmodel.displaynameattribute?view=net-8.0"/>
        /// </summary>
        /// <param name="this"></param>
        /// <param name="memberName">调用方为Type类型时，此字段为成员名称。</param>
        /// <returns>DisplayName特性值</returns>
        public static string DisplayName(this MemberInfo @this, string memberName = "")
        {
            if (@this.IsNull())
            {
                return string.Empty;
            }

            if (memberName.IsNullOrEmpty())
            {
                var attr = @this.GetAttribute<DisplayNameAttribute>();
                return attr?.DisplayName;
            }
            else
            {
                if (@this is Type type)
                {
                    return type.GetMember(memberName).FirstOrDefault()?.DisplayName();
                }
            }

            return null;
        }
        /// <summary>
        /// 获取成员类型
        /// </summary>
        /// <param name="this">成员</param>
        /// <returns>成员类型</returns>
        public static Type GetMemberType(this MemberInfo @this)
        {
            @this.ThrowIfNull(string.Empty, MemberInfoNullMsg);

#if NETCOREAPP2_0_OR_GREATER
            return @this.MemberType switch
            {
                MemberTypes.Field => (@this as FieldInfo).FieldType,
                MemberTypes.Property => (@this as PropertyInfo).PropertyType,
                MemberTypes.Method => (@this as MethodInfo).ReturnType,
                _ => null
            };
#else
            switch (@this.MemberType)
            {
                case MemberTypes.Field:
                    return (@this as FieldInfo).FieldType;
                case MemberTypes.Property:
                    return (@this as PropertyInfo).PropertyType;
                case MemberTypes.Method:
                    return (@this as MethodInfo).ReturnType;
            }
            return null;
#endif
        }
        /// <summary>
        /// 判断成员是否包含特定的特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="this">成员</param>
        /// <param name="inherit">是否继承</param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this ICustomAttributeProvider @this, bool inherit = false)
            where T : Attribute
        {
            if (@this.IsNull())
            {
                return false;
            }

            return @this.IsDefined(typeof(T), inherit);
        }
        /// <summary>
        /// 获取指定的特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="this">成员</param>
        /// <param name="inherit">是否集成</param>
        /// <returns>返回元数据</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider @this, bool inherit = false)
            where T : Attribute
        {
            var attributes = @this.GetAttributes<T>(inherit);

            return attributes.FirstOrDefault();
        }
        /// <summary>
        /// 获取成员特性数组
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="this">成员</param>
        /// <param name="inherit">是否继承</param>
        /// <returns>返回成员特性数组</returns>
        internal static T[] GetAttributes<T>(this ICustomAttributeProvider @this, bool inherit = false)
            where T : Attribute
        {
            @this.ThrowIfNull(string.Empty, ICustomAttributeProviderNullMsg);

            return @this.GetCustomAttributes(typeof(T), inherit) as T[];
        }


    }
}
