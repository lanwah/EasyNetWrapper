using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Log
{
#if NET8_0_OR_GREATER
    /// <summary>
    /// 日志记录器
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <param name="loggers">日志记录器</param>
    public class Logger(string categoryName, List<ILogger> loggers) : ILogger
    {
        /// <summary>
        /// The category name for messages produced by the logger.
        /// </summary>
        public string CategoryName => categoryName;
        /// <summary>
        /// 日志记录器
        /// </summary>
        public List<ILogger> Loggers => loggers;

#else
    /// <summary>
    /// 日志记录器
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// The category name for messages produced by the logger.
        /// </summary>
        public string CategoryName
        {
            get;
            private set;
        }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public List<ILogger> Loggers
        {
            get;
            private set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <param name="loggers">日志记录器</param>
        public Logger(string categoryName, List<ILogger> loggers)
        {
            this.CategoryName = categoryName;
            this.Loggers = loggers;
        }
#endif


        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
#if NETCOREAPP3_1_OR_GREATER
            where TState : notnull
#endif
        {
            return NullScope.Instance;
        }
        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            // Everything is enabled unless the debugger is not attached
            return logLevel != LogLevel.None;
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
