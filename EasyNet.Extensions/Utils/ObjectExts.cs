using System;
using System.Collections.Generic;
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
        internal static void ThrowIfNullInternal(object @this, string argumentName, string message = "")
        {
            if (@this.IsNull())
            {
                throw new ArgumentNullException(argumentName, message);
            }
        }
    }
}
