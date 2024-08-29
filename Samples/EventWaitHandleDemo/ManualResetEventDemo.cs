using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EventWaitHandleDemo
// CLR版本：4.0.30319.42000
// 运行要求：4.5
// 文件名称：ManualResetEventDemo.cs
// 创建用户：lanwah
// 创建日期：2024/8/27 19:32:04
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EventWaitHandleDemo
{
    /// <summary>
    /// ManualResetEvent 类:https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.manualresetevent?view=net-8.0
    /// </summary>
    public class ManualResetEventDemo
    {
        private static ManualResetEvent event_1 = new ManualResetEvent(true);
        private static ManualResetEvent event_2 = new ManualResetEvent(false);
        // mre is used to block and release threads manually. It is
        // created in the unsignaled state.
        private static ManualResetEvent mre = new ManualResetEvent(false);

        public static void Run()
        {
            // 构造函数参数解析：
            // bool initialState: 初始状态，true 为有信号状态，false 无信号状态
            // 有信号状态下，WaitOne 线程继续执行，不会阻塞
            // 无信号状态下，WaitOne 将导致线程阻塞
            // 方法解析：
            // Reset()方法将状态设置为无信号状态，从而使得碰到WaitOne时线程将被阻塞
            // Set()方法将状态设置为有信号状态，从而使得卡在WaitOne的线程恢复继续执行。
            // WaitOne()方法将根据当时的状态决定是否阻塞线程，如果状态为有信号状态，则线程将继续执行（注意！！！AutoResetEvent中WaitOne后会自动调用Reset使得一次只能有一个线程执行），如果状态为无信号状态，则线程将被阻塞。

            //event_1.Reset();
            Console.WriteLine("Press Enter to create three threads and start them.\r\n" +
                              "The threads wait on ManualResetEvent #1, which was created\r\n" +
                              "in the signaled state, so the first thread is released.\r\n" +
                              "This puts ManualResetEvent #1 into the unsignaled state.");
            Console.ReadLine();

            void ThreadProc()
            {
                string name = Thread.CurrentThread.Name;

                Console.WriteLine("{0} waits on ManualResetEvent #1.", name);
                // WaitOne will return immediately if the event is already signaled.
                // In this case, the event is already signaled, so the thread will
                // immediately continue.
                // If the event is not signaled, WaitOne will block the thread until
                // the event is signaled.
                Console.WriteLine();

                event_1.WaitOne();
                // 需要手动设置为无信号状态，注释此行，线程状态不会重置，一直为有信号状态，所有线程将继续执行
                event_1.Reset();
                Console.WriteLine("{0} is released from ManualResetEvent #1.", name);

                event_1.WaitOne();
                // 需要手动设置为无信号状态
                event_1.Reset();

                Console.WriteLine("{0} waits on ManualResetEvent #2.", name);
                event_2.WaitOne();
                Console.WriteLine("{0} is released from ManualResetEvent #2.", name);

                Console.WriteLine("{0} ends.", name);
            }

            // 创建3个线程
            for (int i = 1; i < 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }
            Thread.Sleep(250);

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Press Enter to release another thread.");
                Console.ReadLine();
                event_1.Set();
                Thread.Sleep(250);
            }

            Console.WriteLine("\r\nAll threads are now waiting on ManualResetEvent #2.");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Press Enter to release a thread.");
                Console.ReadLine();
                event_2.Set();
                Thread.Sleep(250);
            }

            // Visual Studio: Uncomment the following line.
            //Console.Readline();
        }

        public static void Run2()
        {
            void ThreadProc()
            {
                string name = Thread.CurrentThread.Name;

                Console.WriteLine(name + " starts and calls mre.WaitOne()");

                mre.WaitOne();

                Console.WriteLine(name + " ends.");
            }

            Console.WriteLine("\nStart 3 named threads that block on a ManualResetEvent:\n");

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nWhen all three threads have started, press Enter to call Set()" +
                              "\nto release all the threads.\n");
            Console.ReadLine();

            mre.Set();

            Thread.Sleep(500);
            Console.WriteLine("\nWhen a ManualResetEvent is signaled, threads that call WaitOne()" +
                              "\ndo not block. Press Enter to show this.\n");
            Console.ReadLine();

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Reset(), so that threads once again block" +
                              "\nwhen they call WaitOne().\n");
            Console.ReadLine();

            mre.Reset();

            // Start a thread that waits on the ManualResetEvent.
            Thread t5 = new Thread(ThreadProc);
            t5.Name = "Thread_5";
            t5.Start();

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Set() and conclude the demo.");
            Console.ReadLine();

            mre.Set();

            // If you run this example in Visual Studio, uncomment the following line:
            //Console.ReadLine();
        }
    }
}
