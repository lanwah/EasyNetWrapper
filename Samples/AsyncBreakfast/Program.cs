using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.OutputEncoding = System.Text.Encoding.ASCII;

            BenchmarkRunner.Run<Breakfast>();

            Console.ReadKey();
        }
    }
}
