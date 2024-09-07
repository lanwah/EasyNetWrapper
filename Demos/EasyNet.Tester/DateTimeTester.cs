using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Tester
// CLR版本：4.0.30319.42000
// 运行要求：4.8
// 文件名称：DateTimeTester.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 9:45:16
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Tester
{
    internal class DateTimeTester
    {
        /// <summary>
        /// https://tool.lu/timestamp/
        /// https://www.cnblogs.com/fengjq/p/17583940.html
        /// https://www.cnblogs.com/ouyangkai/p/17412008.html
        /// </summary>
        public static void Run()
        {
            // 把本地时间转换为UTC时间
            DateTime sourceTime = Convert.ToDateTime("2024-09-05 17:26:49.123+08:00");//Convert.ToDateTime("2024-09-05 17:26:49.123");
            //DateTime sourceTime = Convert.ToDateTime("2024-09-05 17:26:49.123");
            var localTime = sourceTime.ToLocalTime();
            DateTime utcTime = sourceTime.ToUniversalTime();
            var len = 30;
            var format = "yyyy-MM-dd HH:mm:ss.fff";
            Console.WriteLine($"{"SourceTime:".PadLeft(len)} {sourceTime.ToString(format)}, kind: {sourceTime.Kind}");
            Console.WriteLine($"{"Local Time:".PadLeft(len)} {localTime.ToString(format)}, kind: {localTime.Kind}");
            Console.WriteLine($"{"UTC Time:".PadLeft(len)} {utcTime.ToString(format)}, kind: {utcTime.Kind}");

            //DateTimeOffset localOffsetTime = DateTimeOffset.Now;
            DateTimeOffset localOffsetTime = new DateTimeOffset(localTime);
            DateTimeOffset utcOffsetTime = localOffsetTime.ToUniversalTime();
            Console.WriteLine($"{"Local Offset Time:".PadLeft(len)} {localOffsetTime}");
            Console.WriteLine($"{"UTC Offset Time:".PadLeft(len)} {utcOffsetTime}");

            // 把当前时间转换成10位Unix时间戳
            long unixTime = (long)(localTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            Console.WriteLine($"{"Unix TimeStamp:".PadLeft(len)} {unixTime} Local Time Converted,ERROR!");

            unixTime = (long)(localTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
            Console.WriteLine($"{"Unix TimeStamp:".PadLeft(len)} {unixTime} Local Time Converted,ERROR!");

            unixTime = localOffsetTime.ToUnixTimeSeconds();
            Console.WriteLine($"{"Unix TimeStamp:".PadLeft(len)} {unixTime}");
            Console.WriteLine($"{"10 Bits Unix TimeStamp:".PadLeft(len)} {localTime.To10BitsUnixTimeStamp()}");
            Console.WriteLine($"{"10 Bits Unix TimeStamp:".PadLeft(len)} {utcTime.To10BitsUnixTimeStamp()}");

            var utcTime2 = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixTime);
            Console.WriteLine($"{"Unix TimeStamp From:".PadLeft(len)} {utcTime2.ToString(format)}, kind: {utcTime2.Kind}");
            var time = unixTime.From10BitsUnixTimeStamp();
            Console.WriteLine($"{"10 Bits Unix TimeStamp From:".PadLeft(len)} {time.ToString(format)}, kind: {time.Kind}");

            // 把10位Unix时间戳转换成DateTime
            DateTime unixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
            Console.WriteLine($"{"Unix TimeStamp From:".PadLeft(len)} {unixDateTime.ToString(format)}, kind: {unixDateTime.Kind}");

            unixTime = localOffsetTime.ToUnixTimeMilliseconds();
            Console.WriteLine($"{"Unix TimeStamp:".PadLeft(len)} {unixTime}");
            Console.WriteLine($"{"13 Bits Unix TimeStamp:".PadLeft(len)} {localTime.To13BitsUnixTimeStamp()}");
            Console.WriteLine($"{"13 Bits Unix TimeStamp:".PadLeft(len)} {utcTime.To13BitsUnixTimeStamp()}");
            time = unixTime.From13BitsUnixTimeStamp();
            Console.WriteLine($"{"13 Bits Unix TimeStamp From:".PadLeft(len)} {time.ToString(format)}, kind: {time.Kind}");
        }
    }
}
