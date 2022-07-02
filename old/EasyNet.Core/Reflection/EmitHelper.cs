using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：EmitHelper
// 创 建 者：lanwah
// 创建日期：2021/07/01 11:18:17
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
    internal static class EmitHelper
    {
        /// <summary>
        /// 拆箱或转换操作
        /// </summary>
        /// <param name="il"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILGenerator UnboxOrCast(this ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }

            return il;
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="il"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ILGenerator LoadArgument(this ILGenerator il, int index)
        {
            switch (index)
            {
                case 0:
                    il.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index > -129 && index < 128)
                    {
                        il.Emit(OpCodes.Ldarg_S, (sbyte)index);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldarg, index);
                    }

                    break;
            }

            return il;
        }
        /// <summary>
        /// 加载数字
        /// </summary>
        /// <param name="il"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ILGenerator LoadInt(this ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (value > -129 && value < 128)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }

                    break;
            }

            return il;
        }

        public static void EmitDynamicMethod(MethodInfo method, DynamicMethod callable)
        {
            var info = new MethodMetaData(method);

            ILGenerator il = callable.GetILGenerator();

            il.EmitLoadParameters(info, 1);

            if (method.IsStatic)
            {
                il.EmitCall(OpCodes.Call, method, null);
            }
            else if (method.IsVirtual)
            {
                il.EmitCall(OpCodes.Callvirt, method, null);
            }
            else
            {
                il.EmitCall(OpCodes.Call, method, null);
            }

            if (method.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Ldnull);
            }
            else
            {
                if (method.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Box, method.ReturnType);
                }
            }
            il.Emit(OpCodes.Ret);
        }
        public static void EmitLoadParameters(this ILGenerator il, MethodMetaData info, int argumentArrayIndex)
        {
            if (!info.Method.IsStatic && !(info.Method is ConstructorInfo))
            {
                il.Emit(OpCodes.Ldarg_0);
                il.UnboxOrCast(info.Method.DeclaringType);
            }

            for (int index = 0; index < info.Parameters.Length; index++)
            {
                il.LoadArgument(argumentArrayIndex);
                il.LoadInt(index);
                il.Emit(OpCodes.Ldelem_Ref);
                il.UnboxOrCast(info.ParameterTypes[index]);
            }
        }
    }
}
