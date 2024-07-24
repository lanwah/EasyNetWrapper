using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Core
{
    /// <summary>
    /// 日志接口，日志等级，低 -> 高：Trace  Debug  Info  Warn  Error  Fatal
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// 记录跟踪日志，最常见的记录信息，一般用于普通输出
        /// </summary>
        /// <param name="message">日志内容</param>
        void Trace(string message);
        /// <summary>
        /// 记录调试日志，同样是记录信息，不过出现的频率要比Trace少一些，一般用来调试程序
        /// </summary>
        /// <param name="message">日志内容</param>
        void Debug(string message);
        /// <summary>
        /// 记录信息日志，信息类型的消息
        /// </summary>
        /// <param name="message">日志内容</param>
        void Info(string message);
        /// <summary>
        /// 记录警告日志，警告信息，一般用于比较重要的场合
        /// </summary>
        /// <param name="message">日志内容</param>
        void Warn(string message);
        /// <summary>
        /// 记录错误日志，错误信息
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);
        /// <summary>
        /// 记录致命错误日志，致命异常信息。一般来讲，发生致命异常之后程序将无法继续执行
        /// </summary>
        /// <param name="message">日志内容</param>
        void Fatal(string message);
    }
}
