using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Log;
using EasyNet.Core.Service;
using EasyNet.Extensions;

namespace EasyNet.Tester
{
    internal class Program
    {
        static void Main()
        {
            // Description扩展方法测试
            ////DescriptionTester.Run();

            //// 日志测试
            //LogTester.Run();

            //// CRC 算法测试
            //CRCTester.Run();

            //// Emit 测试
            //EmitTester.Run();

            //// 条件监视器测试
            //ConditionMonitorTest.Run();

            Validation();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        public static void Validation()
        {  
        }
    }
}
