using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core
// 文件名称：ArgsChecker
// 创 建 者：lanwah
// 创建日期：2021/02/22 15:21:57
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core
{
    /// <summary>
    /// 参数检测，不符合检测结果时抛出异常
    /// </summary>
    public static partial class ArgChecker
    {
        /// <summary>
        /// 判断参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        public static void NotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
        /// <summary>
        /// 判断参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        public static void NotNullOrEmpty(string argumentValue, string argumentName)
        {
            if ((argumentValue == null) || (argumentValue.Length == 0) || (argumentValue.Trim().Length == 0))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 判断参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        public static void NotNullChecker(this object argumentValue, string argumentName)
        {
            NotNull(argumentValue, argumentName);
        }
        /// <summary>
        /// 判断参数是否非空，空时抛出ArgumentNullException
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名称，可以通过nameof(argumentValue)进行使用</param>
        public static void NotNullOrEmptyChecker(this string argumentValue, string argumentName)
        {
            NotNullOrEmpty(argumentValue, argumentName);
        }
    }
}
