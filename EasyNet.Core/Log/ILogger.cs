using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyNet.Extensions;

namespace EasyNet.Log
{
    /// <summary>
    /// Represents a type used to perform logging.
    /// Aggregates most logging patterns to a single method.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a System.String message of the state and exception.</param>
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        /// <summary>
        /// Checks if the given logLevel is enabled.
        /// </summary>
        /// <param name="logLevel">Level to be checked.</param>
        /// <returns>true if enabled.</returns>
        bool IsEnabled(LogLevel logLevel);

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An System.IDisposable that ends the logical operation scope on dispose.</returns>
        IDisposable BeginScope<TState>(TState state)
#if NETCOREAPP3_1_OR_GREATER
            where TState : notnull
#endif
            ;
    }
    /// <summary>
    /// Defines logging severity levels.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Logs that contain the most detailed messages. These messages may contain sensitive application data.
        /// These messages are disabled by default and should never be enabled in a production environment.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Logs that are used for interactive investigation during development.  These logs should primarily contain
        /// information useful for debugging and have no long-term value.
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Logs that track the general flow of the application. These logs should have long-term value.
        /// </summary>
        Information = 2,

        /// <summary>
        /// Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the
        /// application execution to stop.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Logs that highlight when the current flow of execution is stopped due to a failure. These should indicate a
        /// failure in the current activity, not an application-wide failure.
        /// </summary>
        Error = 4,

        /// <summary>
        /// Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires
        /// immediate attention.
        /// </summary>
        Critical = 5,

        /// <summary>
        /// Not used for writing log messages. Specifies that a logging category should not write any messages.
        /// </summary>
        None = 6,
    }
    /// <summary>
    /// Identifies a logging event. The primary identifier is the "Id" property, with the "Name" property providing a short description of this type of event.
    /// </summary>
#if NET8_0_OR_GREATER
    public readonly struct EventId(int id, string name = null) : IEquatable<EventId>
    {
        /// <summary>
        /// Gets the numeric identifier for this event.
        /// </summary>
        public int Id => id;
        /// <summary>
        /// Gets the name of this event.
        /// </summary>
        public string Name => name;
#else
    public readonly struct EventId : IEquatable<EventId>
    {
        /// <summary>
        /// Gets the numeric identifier for this event.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Gets the name of this event.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="EventId"/> struct.
        /// </summary>
        /// <param name="id">The numeric identifier for this event.</param>
        /// <param name="name">The name of this event.</param>
        public EventId(int id, string name = null)
        {
            Id = id;
            Name = name;
        }
#endif
        /// <summary>
        /// Implicitly creates an EventId from the given <see cref="int"/>.
        /// </summary>
        /// <param name="i">The <see cref="int"/> to convert to an EventId.</param>
        public static implicit operator EventId(int i)
        {
            return new EventId(i);
        }

        /// <summary>
        /// Checks if two specified <see cref="EventId"/> instances have the same value. They are equal if they have the same Id.
        /// </summary>
        /// <param name="left">The first <see cref="EventId"/>.</param>
        /// <param name="right">The second <see cref="EventId"/>.</param>
        /// <returns><see langword="true" /> if the objects are equal.</returns>
        public static bool operator ==(EventId left, EventId right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Checks if two specified <see cref="EventId"/> instances have different values.
        /// </summary>
        /// <param name="left">The first <see cref="EventId"/>.</param>
        /// <param name="right">The second <see cref="EventId"/>.</param>
        /// <returns><see langword="true" /> if the objects are not equal.</returns>
        public static bool operator !=(EventId left, EventId right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name ?? Id.ToString();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type. Two events are equal if they have the same id.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(EventId other)
        {
            return Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is EventId eventId && Equals(eventId);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id;
        }
    }
    /// <summary>
    /// An empty scope without any logic
    /// </summary>
    internal sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope()
        {
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }

    /// <summary>
    /// 日志配置选项
    /// </summary>
    public class LoggerOptions
    {
        /// <summary>
        /// 消息前带日志级别
        /// </summary>
        public bool WithLevel { get; set; } = true;
        /// <summary>
        /// 消息前带时间
        /// </summary>
        public bool UseTime { get; set; } = true;
        /// <summary>
        /// 时间格式
        /// </summary>
        public string TimeFormat { get; set; } = "HH:mm:ss.fff";
        /// <summary>
        /// 日志输出最小级别
        /// </summary>
        public LogLevel MinimumLevel { get; set; } = LogLevel.Information;
        /// <summary>
        /// 是否对日志级别进行填充，使日志级别长度都一致
        /// </summary>
        public bool IsPaddingLevel { get; set; } = true;
        /// <summary>
        /// 日志级别是否填充左边，还是右边
        /// </summary>
        public bool PadLeft { get; set; } = true;

        /// <summary>
        /// 日志配置选项默认值
        /// </summary>
        public static LoggerOptions Default { get; } = new LoggerOptions();
    }

    /// <summary>
    /// logger base.
    /// </summary>

#if NET8_0_OR_GREATER
    public abstract partial class LoggerBase(string name) : ILogger
    {
        /// <summary>
        /// The name of the logger.
        /// </summary>
        protected string Name => name;
#else
    public abstract partial class LoggerBase : ILogger
    {
        /// <summary>
        /// The name of the logger.
        /// </summary>
        protected readonly string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerBase"/> class.
        /// </summary>
        /// <param name="name">The name of the logger.</param>
        public LoggerBase(string name)
        {
            Name = name;
        }
#endif


        /// <inheritdoc />
        public virtual IDisposable BeginScope<TState>(TState state)
#if NETCOREAPP3_1_OR_GREATER
            where TState : notnull
#endif
        {
            return NullScope.Instance;
        }

        /// <inheritdoc />
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            // Everything is enabled 
            return logLevel != LogLevel.None;
        }

        /// <inheritdoc />
        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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

            // 拼接日志级别和消息
            var logBuilder = new StringBuilder();
            if (LoggerOptions.Default.UseTime)
            {
                logBuilder.Append($"{DateTime.Now.ToString(LoggerOptions.Default.TimeFormat)} ");
            }
            if (LoggerOptions.Default.WithLevel)
            {
                logBuilder.Append($"{this.GetLevelString(logLevel)}: ");
            }

            logBuilder.Append(message);
            message = logBuilder.ToString();

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception;
            }

            this.WriteLine(logLevel, message);
        }

        private readonly int MaxLevelLength = LogLevel.Information.ToString().Length;
        /// <summary>
        /// 处理日志级别的格式化
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        private string GetLevelString(LogLevel logLevel)
        {
            var levelString = logLevel.ToString();
            if (LoggerOptions.Default.IsPaddingLevel)
            {
                if (LoggerOptions.Default.PadLeft)
                {
                    levelString = levelString.PadLeft(MaxLevelLength);
                }
                else
                {
                    levelString = levelString.PadRight(MaxLevelLength);
                }
            }
            return levelString;
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logLevel"></param>
        /// <param name="message"></param>
        protected abstract void WriteLine(LogLevel logLevel, string message);
    }
}
