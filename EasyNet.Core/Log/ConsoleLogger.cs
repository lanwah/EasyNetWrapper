using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyNet.Extensions;

namespace EasyNet.Log
{
    /// <summary>
    /// The provider for the <see cref="ConsoleLogger"/>.
    /// </summary>
    [ProviderAlias("Console")]
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new ConsoleLogger(name);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    /// <summary>
    /// A logger that writes messages in the console output window.
    /// </summary>
#if NET8_0_OR_GREATER
    internal sealed partial class ConsoleLogger(string name) : ILogger
#else
    internal sealed partial class ConsoleLogger : ILogger
#endif
    {

#if !NET8_0_OR_GREATER
        private readonly string name;
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public ConsoleLogger(string name)
        {
            this.name = name;
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
            return logLevel != LogLevel.None;
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            formatter.ThrowIfNull(nameof(formatter));

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{logLevel}: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception;
            }

            DebugWriteLine(message, name);
        }
        private static void DebugWriteLine(string message, string name)
        {
            Console.WriteLine(message, name);
        }
    }
}
