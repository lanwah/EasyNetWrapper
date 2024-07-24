using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparer
{
    internal class Program
    {
        static void Main()
        {
            //// 比较二进制内容是否一致
            //BenchmarkRunner.Run<SequenceEqual>();

            //// 对象判空
            //BenchmarkRunner.Run<ObjectNull>();

            //// 字符串判空
            //BenchmarkRunner.Run<StringNull>();

            //// 图片类型识别
            //BenchmarkRunner.Run<ImageType>();

            ImageType.Run();

            Console.ReadKey();
        }
    }
}
