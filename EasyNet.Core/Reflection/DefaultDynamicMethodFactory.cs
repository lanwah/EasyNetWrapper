using EasyNet.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：DefaultDynamicMethodFactory
// 创 建 者：lanwah
// 创建日期：2021/07/01 11:15:08
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Reflection
{
    internal class DefaultDynamicMethodFactory
    {
        #region Getter
        private static Getter CreateGetter(FieldInfo field)
        {
            DynamicMethod callable = CreateDynamicGetterMethod();

            Type returnType = field.FieldType;
            ILGenerator il = callable.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.UnboxOrCast(field.DeclaringType);
            il.Emit(OpCodes.Ldfld, field);

            if (returnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(Getter)) as Getter;
        }
        private static Getter CreateGetter(PropertyInfo property)
        {
            MethodInfo method = property.GetGetMethod();

            if (method == null)
            {
                method = property.GetGetMethod(true);
            }

            return CreateGetter(method);
        }
        private static Getter CreateGetter(MethodInfo method)
        {
            var callable = CreateDynamicGetterMethod();

            Type returnType = method.ReturnType;
            ILGenerator il = callable.GetILGenerator();


            il.Emit(OpCodes.Ldarg_0);
            il.UnboxOrCast(method.DeclaringType);

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

            return callable.CreateDelegate(typeof(Getter)) as Getter;
        }

        public static Getter CreateGetter(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field: return CreateGetter(member as FieldInfo);
                case MemberTypes.Property: return CreateGetter(member as PropertyInfo);
                case MemberTypes.Method: return CreateGetter(member as MethodInfo);
            }

            return null;
        }
        #endregion


        #region Setter
        public static Setter CreateSetter(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return CreateSetter(member as FieldInfo);
                case MemberTypes.Property:
                    return CreateSetter(member as PropertyInfo);
                case MemberTypes.Method:
                    return CreateSetter(member as MethodInfo);
            }

            return null;
        }
        private static Setter CreateSetter(FieldInfo field)
        {
            DynamicMethod callable = CreateDynamicSetterMethod();

            Type returnType = field.FieldType;
            ILGenerator il = callable.GetILGenerator();

            il.DeclareLocal(returnType);

            il.Emit(OpCodes.Ldarg_1);
            il.UnboxOrCast(returnType);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_0);
            il.UnboxOrCast(field.DeclaringType);
            il.Emit(OpCodes.Ldloc_0);

            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(Setter)) as Setter;
        }
        private static Setter CreateSetter(PropertyInfo property)
        {
            MethodInfo method = property.GetSetMethod();
            if (method == null)
            {
                method = property.GetSetMethod(true);
            }

            return CreateSetter(method);
        }

        private static Setter CreateSetter(MethodInfo method)
        {
            var dm = CreateDynamicSetterMethod();

            Type returnType = method.GetParameterTypes()[0];
            ILGenerator il = dm.GetILGenerator();
            il.DeclareLocal(returnType);

            il.Emit(OpCodes.Ldarg_1);
            il.UnboxOrCast(returnType);
            il.Emit(OpCodes.Stloc_0);

            il.Emit(OpCodes.Ldarg_0);
            il.UnboxOrCast(method.DeclaringType);
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

            return dm.CreateDelegate(typeof(Setter)) as Setter;
        }

        #endregion

        public static Proc CreateProcMethod(MethodInfo method)
        {
            var func = CreateMethod(method);
            Proc result = (target, args) => func(target, args);
            return result;
        }
        public static Meth CreateMethod(MethodInfo method)
        {
            var dm = CreateDynamicMeth();
            EmitHelper.EmitDynamicMethod(method, dm);

            return dm.CreateDelegate(typeof(Meth)) as Meth;
        }
        public static DefaultConstructorHandler CreateDefaultConstructorMethod(Type type)
        {
            if (type == Types.String)
            {
                DefaultConstructorHandler s = () => null;
                return s;
            }

            var ctorExpression = Expression.Lambda<DefaultConstructorHandler>(Expression.Convert(Expression.New(type), typeof(object)));
            return ctorExpression.Compile();
        }
        public static ConstructorHandler CreateConstructorMethod(ConstructorInfo constructor)
        {
            DynamicMethod callable = CreateDynamicFactoryMethod();
            var info = new MethodMetaData(constructor);

            Type returnType = constructor.ReflectedType;
            ILGenerator il = callable.GetILGenerator();

            il.EmitLoadParameters(info, 0);
            il.Emit(OpCodes.Newobj, constructor);

            if (info.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, returnType);
            }

            il.Emit(OpCodes.Ret);

            return callable.CreateDelegate(typeof(ConstructorHandler)) as ConstructorHandler;
        }


        private static readonly Module Module = DynamicAssemblyManager.Module;
        private static DynamicMethod CreateDynamicGetterMethod()
        {
            return new DynamicMethod(String.Empty, typeof(object), new[] { typeof(object) }, Module, true);
        }
        private static DynamicMethod CreateDynamicSetterMethod()
        {
            return new DynamicMethod(String.Empty, typeof(void), new[] { typeof(object), typeof(object) }, Module, true);
        }
        private static DynamicMethod CreateDynamicMeth()
        {
            return new DynamicMethod(String.Empty, typeof(object), new[] { typeof(object), typeof(object[]) }, Module, true);
        }
        private static DynamicMethod CreateDynamicProc()
        {
            return new DynamicMethod(String.Empty, typeof(void), new[] { typeof(object), typeof(object[]) }, Module, true);
        }
        private static DynamicMethod CreateDynamicFactoryMethod()
        {
            return new DynamicMethod(String.Empty, typeof(object), new[] { typeof(object[]) }, Module, true);
        }
    }
}
