using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace EventWaitHandleDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ManualResetEventDemo.Run2();
            WriteLine("End of program, press any key to exit.");
            ReadKey();
        }
    }
}
