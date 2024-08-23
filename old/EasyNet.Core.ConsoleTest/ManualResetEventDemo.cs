using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EasyNet.Core.ConsoleTest
{
    internal class ManualResetEventDemo
    {
        private static ManualResetEvent event_1 = new ManualResetEvent(true);
        private static ManualResetEvent event_2 = new ManualResetEvent(false);

        public static void Run()
        {
            //event_1.Reset();
            Console.WriteLine("Press Enter to create three threads and start them.\r\n" +
                              "The threads wait on AutoResetEvent #1, which was created\r\n" +
                              "in the signaled state, so the first thread is released.\r\n" +
                              "This puts ManualResetEvent #1 into the unsignaled state.");
            Console.ReadLine();

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

        static void ThreadProc()
        {
            string name = Thread.CurrentThread.Name;

            Console.WriteLine("{0} waits on ManualResetEvent #1.", name);
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
            event_1.Reset();
            Console.WriteLine("{0} is released from ManualResetEvent #1.", name);

            Console.WriteLine("{0} waits on ManualResetEvent #2.", name);
            event_2.WaitOne();
            Console.WriteLine("{0} is released from ManualResetEvent #2.", name);

            Console.WriteLine("{0} ends.", name);
        }
    }
}
