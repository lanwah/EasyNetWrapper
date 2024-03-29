﻿using EasyNet.Core.Extension;
using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyNet.Core.Test
{
    [TestFixture]
    internal class Program
    {
        private static ILog log = ServiceSingleton.GetRequiredService<ILog>();

        //[Test]
        //private static void Main(string[] args)
        //{
        //    log.Debug("测试日志输出...");
        //    int a = 2;
        //    Assert.AreEqual(2, a);
        //    Console.ReadKey();
        //}

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
        }

    }
}
