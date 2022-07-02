using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// object 扩展方法
    /// </summary>
    public static class ObjectExtn
    {
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
        /// 判断对象是否非空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }

        /// <summary>
        /// 检查参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        public static void NotNullCheck(this object argumentValue, string argumentName)
        {
            NotNull(argumentValue, argumentName);
        }
        /// <summary>
        /// 检查参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        /// <see cref="nameof"/>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        internal static void NotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
