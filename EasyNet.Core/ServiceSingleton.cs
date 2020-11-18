using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core
// 文件名称：ServiceSingleton
// 创 建 者：lanwah
// 创建日期：2020/11/18 17:26:35
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
    /// 单例服务提供程序
    /// </summary>
    public static class ServiceSingleton
    {
        private static readonly IServiceProvider _serviceContainer = null;
        private static IServiceProvider _serviceProvider = null;

        static ServiceSingleton()
        {
            _serviceContainer = new System.ComponentModel.Design.ServiceContainer();

            InitializeDefaultService();
        }

        /// <summary>
        /// 初始化默认服务
        /// </summary>
        private static void InitializeDefaultService()
        {
            _serviceProvider = _serviceContainer;

            AddService(typeof(ILog), LogProvider.Log);
            Log.Debug("ILog 服务已添加.");
            AddService(typeof(IServiceProvider), _serviceContainer);
            Log.Debug("IServiceProvider 服务已添加.");
            AddService(typeof(IServiceContainer), _serviceContainer);
            Log.Debug("IServiceContainer 服务已添加.");
        }

        /// <summary>
        /// 获取或设置服务管理提供程序
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _serviceProvider = value;
            }
        }
        /// <summary>
        /// Retrieves the service of type <typeparamref name="T"/> from the provider.
        /// If the service cannot be found, a <see cref="ServiceNotFoundException"/> will be thrown.
        /// </summary>
        public static T GetRequiredService<T>()
        {
            var service = _serviceProvider.GetService(typeof(T));
            if (service == null)
            {
                throw new ServiceNotFoundException(typeof(T));
            }

            return (T)service;
        }

        /// <summary>
        /// 获取服务容器管理器
        /// </summary>
        public static IServiceContainer ServiceContainer
        {
            get
            {
                return GetRequiredService<IServiceContainer>();
            }
        }
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        public static void AddService(Type serviceType, object serviceInstance)
        {
            ServiceContainer.AddService(serviceType, serviceInstance);
        }

        /// <summary>
        /// 获取服务中的日志提供程序
        /// </summary>
        public static ILog Log
        {
            get
            {
                return GetRequiredService<ILog>();
            }
        }
    }

    /// <summary>
    /// 服务未找到异常
    /// </summary>
    [Serializable()]
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException() : base()
        {
        }

        public ServiceNotFoundException(Type serviceType) : base("Required service not found: " + serviceType.FullName)
        {
        }

        public ServiceNotFoundException(string message) : base(message)
        {
        }

        public ServiceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ServiceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
