using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core
// 文件名称：DefaultLog
// 创 建 者：lanwah
// 创建日期：2020/11/18 19:35:10
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
    /// 
    /// </summary>
    public static class LogProvider
    {
        private static ILog _log = null;

        static LogProvider()
        {
            InitializeDefaultLog();
        }

        /// <summary>
        /// 获取日志类
        /// </summary>
        public static ILog Log
        {
            get
            {
                return _log;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _log = value;
            }
        }

        /// <summary>
        /// 创建默认的服务提供程序
        /// </summary>
        private static void InitializeDefaultLog()
        {
            if (null == _log)
            {
                string log4netConfFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs\\log4net.config");
                XmlConfigurator.ConfigureAndWatch(new FileInfo(log4netConfFile));
#if DEBUG
                _log = LogManager.GetLogger("DebugLoggingService");
#else
                _log = LogManager.GetLogger("ReleaseLoggingService");
#endif
            }
        }
        /// <summary>
        /// 设置DEBUG Log
        /// </summary>
        [Conditional("DEBUG")]
        private static void SetDebugLog()
        {
            _log = LogManager.GetLogger("DebugLoggingService");
        }
    }
}
