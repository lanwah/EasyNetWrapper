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
            // Description扩展方法测试
            ////DescriptionTester.Run();

            // 日志测试
            ////LogTester.Run();

            //// CRC 算法测试
            //CRCTester.Run();

            // Emit 测试
            EmitTester.Run();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
