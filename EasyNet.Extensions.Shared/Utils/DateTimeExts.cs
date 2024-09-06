using System;
using System.Collections.Generic;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.Utils
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：DateTimeExts.cs
// 创建用户：lanwah
// 创建日期：2024/9/5 19:24:23
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// DateTime扩展类
    /// </summary>
    public static partial class DateTimeExts
    {
        /// <summary>
        /// 把DateTime转换成10位Unix时间戳, 精确到秒(本地时间会被转换成UTC时间然后计算时间戳)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long To10BitsUnixTimeStamp(this DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeSeconds();
        }
        /// <summary>
        /// 把DateTime转换成13位Unix时间戳, 精确到毫秒(本地时间会被转换成UTC时间然后计算时间戳)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long To13BitsUnixTimeStamp(this DateTime dateTime)
        {
            var dateTimeOffset = new DateTimeOffset(dateTime);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
        /// <summary>
        /// 把10位Unix时间戳转换成 UTC 时间, 精确到秒
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime From10BitsUnixTimeStamp(this long unixTime)
        {
            var dateTimeOffset = unixTime.FromUnixTimeSeconds();
            return dateTimeOffset.UtcDateTime;
        }
        /// <summary>
        /// 把13位Unix时间戳转换成 UTC 时间, 精确到毫秒
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime From13BitsUnixTimeStamp(this long unixTime)
        {
            var dateTimeOffset = unixTime.FromUnixTimeMilliseconds();
            return dateTimeOffset.UtcDateTime;
        }
        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringEx(this DateTime? dateTime, string format = "yyyy-MM-dd HH:mm")
        {
            return dateTime.HasValue ? dateTime.Value.ToString(format) : string.Empty;
        }

#if NETFRAMEWORK
        /// <summary>
        /// 把DateTimeOffset转换成10位Unix时间戳, 精确到秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeSeconds(this DateTimeOffset dateTime)
        {
            var utcDateTime = dateTime.UtcDateTime;

            return (long)(utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
        /// <summary>
        /// 把DateTimeOffset转换成13位Unix时间戳, 精确到毫秒
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(this DateTimeOffset dateTime)
        {
            var utcDateTime = dateTime.UtcDateTime;

            return (long)(utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
        /// <summary>
        /// 把10位Unix时间戳转换成 DateTimeOffset, 精确到秒
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTimeOffset FromUnixTimeSeconds(this long unixTime)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);

            return new DateTimeOffset(dateTime);
        }
        /// <summary>
        /// 把13位Unix时间戳转换成 DateTimeOffset, 精确到毫秒
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTimeOffset FromUnixTimeMilliseconds(this long unixTime)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTime);

            return new DateTimeOffset(dateTime);
        }
#else
        /// <summary>
        /// 把10位Unix时间戳转换成 DateTimeOffset, 精确到秒
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTimeOffset FromUnixTimeSeconds(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime);
        }
        /// <summary>
        /// 把13位Unix时间戳转换成 DateTimeOffset, 精确到毫秒
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTimeOffset FromUnixTimeMilliseconds(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
        }
#endif
    }
}
