using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：DynamicMethodFactory
// 创 建 者：lanwah
// 创建日期：2021/04/13 13:21:00
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
    /// <summary>
    /// 通过反射设置属性值
    /// </summary>
    /// <param name="target"></param>
    /// <param name="arg"></param>
    public delegate void SetValueDelegate(object target, object arg);
    /// <summary>
    /// 通过反射调用方法
    /// </summary>
    /// <param name="target"></param>
    /// <param name="paramters"></param>
    /// <returns></returns>
    public delegate object FastInvokeHandler(object target, object[] paramters);

    /// <summary>
    /// 动态方法工厂
    /// </summary>
    public static class DynamicMethodFactory
    {
        /// <summary>
        /// Emit方法优化反射设置属性值
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public static void SetValue(PropertyInfo propertyInfo, object target, object value)
        {
            var setter = DynamicMethodFactory.CreatePropertySetter(propertyInfo);
            setter(target, value);
        }
        /// <summary>
        /// Emit方法优化反射调用方法
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="target"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static object InvokeMethod(MethodInfo methodInfo, object target, params object[] paramters)
        {
            var method = DynamicMethodFactory.GetMethodInvoker(methodInfo);
            return method(target, paramters);
        }

        private static void EmitCastToReference(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }
        private static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;

            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);

            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }
        private static void EmitBoxIfNeeded(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
            }
        }

        private static SetValueDelegate CreatePropertySetter(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            if (!property.CanWrite)
            {
                return null;
            }

            MethodInfo setMethod = property.GetSetMethod(true);

            DynamicMethod dm = new DynamicMethod("PropertySetter", null,
                new Type[] { typeof(object), typeof(object) }, property.DeclaringType, true);

            ILGenerator il = dm.GetILGenerator();

            if (!setMethod.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            il.Emit(OpCodes.Ldarg_1);

            EmitCastToReference(il, property.PropertyType);
            if (!setMethod.IsStatic && !property.DeclaringType.IsValueType)
            {
                il.EmitCall(OpCodes.Callvirt, setMethod, null);
            }
            else
            {
                il.EmitCall(OpCodes.Call, setMethod, null);
            }

            il.Emit(OpCodes.Ret);

            return (SetValueDelegate)dm.CreateDelegate(typeof(SetValueDelegate));
        }
        private static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
        {
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty,
                typeof(object),
                new Type[] { typeof(object), typeof(object[]) },
                methodInfo.DeclaringType.Module);
            ILGenerator il = dynamicMethod.GetILGenerator();
            ParameterInfo[] ps = methodInfo.GetParameters();
            Type[] paramTypes = new Type[ps.Length];
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    paramTypes[i] = ps[i].ParameterType.GetElementType();
                }
                else
                {
                    paramTypes[i] = ps[i].ParameterType;
                }
            }
            LocalBuilder[] locals = new LocalBuilder[paramTypes.Length];

            for (int i = 0; i < paramTypes.Length; i++)
            {
                locals[i] = il.DeclareLocal(paramTypes[i], true);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_1);
                EmitFastInt(il, i);
                il.Emit(OpCodes.Ldelem_Ref);
                EmitCastToReference(il, paramTypes[i]);
                il.Emit(OpCodes.Stloc, locals[i]);
            }
            if (!methodInfo.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
            }
            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldloca_S, locals[i]);
                }
                else
                {
                    il.Emit(OpCodes.Ldloc, locals[i]);
                }
            }
            if (methodInfo.IsStatic)
            {
                il.EmitCall(OpCodes.Call, methodInfo, null);
            }
            else
            {
                il.EmitCall(OpCodes.Callvirt, methodInfo, null);
            }

            if (methodInfo.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                EmitBoxIfNeeded(il, methodInfo.ReturnType);
            }

            for (int i = 0; i < paramTypes.Length; i++)
            {
                if (ps[i].ParameterType.IsByRef)
                {
                    il.Emit(OpCodes.Ldarg_1);
                    EmitFastInt(il, i);
                    il.Emit(OpCodes.Ldloc, locals[i]);
                    if (locals[i].LocalType.IsValueType)
                    {
                        il.Emit(OpCodes.Box, locals[i].LocalType);
                    }

                    il.Emit(OpCodes.Stelem_Ref);
                }
            }

            il.Emit(OpCodes.Ret);
            FastInvokeHandler invoder = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
            return invoder;
        }
    }
}
