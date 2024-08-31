using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Service
// CLR版本：4.0.30319.42000
// 运行要求：3.5
// 文件名称：ServiceNotFoundException.cs
// 创建用户：lanwah
// 创建日期：2024/8/30 13:33:45
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Service
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// 服务未找到异常
    /// </summary>
    /// <param name="serviceType"></param>
    public class ServiceNotFoundException(Type serviceType) : Exception("Required service not found: " + serviceType.FullName)
    {
#else
    /// <summary>
    /// 服务未找到异常
    /// </summary>
    public class ServiceNotFoundException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceType"></param>
        public ServiceNotFoundException(Type serviceType) : base("Required service not found: " + serviceType.FullName)
        {
        }
#endif
    }
}
