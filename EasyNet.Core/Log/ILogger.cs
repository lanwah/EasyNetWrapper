using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
