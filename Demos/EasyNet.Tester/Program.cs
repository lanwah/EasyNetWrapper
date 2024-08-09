using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //var loggerProvider = new ConsoleLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //var loggerProvider = new DebugLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //var loggerProvider = new SimpleFileLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //DescriptionTester.Run();

            var logger = LoggerFactory.Create(builder =>
            {
                //builder.AddConsole();
                //builder.AddDebug();
                //builder.AddSimpleFile();
                builder.AddColorConsole();
                builder.SetMinimumLevel(LogLevel.Trace);
            }).CreateLogger("Test");
            logger.LogDebug("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "D");
            logger.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "I");
            logger.LogError("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "E");


            //Console.WriteLine("Hello, world!","Title");
            //Debug.WriteLine("Hello, world!", "Title");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
