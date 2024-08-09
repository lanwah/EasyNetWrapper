using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Log
{
    /// <summary>
    /// 日志工厂
    /// </summary>
    public class LoggerFactory
    {
        private readonly ILoggingBuilder _loggingBuilder = new DefaultLoggingBuilder();

        /// <summary>
        /// 创建日志记录器工厂
        /// </summary>
        /// <returns></returns>
        public static LoggerFactory Create(Action<ILoggingBuilder> builder)
        {
            var instance = new LoggerFactory();
            builder.Invoke(instance._loggingBuilder);
            return instance;
        }


        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            var loggers = this._loggingBuilder.LoggerProviders.Select(p => p.CreateLogger(categoryName)).ToList();
            return new Logger(categoryName, loggers);
        }
    }

    /// <summary>
    /// 日志记录器接口
    /// </summary>
    public interface ILoggingBuilder
    {
        /// <summary>
        /// 日志记录器提供者
        /// </summary>
        List<ILoggerProvider> LoggerProviders
        {
            get;
        }
    }

    /// <summary>
    /// 默认的日志记录器构建器
    /// </summary>
    public class DefaultLoggingBuilder : ILoggingBuilder
    {
        /// <inheritdoc />
#if NET8_0_OR_GREATER
        public List<ILoggerProvider> LoggerProviders { get; } = [];
#else
        public List<ILoggerProvider> LoggerProviders { get; } = new List<ILoggerProvider>();
#endif
    }

    /// <summary>
    /// 日志记录器扩展
    /// </summary>
    public static class LoggingBuilderExts
    {
        /// <summary>
        /// 添加控制台日志记录器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddConsole(this ILoggingBuilder builder)
        {
            builder.LoggerProviders.Add(new ConsoleLoggerProvider());
            return builder;
        }
        /// <summary>
        /// 添加文件日志记录器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddSimpleFile(this ILoggingBuilder builder)
        {
            builder.LoggerProviders.Add(new SimpleFileLoggerProvider());
            return builder;
        }
        /// <summary>
        /// 添加调试日志记录器
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddDebug(this ILoggingBuilder builder)
        {
            builder.LoggerProviders.Add(new DebugLoggerProvider());
            return builder;
        }
        /// <summary>
        /// 设置日志记录器最小级别
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static ILoggingBuilder SetMinimumLevel(this ILoggingBuilder builder, LogLevel level)
        {
            LoggerOptions.Default.MinimumLevel = level;
            return builder;
        }
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// 日志记录器
    /// </summary>
    /// <param name="categoryName">The category name for messages produced by the logger.</param>
    /// <param name="loggers">日志记录器</param>
    internal class Logger(string categoryName, List<ILogger> loggers) : LoggerBase(categoryName)
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public List<ILogger> Loggers => loggers;

#else
    /// <summary>
    /// 日志记录器
    /// </summary>
    internal class Logger : LoggerBase
    {
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
        public Logger(string categoryName, List<ILogger> loggers) : base(categoryName)
        {
            this.Loggers = loggers;
        }
#endif

        /// <inheritdoc />
        public override bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel >= LoggerOptions.Default.MinimumLevel)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc />
        protected override void WriteLine(LogLevel logLevel, string message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (this.Loggers.HasNoData())
            {
                return;
            }

            foreach (var logger in this.Loggers)
            {
                logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}
