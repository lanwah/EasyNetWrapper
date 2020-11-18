using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Core.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var log = ServiceSingleton.GetRequiredService<ILog>();
            log.Debug("测试日志输出...");
            Console.ReadKey();
        }
    }
}
