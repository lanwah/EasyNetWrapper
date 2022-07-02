using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static System.Console;

namespace EasyNet.Core.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 启动控制台");
            

            ReadKey();
        }

    }
}
