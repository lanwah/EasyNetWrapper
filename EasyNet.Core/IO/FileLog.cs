using EasyNet.Extension;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.IO
// 文件名称：Log
// 创 建 者：lanwah
// 创建日期：2022/8/8 13:47:55
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //


namespace EasyNet.Core.IO
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    public class FileLog : IFileLog, IDisposable
    {
        // 日志等级：Trace < Debug < Info < Warn < Error < Fatal

        /// <summary>
        /// 默认日志名称
        /// </summary>
        public const string DEFAULT_NAME = "AppLog";
        /// <summary>
        /// 日志配置文件名称
        /// </summary>
        public const string DEFAULT_NLOG_CONFIG_NAME = "NLog.config";
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
        /// <inheritdoc/>
        public string Name
        {
            get;
            private set;
        }
        /// <inheritdoc/>
        public string ConfigFileName
        {
            get;
            private set;
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
        /// 日志类
        /// </summary>
        protected Logger NLog
        {
            get;
            private set;
        }



        /// <summary>
        /// 构造函数，使用前请调用 <see cref="Initial"/> 进行初始化
        /// </summary>
        /// <param name="appDir">设置日志文件根目录</param>
        /// <param name="name">日志文件名称</param>
        public FileLog(string appDir, string name)
        {
            this.SetAppDir(appDir);
            this.Name = name;
        }
        /// <summary>
        /// 构造函数，使用前请调用 <see cref="Initial"/> 进行初始化
        /// </summary>
        public FileLog() : this(null, DEFAULT_NAME)
        {
            
        }



        /// <summary>
        /// 初始化函数
        /// </summary>
        public void Initial()
        {
            if (null == this.NLog)
            {
                this.InitialDefault();

                LogManager.LoadConfiguration(Path.Combine(this.AppDir, this.ConfigFileName));
                this.NLog = LogManager.GetLogger(this.Name);
            }
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
        /// 设置日志配置文件名称
        /// </summary>
        /// <param name="configFileName"></param>
        public void SetConfigFileName(string configFileName)
        {
            this.ConfigFileName = configFileName;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (null != this.NLog)
            {
                this.NLog = null;
            }
        }
        /// <inheritdoc/>
        public void Trace(string message)
        {
            this.NLog.Trace(this.Format(message, true));
        }
        /// <inheritdoc/>
        public void Debug(string message)
        {
            this.NLog.Debug(this.Format(message, true));
        }
        /// <inheritdoc/>
        public void Info(string message)
        {
            this.NLog.Info(this.Format(message, true));
        }
        /// <inheritdoc/>
        public void Warn(string message)
        {
            this.NLog.Warn(this.Format(message, true));
        }
        /// <inheritdoc/>
        public void Error(string message)
        {
            this.NLog.Error(this.Format(message, false));
        }
        /// <inheritdoc/>
        public void Fatal(string message)
        {
            this.NLog.Fatal(this.Format(message, false));
        }



        /// <summary>
        /// 初始化默认参数
        /// </summary>
        private void InitialDefault()
        {
            if (this.Name.IsNullOrEmptyEx())
            {
                this.Name = DEFAULT_NAME;
            }
            if (this.ConfigFileName.IsNullOrEmptyEx())
            {
                this.SetConfigFileName(DEFAULT_NLOG_CONFIG_NAME);
            }
            if (this.AppDir.IsNullOrEmptyEx())
            {
                this.SetAppDir(AppDomain.CurrentDomain.BaseDirectory);
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
    }

    /// <summary>
    /// 文件日志类
    /// </summary>
    public interface IFileLog : ILogService
    {
        /// <summary>
        /// 应用程序目录
        /// </summary>
        string AppDir
        {
            get;
        }
        /// <summary>
        /// 日志记录器名称
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// 日志配置文件名称
        /// </summary>
        string ConfigFileName
        {
            get;
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
