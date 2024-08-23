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
            //DescriptionTester.Run();

            LogTester.Run();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
