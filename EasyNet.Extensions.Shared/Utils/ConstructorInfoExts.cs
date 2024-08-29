#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using EasyNet.Core;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.Utils
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：ConstructorInfoExts.cs
// 创建用户：lanwah
// 创建日期：2024/8/28 16:28:55
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// 构造函数信息扩展方法
    /// </summary>
    public static class ConstructorInfoExts
    {
        /// <summary>
        /// 获取构造函数委托
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static ConstructorHandler GetConstructorHandler(this System.Reflection.ConstructorInfo @this)
        {
            @this.ThrowIfNull(string.Empty, ReflectionExts.ConstructorInfoNullMsg);

            var ctor = @this.DeclaringType.IsValueType ? (args) => @this.Invoke(args) : @this.CreateConstructorDelegate();
            object handler(object[] args)
            {
                if (args == null)
                {
                    args = new object[@this.GetParameters().Length];
                }

                try
                {
                    return ctor(args);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex.InnerException;
                }
            }

            return handler;
        }

        /// <summary>
        /// 创建动态构造函数委托
        /// </summary>
        /// <param name="constructor"></param>
        /// <returns></returns>
        internal static ConstructorHandler CreateConstructorDelegate(this System.Reflection.ConstructorInfo constructor)
        {
            var callable = (DynamicMethod)CreateDynamicConstructorMethod();
            var info = new MethodMetaData(constructor);

            var returnType = constructor.ReflectedType;
            var il = callable.GetILGenerator();

            il.EmitLoadParameters(info, 0);
            il.Emit(OpCodes.Newobj, constructor);

            if (info.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(ConstructorHandler)) as ConstructorHandler;
        }
        private static DynamicMethod CreateDynamicConstructorMethod()
        {
            return new DynamicMethod(String.Empty, typeof(object), new[] { typeof(object[]) }, DynamicAssemblyManager.Module, true);
        }
    }

    /// <summary>
    /// MethodInfo 扩展方法
    /// </summary>
    public static partial class MethodInfoExts
    {
        /// <summary>
        /// 得到函数委托（有返回值函数）
        /// </summary>
        /// <param name="this">方法对象</param>
        /// <returns>返回函数委托</returns>
        public static MethodHandler GetMethodHandler(this System.Reflection.MethodInfo @this)
        {
            @this.ThrowIfNull(string.Empty, ReflectionExts.MethodInfoNullMsg);

            var func = @this.DeclaringType.IsValueType ? (target, args) => @this.Invoke(target, args) : @this.CreateMethodDelegate();

            object handler(object target, object[] args)
            {
                if (args == null)
                {
                    args = new object[@this.GetParameters().Length];
                }

                try
                {
                    return func(target, args);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return handler;
        }

        /// <summary>
        /// 创建动态方法的委托
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static MethodHandler CreateMethodDelegate(this MethodInfo method)
        {
            var dm = CreateDynamicMethod();
            method.EmitDynamicMethod(dm);

            return dm.CreateDelegate(typeof(MethodHandler)) as MethodHandler;
        }
        private static DynamicMethod CreateDynamicMethod()
        {
            return new DynamicMethod(String.Empty, typeof(object), new[] { typeof(object), typeof(object[]) }, DynamicAssemblyManager.Module, true);
        }
        private static void EmitDynamicMethod(this MethodInfo @this, DynamicMethod callable)
        {
            var info = new MethodMetaData(@this);

            var il = callable.GetILGenerator();
            il.EmitLoadParameters(info, 1);

            if (@this.IsStatic)
            {
                il.EmitCall(OpCodes.Call, @this, null);
            }
            else if (@this.IsVirtual)
            {
                il.EmitCall(OpCodes.Callvirt, @this, null);
            }
            else
            {
                il.EmitCall(OpCodes.Call, @this, null);
            }

            if (@this.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                if (@this.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, @this.ReturnType);
                }
            }
            il.Emit(OpCodes.Ret);
        }
    }

    /// <summary>
    /// MemberInfo 扩展方法
    /// </summary>
    public static partial class MemberInfoExts
    {
        /// <summary>
        /// 获取访问器委托
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static GetHandler GetGetHandler(this System.Reflection.MemberInfo @this)
        {
            @this.ThrowIfNull(string.Empty, ReflectionExts.MemberInfoNullMsg);

            GetHandler getter = null;
            if (@this.DeclaringType.IsValueType)
            {
                switch (@this.MemberType)
                {
                    case MemberTypes.Field:
                        getter = (target) => (@this as FieldInfo).GetValue(target);
                        break;
                    case MemberTypes.Property:
                        getter = (target) => (@this as PropertyInfo).GetValue(target, null);
                        break;
                    case MemberTypes.Method:
                        getter = (target) => (@this as MethodInfo).Invoke(target, new object[] { });
                        break;
                }

            }
            else
            {
                getter = @this.CreateGetDelegate();
            }

            return target =>
            {
                try
                {
                    return getter(target);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }

        /// <summary>
        /// 创建访问器委托
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        internal static GetHandler CreateGetDelegate(this System.Reflection.MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field: return CreateGetDelegate(member as FieldInfo);
                case MemberTypes.Property: return CreateGetDelegate(member as PropertyInfo);
                case MemberTypes.Method: return CreateGetDelegate(member as MethodInfo);
            }

            return null;
        }
        private static GetHandler CreateGetDelegate(FieldInfo field)
        {
            var callable = CreateDynamicGetMethod();

            var returnType = field.FieldType;
            var il = callable.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.EmitUnboxOrCast(field.DeclaringType);
            il.Emit(OpCodes.Ldfld, field);

            if (returnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(GetHandler)) as GetHandler;
        }
        private static GetHandler CreateGetDelegate(PropertyInfo property)
        {
            var method = property.GetGetMethod();
            method = method ?? property.GetGetMethod(true);

            return CreateGetDelegate(method);
        }
        private static GetHandler CreateGetDelegate(MethodInfo method)
        {
            var callable = CreateDynamicGetMethod();

            var returnType = method.ReturnType;
            var il = callable.GetILGenerator();


            il.Emit(OpCodes.Ldarg_0);
            il.EmitUnboxOrCast(method.DeclaringType);

            if (method.IsFinal)
            {
                il.Emit(OpCodes.Call, method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, method);
            }

            if (returnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(GetHandler)) as GetHandler;
        }
        private static readonly System.Reflection.Module Module = DynamicAssemblyManager.Module;
        private static DynamicMethod CreateDynamicGetMethod()
        {
            return new DynamicMethod(String.Empty, typeof(object), new[] { typeof(object) }, Module, true);
        }

        /// <summary>
        /// 得到设置器委托
        /// </summary>
        /// <param name="this">成员</param>
        /// <returns>返回设置器委托</returns>
        public static SetHandler GetSetHandler(this System.Reflection.MemberInfo @this)
        {
            @this.ThrowIfNull(string.Empty, ReflectionExts.MemberInfoNullMsg);

            SetHandler setter = null;
            if (@this.DeclaringType.IsValueType)
            {
                switch (@this.MemberType)
                {
                    case MemberTypes.Field:
                        setter = (target, value) => (@this as FieldInfo).SetValue(target, value);
                        break;
                    case MemberTypes.Property:
                        setter = (target, value) => (@this as PropertyInfo).SetValue(target, value, null);
                        break;
                    case MemberTypes.Method:
                        setter = (target, value) => (@this as MethodInfo).Invoke(target, new object[] { value });
                        break;
                }
            }
            else
            {
                setter = @this.CreateSetHandler();
            }

            return (target, value) =>
            {
                setter?.Invoke(target, value);
            };
        }

        /// <summary>
        /// 创建设置器委托
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        internal static SetHandler CreateSetHandler(this System.Reflection.MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return CreateSetDelegate(member as FieldInfo);
                case MemberTypes.Property:
                    return CreateSetDelegate(member as PropertyInfo);
                case MemberTypes.Method:
                    return CreateSetDelegate(member as MethodInfo);
            }

            return null;
        }
        private static SetHandler CreateSetDelegate(FieldInfo field)
        {
            var callable = CreateDynamicSetMethod();

            var returnType = field.FieldType;
            var il = (ILGenerator)callable.GetILGenerator();

            il.DeclareLocal(returnType);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitUnboxOrCast(returnType);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_0);
            il.EmitUnboxOrCast(field.DeclaringType);
            il.Emit(OpCodes.Ldloc_0);

            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(SetHandler)) as SetHandler;
        }
        private static SetHandler CreateSetDelegate(PropertyInfo property)
        {
            var method = property.GetSetMethod();
            method = method ?? property.GetSetMethod(true);

            return CreateSetDelegate(method);
        }
        private static SetHandler CreateSetDelegate(MethodInfo method)
        {
            var dm = CreateDynamicSetMethod();

            Type returnType = method.GetParameterTypes()[0];
            ILGenerator il = dm.GetILGenerator();
            il.DeclareLocal(returnType);

            il.Emit(OpCodes.Ldarg_1);
            il.EmitUnboxOrCast(returnType);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_0);
            il.EmitUnboxOrCast(method.DeclaringType);
            il.Emit(OpCodes.Ldloc_0);

            if (method.IsFinal)
            {
                il.Emit(OpCodes.Call, method);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, method);
            }

            il.Emit(OpCodes.Ret);

            return dm.CreateDelegate(typeof(SetHandler)) as SetHandler;
        }
        private static DynamicMethod CreateDynamicSetMethod()
        {
            return new DynamicMethod(String.Empty, typeof(void), new[] { typeof(object), typeof(object) }, Module, true);
        }
    }

    /// <summary>
    /// 动态程序集管理器
    /// </summary>
    public class DynamicAssemblyManager
    {
        internal static Module Module
        {
            get;
            private set;
        }
        private static AssemblyName AssemblyName
        {
            get;
            set;
        }
        private static AssemblyBuilder AssemblyBuilder
        {
            get; set;
        }
        private static ModuleBuilder ModuleBuilder
        {
            get; set;
        }
        /// <summary>
        /// 动态程序集名称
        /// </summary>
        internal const string NAME = "EasyNetDynamicAssembly";

        static DynamicAssemblyManager()
        {
            AssemblyName = new AssemblyName(NAME);
            AssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(AssemblyName.Name, $"{AssemblyName.Name}.dll", true);
            Module = AssemblyBuilder.GetModules().FirstOrDefault();
        }

        /// <summary>
        /// 保存动态程序集
        /// </summary>
        public static void SaveAssembly()
        {
            lock (typeof(DynamicAssemblyManager))
            {
                AssemblyBuilder.Save(AssemblyName.Name + ".dll");
            }
        }
        private static string CorrectTypeName(string typeName)
        {
            if (typeName.Length >= 1042)
            {
                typeName = $"type_{typeName.Substring(0, 900)}{Guid.NewGuid().ToString().Replace("-", "")}";
            }
            return typeName;
        }

        internal static TypeBuilder DefineType(string typeName, Type parent)
        {
            lock (typeof(DynamicAssemblyManager))
            {
                return ModuleBuilder.DefineType(CorrectTypeName(typeName), TypeAttributes.Public, parent, null);
            }
        }
    }
}
#endif
