using EasyNet.Extensions;
using EasyNet.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Service
// CLR版本：4.0.30319.42000
// 运行要求：3.5
// 文件名称：SimpleServiceProvider.cs
// 创建用户：lanwah
// 创建日期：2024/8/30 13:36:15
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
    /// <summary>
    /// 服务提供程序
    /// </summary>
    public class SimpleService
    {
        /// <summary>
        /// 日志服务
        /// </summary>
        public static ILogger Logger
        {
            get => GetService<ILogger>();
        }

        private static ServiceContainer _container;
        /// <summary>
        /// 默认服务容器
        /// </summary>
        internal static ServiceContainer Container
        {
            get
            {
                if (null == _container)
                {
                    _container = new ServiceContainer();
                }

                return _container;
            }
        }
        /// <summary>
        /// 获取服务容器
        /// </summary>
        internal static IServiceContainer Service
        {
            get => Container;
        }
        /// <summary>
        /// 获取服务提供程序
        /// </summary>
        internal static IServiceProvider Provider
        {
            get => Container;
        }

        static SimpleService()
        {
            // DefaultServices 包含 IServiceContainer 服务
            InitializeDefaultService();
        }
        private SimpleService()
        {

        }

        private static void InitializeDefaultService()
        {
            var container = Container;
            var provider = Container;

            // 添加服务容器
            Container.AddService(typeof(IServiceContainer), container);
            // 添加服务提供程序
            Container.AddService(typeof(IServiceProvider), provider);
            // 添加日志服务
            var log = LoggerFactory.Default;
            if (log.IsNotNull())
            {
                Container.AddService(typeof(ILogger), log);
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ServiceNotFoundException"></exception>
        public static T GetService<T>()
        {
            var service = Provider.GetService(typeof(T)) ?? throw new ServiceNotFoundException(typeof(T));
            return (T)service;
        }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public static void AddService(Type serviceType, object serviceInstance)
        {
            Service.AddService(serviceType, serviceInstance);
        }
    }
}
