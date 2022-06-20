using AsyncBreakfast.Model;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：AsyncBreakfast
// 文件名称：Breakfast
// 创 建 者：lanwah
// 创建日期：2022/3/16 14:25:39
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace AsyncBreakfast
{
    public class Breakfast
    {
        /// <summary>
        /// 同步版本
        /// </summary>
        [Benchmark(Baseline = true)]
        public void RunV1()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            Egg eggs = FryEggs(2);
            FryEggsSuccess();

            Bacon bacon = FryBacon(3);
            FryBaconSuccess();

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            ToastBreadSuccess();

            Juice oj = PourOJ();
            PourOJSuccess();
            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }

        /// <summary>
        /// 异步版本，注意 await 关键字
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public async Task RunV2()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            // 异步煎鸡蛋
            Egg eggs = await FryEggsAsync(2);
            FryEggsSuccess();

            // 异步煎培根
            Bacon bacon = await FryBaconAsync(3);
            FryBaconSuccess();

            // 异步烤面包
            Toast toast = await ToastBreadAsync(2);
            ApplyButter(toast);
            ApplyJam(toast);
            ToastBreadSuccess();

            Juice oj = PourOJ();
            PourOJSuccess();

            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }

        [Benchmark]
        public async Task RunV3()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            var eggsTask = FryEggsAsync(2);
            await eggsTask;
            FryEggsSuccess();

            var baconTask = FryBaconAsync(3);
            await baconTask;
            FryBaconSuccess();

            var toastTask = ToastBreadAsync(2);
            var toast = await toastTask;
            ApplyButter(toast);
            ApplyJam(toast);
            ToastBreadSuccess();

            Juice oj = PourOJ();
            PourOJSuccess();

            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }

        [Benchmark]
        public async Task RunV4()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = ToastBreadAsync(2);

            await eggsTask;
            FryEggsSuccess();

            await baconTask;
            FryBaconSuccess();

            var toast = await toastTask;
            ApplyButter(toast);
            ApplyJam(toast);
            ToastBreadSuccess();

            Juice oj = PourOJ();
            PourOJSuccess();

            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }

        [Benchmark]
        public async Task RunV5()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            await eggsTask;
            FryEggsSuccess();

            await baconTask;
            FryBaconSuccess();

            var toast = await toastTask;
            ToastBreadSuccess();

            Juice oj = PourOJ();
            PourOJSuccess();

            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }

        [Benchmark]
        public async Task RunV6()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            await Task.WhenAll(eggsTask, baconTask, toastTask);
            FryEggsSuccess();
            FryBaconSuccess();
            ToastBreadSuccess();

            Juice oj = PourOJ();
            PourOJSuccess();

            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }

        [Benchmark]
        public async Task RunV7()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 开始....");
            var st = Stopwatch.StartNew();

            Coffee cup = PourCoffee();
            PourCoffeeSuccess();

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);
            var breakfastTasks = new List<Task>() { eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                var task = await Task.WhenAny(breakfastTasks);
                if (task == eggsTask)
                {
                    FryEggsSuccess();
                }
                else if (task == baconTask)
                {
                    FryBaconSuccess();
                }
                else if (task == toastTask)
                {
                    ToastBreadSuccess();
                }

                breakfastTasks.Remove(task);
            }

            Juice oj = PourOJ();
            PourOJSuccess();

            Console.WriteLine($"Breakfast is ready!{Environment.NewLine}");
            st.Stop();
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，耗时 = {st.ElapsedMilliseconds.ToString("###,###")} 毫秒");
        }


        /// <summary>
        /// 倒一杯橙汁
        /// </summary>
        /// <returns></returns>
        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice(倒一杯橙汁)");
            Thread.Sleep(100);
            return new Juice();
        }
        /// <summary>
        /// 面包加果酱
        /// </summary>
        /// <param name="toast"></param>
        private static void ApplyJam(Toast toast)
        {
            Thread.Sleep(50);
            Console.WriteLine("Putting jam on the toast(面包加果酱)");
        }
        /// <summary>
        /// 面包加黄油
        /// </summary>
        /// <param name="toast"></param>
        private static void ApplyButter(Toast toast)
        {
            Thread.Sleep(50);
            Console.WriteLine("Putting butter on the toast(面包加黄油)");
        }
        /// <summary>
        /// 烤面包
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        private static Toast ToastBread(int slices)
        {
            Console.WriteLine("Start toasting(烤面包)...");
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
                Thread.Sleep(100);
            }
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }
        /// <summary>
        /// 煎培根
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan(煎培根)");
            Console.WriteLine("cooking first side of bacon...");
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
                Thread.Sleep(100);
            }
            Console.WriteLine("cooking the second side of bacon...");
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }
        /// <summary>
        /// 煎鸡蛋
        /// </summary>
        /// <param name="howMany"></param>
        /// <returns></returns>
        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan(煎鸡蛋)...");
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            for (int i = 0; i < howMany; i++)
            {
                Thread.Sleep(100);
            }
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }
        /// <summary>
        /// 倒咖啡
        /// </summary>
        /// <returns></returns>
        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee(倒咖啡)");
            Thread.Sleep(100);
            return new Coffee();
        }

        private static void PourOJSuccess()
        {
            Console.WriteLine("oj is ready(橙汁已倒好)");
        }
        private static void ToastBreadSuccess()
        {
            Console.WriteLine($"toast is ready(面包已烤好){Environment.NewLine}");
        }
        private static void FryBaconSuccess()
        {
            Console.WriteLine($"bacon is ready({3}片培根已煎好){Environment.NewLine}");
        }
        private static void FryEggsSuccess()
        {
            Console.WriteLine($"eggs are ready({2}个鸡蛋已煎好){Environment.NewLine}");
        }
        private static void PourCoffeeSuccess()
        {
            Console.WriteLine($"coffee is ready(咖啡已倒好){Environment.NewLine}");
        }


        /// <summary>
        /// 煎鸡蛋
        /// </summary>
        /// <param name="howMany"></param>
        /// <returns></returns>
        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan(煎鸡蛋)...");
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            for (int i = 0; i < howMany; i++)
            {
                await Task.Delay(100);
            }
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }
        /// <summary>
        /// 煎培根
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan(煎培根)");
            Console.WriteLine("cooking first side of bacon...");
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
                await Task.Delay(100);
            }
            Console.WriteLine("cooking the second side of bacon...");
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }
        /// <summary>
        /// 烤面包
        /// </summary>
        /// <param name="slices"></param>
        /// <returns></returns>
        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            Console.WriteLine("Start toasting(烤面包)...");
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
                await Task.Delay(100);
            }
            //Console.WriteLine("Fire! Toast is ruined!");
            //throw new InvalidOperationException("The toaster is on fire");
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }
        /// <summary>
        /// 烤面包 + 黄油 + 果酱
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }


    }
}
