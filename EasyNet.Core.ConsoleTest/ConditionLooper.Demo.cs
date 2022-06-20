using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.ConsoleTest
// 文件名称：ConditionLooper
// 创 建 者：lanwah
// 创建日期：2022/3/14 9:11:16
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.ConsoleTest
{
    public class ConditionLooper
    {
        public static void Run()
        {
            var i = 0;
            ConditionMonitor.WaitAsync(() =>
            {
                i++;
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {i}.");
                if (i >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (result) =>
            {
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {result}.");
            }, 1000, 5000);

            var j = 0;
            ConditionMonitor.WaitForTimesAsync(() =>
            {
                j++;
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> {j}.");
                if (j >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (result2) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> result = {result2} 任务完成。");
            }, 1000, 5);

            var result3 = ConditionMonitor.Wait(() =>
            {
                i++;
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {i}.");
                if (i >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, 1000, 5000);
            LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {result3}.");

            var result4 = ConditionMonitor.WaitForTimes(() =>
            {
                i++;
                Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> {i}.");
                if (i > 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, 1000, 5);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> result = {result4} 任务完成。");

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
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {name} {i}");
                System.Threading.Thread.Sleep(1000);

                if (i >= LoopTimes)
                {
                    break;
                }
            }

            LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {name} end.");
        }
    }
}
