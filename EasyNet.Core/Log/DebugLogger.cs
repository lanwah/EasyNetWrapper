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
    internal sealed partial class DebugLogger(string name) : LoggerBase(name)
    {
#else
    internal sealed partial class DebugLogger : LoggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public DebugLogger(string name) : base(name)
        {
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">日志配置项</param>
        /// <param name="name"></param>
        public DebugLogger(LoggerOptions option, string name) : this(name)
        {
            this.Options = option;
        }

        /// <inheritdoc />
        public override bool IsEnabled(LogLevel logLevel)
        {
            // Everything is enabled unless the debugger is not attached
            return Debugger.IsAttached && logLevel != LogLevel.None;
        }

        protected override void WriteLine(LogLevel logLevel, string message)
        {
            Debug.WriteLine(message);
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
