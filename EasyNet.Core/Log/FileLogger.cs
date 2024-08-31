using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using EasyNet.Extensions;

namespace EasyNet.Log
{
    /// <summary>
    /// The provider for the <see cref="FileLogger"/>.
    /// </summary>
    [ProviderAlias("File")]
    public class FileLoggerProvider : ILoggerProvider
    {
        /// <inheritdoc />
        public ILogger CreateLogger(string name)
        {
            return new FileLogger(name);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 日志操作类
    /// </summary>
    internal class FileLogger : LoggerBase
    {
        // 日志等级：Trace < Debug < Information < Warn < Error < Critical < None

        /// <summary>
        /// 默认日志名称
        /// </summary>
        public const string DEFAULT_NAME = "Log";
        /// <summary>
        /// 正确标记；样式： .................... √ 
        /// </summary>
        public const string TrueFlag = " .................... √ ";
        /// <summary>
        /// 正确标记；样式：『√』
        /// </summary>
        public const string True = "『√』 ";
        /// <summary>
        /// 失败标记；样式： .................... ×
        /// </summary>
        public const string FalseFlag = " .................... × ";
        /// <summary>
        /// 失败标记；样式：『×』
        /// </summary>
        public const string False = "『×』 ";
        /// <inheritdoc/>
        public string AppDir
        {
            get;
            private set;
        }
        /// <summary>
        /// 日志目录
        /// </summary>
        public string LogDir
        {
            get => Path.Combine(this.AppDir, "Logs");
        }
        private MessageFlag markFlag = MessageFlag.None;
        /// <summary>
        /// 正确/错误消息标志
        /// </summary>
        public MessageFlag MarkFlag
        {
            get => this.markFlag;
            set => this.markFlag = value;
        }



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option">日志配置项</param>
        /// <param name="name"></param>
        public FileLogger(LoggerOptions option, string name) : this(name)
        {
            this.Options = option;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appDir">设置日志文件根目录</param>
        /// <param name="name">日志文件名称</param>
        public FileLogger(string appDir, string name) : base(name)
        {
            this.SetAppDir(appDir);
            this.Initial();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public FileLogger(string name) : this(string.Empty, name)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileLogger() : this(DEFAULT_NAME)
        {

        }



        /// <summary>
        /// 初始化函数
        /// </summary>
        public void Initial()
        {
            this.InitialDefault();
        }
        /// <summary>
        /// 设置应用程序目录
        /// </summary>
        /// <param name="appDir"></param>
        public void SetAppDir(string appDir)
        {
            this.AppDir = appDir;
        }
        /// <summary>
        /// 初始化默认参数
        /// </summary>
        private void InitialDefault()
        {
            if (this.AppDir.IsNullOrEmpty())
            {
                this.SetAppDir(AppDomain.CurrentDomain.BaseDirectory);
            }

            // 创建日志目录
            if (!Directory.Exists(this.LogDir))
            {
                Directory.CreateDirectory(this.LogDir);
            }
        }
        /// <summary>
        /// 格式化日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="isTrue">日志标识</param>
        /// <returns></returns>
        private string Format(string message, bool isTrue)
        {
            if (this.MarkFlag == MessageFlag.HeadFlag)
            {
                return $"{(isTrue ? True : False)}{message}";
            }
            else if (this.MarkFlag == MessageFlag.TailFlag)
            {
                return $"{message}{(isTrue ? TrueFlag : FalseFlag)}";
            }
            else
            {
                return message;
            }
        }

        protected override void WriteLine(LogLevel logLevel, string message)
        {
            bool? isTrue = null;
            if (logLevel < LogLevel.Warning)
            {
                isTrue = true;
            }
            else if (logLevel < LogLevel.Critical)
            {
                isTrue = false;
            }

            var msg = message;
            if (isTrue.HasValue)
            {
                msg = this.Format(message, isTrue.Value);
            }

            var logFile = Path.Combine(this.LogDir, this.Name + ".log");

#if NETCOREAPP3_1_OR_GREATER
            using var writer = new StreamWriter(logFile, true, Encoding.UTF8);
            writer.WriteLine(msg);
            writer.Flush();
            writer.Close();
#else
            using (var writer = new StreamWriter(logFile, true, Encoding.UTF8))
            {
                writer.WriteLine(msg);

                writer.Flush();
                writer.Close();
            }
#endif
        }
    }

    /// <summary>
    /// 消息标识
    /// </summary>
    public enum MessageFlag
    {
        /// <summary>
        /// 不启用
        /// </summary>
        None = 0,
        /// <summary>
        /// 头部标识
        /// </summary>
        HeadFlag = 1,
        /// <summary>
        /// 尾部标识
        /// </summary>
        TailFlag = 2
    }
}
