using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Core;
using EasyNet.Log;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Tester
// CLR版本：4.0.30319.42000
// 运行要求：4.8
// 文件名称：ConditionMonitorTest.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 18:27:41
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Tester
{
    /// <summary>
    /// 条件监视器测试类
    /// </summary>
    public class ConditionMonitorTest
    {
        public static void Run()
        {
            var i = 0;
            Func<bool> condition = () =>
            {
                i++;
                //System.Threading.Thread.Sleep(1000);
                LoggerFactory.Default.LogDebug($"{string.Format("{0,-4}", Core.Environment.ManagedThreadId)} -> {i}.");
                if (i >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
            var result = ConditionCheckResult.None;

            //// 时间限制
            // result = func.WaitUntil(5000, 200);

            //// 次数限制
            //result = condition.WaitUntilTimes(30, 100);

            // 同步结果输出
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> result = {result} 任务完成。{System.Environment.NewLine}");

            //// 异步时间限制
            //condition.WaitUntilAsync((result2) =>
            //{
            //    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> result = {result2} 任务完成。{Environment.NewLine}");
            //}, 30 * 1000, 200);

            // 异步次数限制
            condition.WaitUntilTimesAsync((result2) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> result = {result2} 任务完成。{System.Environment.NewLine}");
            }, -1, 200);

            Print("A");
        }


        private const int LoopTimes = 10;
        /// <summary>
        /// 同步调用
        /// </summary>
        private static void Print(string name)
        {
            int i = 0;
            while (true)
            {
                i++;
                LoggerFactory.Default.LogDebug($"{string.Format("{0,-4}", Core.Environment.ManagedThreadId)} -> {name} {i}");
                System.Threading.Thread.Sleep(1000);

                if (i >= LoopTimes)
                {
                    break;
                }
            }

            LoggerFactory.Default.LogDebug($"{string.Format("{0,-4}", Core.Environment.ManagedThreadId)} -> {name} end.");
        }
    }
}
