using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Log;

namespace EasyNet.Tester
{
    internal class LogTester
    {
        public static void Run()
        {
            var logger = LoggerFactory.Create(builder =>
            {
                //builder.AddConsole();
                builder.AddDebug();
                //builder.AddFile();
                builder.AddColorConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            }).CreateLogger("Test");
            logger.LogDebug("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "D");
            logger.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "I");
            logger.LogError("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "E");

            //var loggerProvider = new ConsoleLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //var loggerProvider = new DebugLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //var loggerProvider = new SimpleFileLoggerProvider();
            //var log = loggerProvider.CreateLogger("Test");
            //log.LogInformation("Hello, world, Thread Id = {ManagedThreadId}，A = {}", System.Threading.Thread.CurrentThread.ManagedThreadId, "B");

            //Console.WriteLine("Hello, world!","Title");
            //Debug.WriteLine("Hello, world!", "Title");
        }
    }
}
