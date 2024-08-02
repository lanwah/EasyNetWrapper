using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Extensions.Shared.Const
{
    /// <summary>
    /// 字符常量定义
    /// </summary>
    public partial class ConstVar
    {
        /// <summary>
        /// Linux/Unix 使用 Line Feed (LF)，"换行"，即 "\n" 表示换行
        /// </summary>
        public const char LF = '\n';
        /// <summary>
        /// Mac 系统中：每行结尾是 "回车"，即 "\r" 表示换行
        /// </summary>
        public const char CR = '\r';
        /// <summary>
        /// Windows 使用 Carriage Return + Line Feed (CRLF)，表示换行
        /// </summary>
        public static readonly char[] CRLF = { CR, LF };
        /// <summary>
        /// 空格
        /// </summary>
        public const char Space = ' ';
        /// <summary>
        /// 逗号
        /// </summary>
        public const char Comma = ',';
    }
}
