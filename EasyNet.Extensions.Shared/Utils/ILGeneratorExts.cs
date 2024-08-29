#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using EasyNet.Core;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.Utils
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：ILGeneratorExts.cs
// 创建用户：lanwah
// 创建日期：2024/8/28 16:37:25
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
    /// ILGenerator 扩展方法
    /// </summary>
    public static class ILGeneratorExts
    {
        /// <summary>
        /// 拆箱或转换操作
        /// </summary>
        /// <param name="this"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILGenerator EmitUnboxOrCast(this ILGenerator @this, Type type)
        {
            if (type.IsValueType)
            {
                @this.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                @this.Emit(OpCodes.Castclass, type);
            }

            return @this;
        }
        /// <summary>
        /// 加载数字
        /// </summary>
        /// <param name="this"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ILGenerator EmitLoadInt(this ILGenerator @this, int value)
        {
            switch (value)
            {
                case -1:
                    @this.Emit(OpCodes.Ldc_I4_M1);
                    break;
                case 0:
                    @this.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    @this.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    @this.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    @this.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    @this.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    @this.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    @this.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    @this.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    @this.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (value > -129 && value < 128)
                    {
                        @this.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        @this.Emit(OpCodes.Ldc_I4, value);
                    }

                    break;
            }

            return @this;
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="this"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static ILGenerator EmitLoadArgument(this ILGenerator @this, int index)
        {
            switch (index)
            {
                case 0:
                    @this.Emit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    @this.Emit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    @this.Emit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    @this.Emit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index > -129 && index < 128)
                    {
                        @this.Emit(OpCodes.Ldarg_S, (sbyte)index);
                    }
                    else
                    {
                        @this.Emit(OpCodes.Ldarg, index);
                    }

                    break;
            }

            return @this;
        }
        /// <summary>
        /// 加载参数
        /// </summary>
        /// <param name="this"></param>
        /// <param name="info"></param>
        /// <param name="argumentArrayIndex"></param>
        public static void EmitLoadParameters(this ILGenerator @this, MethodMetaData info, int argumentArrayIndex)
        {
            if (!info.Method.IsStatic && !(info.Method is ConstructorInfo))
            {
                @this.Emit(OpCodes.Ldarg_0);
                @this.EmitUnboxOrCast(info.Method.DeclaringType);
            }

            for (int index = 0; index < info.Parameters.Length; index++)
            {
                @this.EmitLoadArgument(argumentArrayIndex);
                @this.EmitLoadInt(index);
                @this.Emit(OpCodes.Ldelem_Ref);
                @this.EmitUnboxOrCast(info.ParameterTypes[index]);
            }
        }
    }
}
#endif
