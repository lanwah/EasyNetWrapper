using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Log;

namespace EasyNet.Tester
{
    internal class Program
    {
        static void Main()
        {
            var loggerProvider =new ConsoleLoggerProvider();
            var log = loggerProvider.CreateLogger("Test");
            log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //var loggerProvider = new ConsoleLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //DescriptionTester.Run();

            Console.ReadKey();
        }
    }
}
