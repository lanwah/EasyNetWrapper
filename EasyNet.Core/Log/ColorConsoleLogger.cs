using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Log
{
    /// <summary>
    /// The provider for the <see cref="ColorConsoleLogger"/>.
    /// </summary>
    [ProviderAlias("ColorConsole")]
    public class ColorConsoleLoggerProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new ColorConsoleLogger(name);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// ColorConsoleLogger extension methods.
    /// </summary>
    public static class ColorConsoleLoggerExts
    {
        /// <summary>
        /// Adds the <see cref="ColorConsoleLoggerProvider"/> to the logging builder.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static ILoggingBuilder AddColorConsole(this ILoggingBuilder builder)
        {
            builder.LoggerProviders.Add(new ColorConsoleLoggerProvider());
            return builder;
        }
    }

#if NET8_0_OR_GREATER
    internal sealed partial class ColorConsoleLogger(string name) : LoggerBase(name)
    {
#else
    internal sealed partial class ColorConsoleLogger : LoggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public ColorConsoleLogger(string name) : base(name)
        {
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">日志配置项</param>
        /// <param name="name"></param>
        public ColorConsoleLogger(LoggerOptions option, string name) : this(name)
        {
            this.Options = option;
        }
#if NETCOREAPP3_1_OR_GREATER
        private readonly Dictionary<LogLevel, ConsoleColor> LogLevelColor = new() {
            {LogLevel.Critical,ConsoleColor.DarkRed },
            {LogLevel.Error,ConsoleColor.Red },
            {LogLevel.Warning,ConsoleColor.DarkYellow },
            {LogLevel.Information,ConsoleColor.White },
            {LogLevel.Debug,ConsoleColor.DarkGray },
        };
#else
        private readonly Dictionary<LogLevel, ConsoleColor> LogLevelColor = new Dictionary<LogLevel, ConsoleColor>() {
            {LogLevel.Critical,ConsoleColor.DarkRed },
            {LogLevel.Error,ConsoleColor.Red },
            {LogLevel.Warning,ConsoleColor.DarkYellow },
            {LogLevel.Information,ConsoleColor.White },
            {LogLevel.Debug,ConsoleColor.DarkGray },
        };
#endif
        /// <summary>
        /// Sets the color of the log level.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="color"></param>
        public void SetLogLevelColor(LogLevel logLevel, ConsoleColor color)
        {
            this.LogLevelColor[logLevel] = color;
        }

        /// <inheritdoc />
        protected override void WriteLine(LogLevel logLevel, string message)
        {
            // Save the current color
            var color = Console.ForegroundColor;

            try
            {
                if (this.LogLevelColor.TryGetValue(logLevel, out ConsoleColor foreColor))
                {
                    Console.ForegroundColor = foreColor;
                }

                Console.WriteLine(message);
            }
            finally
            {
                // Restore the color
                Console.ForegroundColor = color;
            }
        }
    }
}
