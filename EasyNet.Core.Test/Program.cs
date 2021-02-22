using EasyNet.Core.Extension;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNet.Core.Test
{
    [TestFixture]
    internal class Program
    {
        private static ILog log = ServiceSingleton.GetRequiredService<ILog>();

        [Test]
        private static void Main(string[] args)
        {
            log.Debug("测试日志输出...");
            int a = 2;
            Assert.AreEqual(2, a);
            Console.ReadKey();
        }

    }
}
