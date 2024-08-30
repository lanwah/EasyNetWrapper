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
// 文件名称：AutoResetEventDemo.cs
// 创建用户：lanwah
// 创建日期：2024/8/27 17:02:42
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
    /// AutoResetEvent 类:https://learn.microsoft.com/zh-cn/dotnet/api/system.threading.autoresetevent?view=net-8.0
    /// </summary>
    public class AutoResetEventDemo
    {
        private static readonly AutoResetEvent event_1 = new AutoResetEvent(true);
        private static readonly AutoResetEvent event_2 = new AutoResetEvent(false);

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

            // true - 初始化时，运行流程如下：
            // 1. 构造参数为true，初始状态为有信号状态。
            // 2. 调用WaitOne方法，线程没有任何效果，继续执行，但是调用WaitOne方法之后线程会自动恢复到无信号状态，此时状态为无信号状态。
            // 3. 继续执行，再次碰到WaitOne方法时，由于状态为无信号状态，线程将会被阻塞。
            // 4. 线程将一直被阻塞，直到其他线程调用Set方法将状态设置为有信号状态，此时线程将恢复，继续执行。
            //AutoResetEvent autoResetEvent = new AutoResetEvent(true);
            //autoResetEvent.Reset();
            //autoResetEvent.WaitOne(); 
            //autoResetEvent.Set();

            // 测试代码
            Console.WriteLine("Press Enter to create three threads and start them.\r\n" +
                              "The threads wait on AutoResetEvent #1, which was created\r\n" +
                              "in the signaled state, so the first thread is released.\r\n" +
                              "This puts AutoResetEvent #1 into the unsignaled state.");
            Console.ReadLine();

            void ThreadProc()
            {
                string name = Thread.CurrentThread.Name;

                Console.WriteLine("{0} waits on AutoResetEvent #1.", name);
                // WaitOne will return immediately if the event is already signaled.
                // In this case, the event is already signaled, so the thread will
                // immediately continue.
                // If the event is not signaled, WaitOne will block the thread until
                // the event is signaled.
                Console.WriteLine();
                // AutoResetEvent.WaitOne will automatically reset the event to
                // the unsignaled state after the thread is released.
                // AutoResetEvent.WaitOne 的功能相当于 ManualResetEvent.WaitOne 和 ManualResetEvent.Reset 的结合体。
                event_1.WaitOne();
                Console.WriteLine("{0} is released from AutoResetEvent #1.", name);

                Console.WriteLine("{0} waits on AutoResetEvent #2.", name);
                event_1.WaitOne();
                Console.WriteLine("{0} is released from AutoResetEvent #2.", name);

                Console.WriteLine("{0} ends.", name);
            }

            // 开启3个线程
            for (int i = 1; i < 4; i++)
            {
                var t = new Thread(ThreadProc)
                {
                    Name = "Thread_" + i
                };
                t.Start();
            }
            Thread.Sleep(250);

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Press Enter to release another thread.");
                Console.ReadLine();
                event_1.Set();
                Thread.Sleep(250);
            }
        }

        public static void Run2()
        {
            Console.WriteLine("Press Enter to create three threads and start them.\r\n" +
                              "The threads wait on AutoResetEvent #1, which was created\r\n" +
                              "in the signaled state, so the first thread is released.\r\n" +
                              "This puts AutoResetEvent #1 into the unsignaled state.");
            Console.ReadLine();
            void ThreadProc()
            {
                string name = Thread.CurrentThread.Name;

                Console.WriteLine("{0} waits on AutoResetEvent #1.", name);
                // WaitOne will return immediately if the event is already signaled.
                // In this case, the event is already signaled, so the thread will
                // immediately continue.
                // If the event is not signaled, WaitOne will block the thread until
                // the event is signaled.
                Console.WriteLine();
                // AutoResetEvent.WaitOne will automatically reset the event to
                // the unsignaled state after the thread is released.
                // AutoResetEvent.WaitOne 的功能相当于 ManualResetEvent.WaitOne 和 ManualResetEvent.Reset 的结合体。
                event_1.WaitOne();
                Console.WriteLine("{0} is released from AutoResetEvent #1.", name);

                Console.WriteLine("{0} waits on AutoResetEvent #2.", name);
                event_2.WaitOne();
                Console.WriteLine("{0} is released from AutoResetEvent #2.", name);

                Console.WriteLine("{0} ends.", name);
            }
            for (int i = 1; i < 4; i++)
            {
                var t = new Thread(ThreadProc)
                {
                    Name = "Thread_" + i
                };
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

            Console.WriteLine("\r\nAll threads are now waiting on AutoResetEvent #2.");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Press Enter to release a thread.");
                Console.ReadLine();
                event_2.Set();
                Thread.Sleep(250);
            }
        }
    }
}
