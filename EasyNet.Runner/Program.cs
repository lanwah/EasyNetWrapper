using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Runner
{
    /// <summary>
    /// 控制台程序入口
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主函数
        /// </summary>
        /// <param name="args"></param>
        public static void Main(params string[] args)
        {
            Console.WriteLine($"EasyNet.Runner{Environment.NewLine}");
            Console.WriteLine("Input the application name to run:");
            var name = Console.ReadLine();

            // =========================== 同步执行 =========================== //
            // 通过cmd.exe /c 执行cmd命令
            // /C 执行字符串指定的命令然后终断
            // /K 执行字符串指定的命令但保留
            // https://bbs.csdn.net/topics/270017090

            var runner = new EasyNet.Runner.ProcessRunner("cmd");
            //// 不显示cmd窗体
            ////runner.CreateNoWindow = false;
            ////runner.RedirectStandardOutput = false;
            //var result = runner.Execute($"/c dir");

            // 显示cmd窗体，在Console中无效果
            runner.CreateNoWindow = false;
            runner.RedirectStandardOutput = false;
            var result = runner.Execute($"/k dir");

            Console.WriteLine(result);

            //// 通过git.exe执行git命令
            //var runner = new EasyNet.Runner.ProcessRunner("git", @"E:\Ewell\StandNuringProj\BinzhouNew\BinzhouNew_debug");
            //var result = runner.Execute("status");
            //Console.WriteLine(result);
            // ============================= END ============================= //



            // =========================== 异步执行 =========================== //
            //// 通过cmd.exe /c 执行cmd命令
            //// /C 执行字符串指定的命令然后终断
            //// /K 执行字符串指定的命令但保留
            //// https://bbs.csdn.net/topics/270017090

            //var runner = new EasyNet.Runner.ProcessRunner("cmd");
            ////runner.CreateNoWindow = false;
            ////runner.RedirectStandardOutput = false;
            //var result = await runner.ExecuteAsync($"/c dir");
            //Console.WriteLine(result);

            //// 通过git.exe执行git命令
            //var runner = new EasyNet.Runner.ProcessRunner("git", @"E:\Ewell\StandNuringProj\BinzhouNew\BinzhouNew_debug");
            //var result = await runner.ExecuteAsync("status");
            //Console.WriteLine(result);
            // ============================= END ============================= //

            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
