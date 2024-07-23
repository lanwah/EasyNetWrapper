using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    internal class Program
    {
        /// <summary>
        /// 类型转换的基准测试
        /// </summary>
        /// <param name="args"></param>
        static void Main()
        {
            //// Object转Int
            //BenchmarkRunner.Run<ObjectToInt>();

            // Object转字符串
            BenchmarkRunner.Run<ObjectToString>();

            Console.ReadKey();
        }
    }
}
