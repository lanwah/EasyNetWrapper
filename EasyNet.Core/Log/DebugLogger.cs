using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EasyNet.Log
{
    /// <summary>
    /// The provider for the <see cref="DebugLogger"/>.
    /// </summary>
    [ProviderAlias("Debug")]
    public class DebugLoggerProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new DebugLogger(name);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    /// <summary>
    /// A logger that writes messages in the debug output window only when a debugger is attached.
    /// </summary>

#if NET8_0_OR_GREATER
    internal sealed partial class DebugLogger(string name) : ILogger
    {
        private string Name => name;
#else
    internal sealed partial class DebugLogger : ILogger
    {
        private readonly string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public DebugLogger(string name)
        {
            Name = name;
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
            return Debugger.IsAttached && logLevel != LogLevel.None;
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

            DebugWriteLine(message, Name);
        }
        private static void DebugWriteLine(string message, string name)
        {
            Debug.WriteLine(message, name);
        }
    }

    /// <summary>
    /// Defines alias for <see cref="ILoggerProvider"/> implementation to be used in filtering rules.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
#if NET8_0_OR_GREATER
    public class ProviderAliasAttribute(string alias) : Attribute
    {
        /// <summary>
        /// The alias of the provider.
        /// </summary>
        public string Alias => alias;
#else
    public class ProviderAliasAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="ProviderAliasAttribute"/> instance.
        /// </summary>
        /// <param name="alias">The alias to set.</param>
        public ProviderAliasAttribute(string alias)
        {
            Alias = alias;
        }

        /// <summary>
        /// The alias of the provider.
        /// </summary>
        public string Alias { get; }
#endif
    }
    /// <summary>
    /// Represents a type that can create instances of <see cref="ILogger"/>.
    /// </summary>
    public interface ILoggerProvider : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The instance of <see cref="ILogger"/> that was created.</returns>
        ILogger CreateLogger(string categoryName);
    }
}
