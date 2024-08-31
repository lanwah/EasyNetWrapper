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
    internal sealed partial class ConsoleLogger(string name) : LoggerBase(name)
    {
#else
    internal sealed partial class ConsoleLogger : LoggerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public ConsoleLogger(string name) : base(name)
        {
        }
#endif
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">日志配置项</param>
        /// <param name="name"></param>
        public ConsoleLogger(LoggerOptions option, string name) : this(name)
        {
            this.Options = option;
        }
        protected override void WriteLine(LogLevel logLevel, string message)
        {
            Console.WriteLine(message);
        }
    }
}
