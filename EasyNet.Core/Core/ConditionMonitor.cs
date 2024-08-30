using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using EasyNet.Core.Service;
using EasyNet.Extensions;
using EasyNet.Log;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Core
// CLR版本：4.0.30319.42000
// 运行要求：3.5
// 文件名称：ConditionMonitor.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 16:48:47
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core
{
    /// <summary>
    /// 条件监视器扩展方法
    /// </summary>
    public static class ConditionMonitorExts
    {
        /// <summary>
        /// 周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或超时时间已到则退出。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="timeOut">检测超时时间，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="interval">检测时间间隔，默认100毫秒。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为 false</param>
        /// <returns>检测结果</returns>
        public static ConditionCheckResult WaitUntil(this Func<bool> condition, int timeOut = -1, int interval = 100, bool isDelay = false)
        {
#if NET5_0_OR_GREATER
            using var monitor = new ConditionMonitor() { Interval = interval, TimeOut = timeOut, IsFirstTimeDelay = isDelay };
            return monitor.Run(condition);
#else
            using (var monitor = new ConditionMonitor() { Interval = interval, TimeOut = timeOut, IsFirstTimeDelay = isDelay })
            {
                return monitor.Run(condition);
            }
#endif
        }
        /// <summary>
        /// 周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或循环检测次数已到则退出。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="loopTimes">循环检测的次数，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="interval">检测时间间隔，默认1000毫秒即1秒。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为 false</param>
        /// <returns></returns>
        public static ConditionCheckResult WaitUntilTimes(this Func<bool> condition, int loopTimes = -1, int interval = 1000, bool isDelay = false)
        {
#if NET5_0_OR_GREATER
            using var monitor = new ConditionMonitor() { Interval = interval, LoopTimes = loopTimes, IsFirstTimeDelay = isDelay };
            return monitor.Run(condition);
#else
            using (var monitor = new ConditionMonitor() { Interval = interval, LoopTimes = loopTimes, IsFirstTimeDelay = isDelay })
            {
                return monitor.Run(condition);
            }
#endif
        }

        /// <summary>
        /// 异步周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或超时时间已到则退出。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="callback">退出时的回调函数。</param>
        /// <param name="timeOut">检测超时时间，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="interval">检测时间间隔，默认100毫秒。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为 false</param>
        /// <returns>检测结果</returns>
        public static void WaitUntilAsync(this Func<bool> condition, Action<ConditionCheckResult> callback, int timeOut = -1, int interval = 100, bool isDelay = false)
        {
            var monitor = new ConditionMonitor() { Interval = interval, TimeOut = timeOut, IsFirstTimeDelay = isDelay };

            monitor.RunAsync(condition, (result) =>
            {
                callback?.Invoke(result);
                monitor.Dispose();
            });
        }

        /// <summary>
        /// 异步周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或循环检测次数已到则退出，通过回调函数返回执行结果。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="callback">退出时的回调函数。</param>
        /// <param name="loopTimes">循环检测的次数，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="interval">检测时间间隔，默认1000毫秒即1秒。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为 false</param>
        /// <returns></returns>
        public static void WaitUntilTimesAsync(this Func<bool> condition, Action<ConditionCheckResult> callback, int loopTimes = -1, int interval = 1000, bool isDelay = false)
        {
            var monitor = new ConditionMonitor() { Interval = interval, LoopTimes = loopTimes, IsFirstTimeDelay = isDelay };

            monitor.RunAsync(condition, (result) =>
            {
                callback?.Invoke(result);
                monitor.Dispose();
            });
        }
    }

    /// <summary>
    /// 条件监视器
    /// </summary>
    internal class ConditionMonitor : IDisposable, IConditionMonitor
    {
        /// <summary>
        /// <see cref="Execute"/>的执行结果，<seealso cref="ConditionCheckResult"/>类型枚举值。
        /// </summary>
        public ConditionCheckResult Result = ConditionCheckResult.None;
        /// <summary>
        /// 周期循环的检查任务，返回 true 时则会终止循环检测
        /// </summary>
        public event Func<bool> Execute;
        /// <summary>
        /// 结束循环调用触发的事件
        /// </summary>
        public event Action<ConditionCheckResult> LoopEnded;
        private int _interval = 100;
        /// <summary>
        /// 时间间隔，默认100毫秒（以毫秒为单位）
        /// </summary>
        public int Interval
        {
            get => this._interval;
            set => this._interval = value;
        }
        private bool _isFirstTimeDelay = false;
        /// <summary>
        /// 首次调用是否延迟<see cref="Interval"/>指定的时间间隔，默认为false
        /// </summary>
        public bool IsFirstTimeDelay
        {
            get => this._isFirstTimeDelay;
            set => this._isFirstTimeDelay = value;
        }
        private int _timeOut = -1;
        /// <summary>
        /// 超时时间（以毫秒为单位），默认为 -1 不启用
        /// </summary>
        public int TimeOut
        {
            get => this._timeOut;
            set => this._timeOut = value;
        }
        private int _loopTimes = -1;
        /// <summary>
        /// 循环执行次数，默认为 -1 不启用
        /// </summary>
        public int LoopTimes
        {
            get => this._loopTimes;
            set => this._loopTimes = value;
        }
        /// <summary>
        /// 是否正在运行
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        } = false;
        private bool _enable;
        /// <summary>
        /// 获取状态，启用/停用
        /// </summary>
        public virtual bool Enable
        {
            get
            {
                return this._enable;
            }
            private set
            {
                if (this._enable != value)
                {
                    this._enable = value;
                }

                if (this._enable)
                {
                    this.Log($"启动循环任务....");
                    this.Log($"Interval = {this.Interval}ms，TimeOut = {this.TimeOut}ms，LoopTimes = {this.LoopTimes}，IsFirstTimeDelay = {this.IsFirstTimeDelay}");

                    this.Start((state) => this.Work(state));
                }
                else
                {
                    this.Log($"停止循环任务....");
                    this.Stop();
                }
            }
        }
        /// <summary>
        /// 判断是否处于工作<see cref="Work"/>状态
        /// </summary>
        private bool IsInWorking
        {
            get; set;
        }
        /// <summary>
        /// 循环任务
        /// </summary>
        /// <param name="state"></param>
        protected virtual void Work(object state)
        {
            this.TriggerWorkTimes++;
            if (this.IsInWorking)
            {
                return;
            }
            this.IsInWorking = true;
            // 调用次数 +1
            this.ExecuteTimes++;

            try
            {
                // 执行任务
                var matched = (this.Execute?.Invoke()).GetValueOrDefault(false);

                if (matched)
                {
                    // 满足条件，则停止
                    if (this.IsEnableTimeOut)
                    {
                        this.Log($"ElapsedMilliseconds(ms) = {this.TotalTimeWatch.ElapsedMilliseconds}/{this.TimeOut}.");
                    }
                    if (this.IsEnableLoopTimes)
                    {
                        this.Log($"ExecuteTimes = {this.ExecuteTimes}/{this.LoopTimes}.");
                    }
                    this.Log($"{this.EndText("满足条件停止")}");

                    this.LoopEndedInternal(ConditionCheckResult.Matched);
                    return;
                }

                // 启用超时设置时进行超时判断
                if (this.IsEnableTimeOut)
                {
                    this.Log($"ElapsedMilliseconds(ms) = {this.TotalTimeWatch.ElapsedMilliseconds}/{this.TimeOut}.");
                    if (this.TotalTimeWatch.ElapsedMilliseconds >= this.TimeOut)
                    {
                        // 超时
                        this.Log($"{this.EndText("超时退出")}");
                        this.LoopEndedInternal(ConditionCheckResult.TimeOut);
                        return;
                    }
                }

                if (this.IsEnableLoopTimes)
                {
                    //// 调用次数 +1
                    //this.ExecuteTime++;

                    this.Log($"ExecuteTimes = {this.ExecuteTimes}/{this.LoopTimes}.");
                    if (this.ExecuteTimes >= this.LoopTimes)
                    {
                        // 超过次数限制
                        this.Log($"{this.EndText("超过次数限制退出")}");
                        this.LoopEndedInternal(ConditionCheckResult.TimesLimited);
                        return;
                    }
                }
            }
            finally
            {
                this.IsInWorking = false;
            }
        }
        /// <summary>
        /// 获取结束时的文字描述
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string EndText(string result)
        {
            var statusText = $"ExecuteTimes = {this.ExecuteTimes}，TriggerWorkTimes = {this.TriggerWorkTimes}";
            return $"{result}, {statusText}";
        }

        /// <summary>
        /// 定时任务
        /// </summary>
        private System.Threading.Timer JobTimer;
        /// <summary>
        /// 总耗时计时器
        /// </summary>
        private Stopwatch TotalTimeWatch;
        /// <summary>
        /// 记录实际调用次数
        /// </summary>
        private int ExecuteTimes
        {
            get; set;
        } = 0;
        /// <summary>
        /// 触发执行<see cref="Work(object)"/>的次数
        /// </summary>
        private double TriggerWorkTimes
        {
            get; set;
        } = 0;
        /// <summary>
        /// 是否启用异步调用
        /// </summary>
        private bool IsAsyncCall = false;
        /// <summary>
        /// 等待信号，false - 默认等待；ture - 默认放行
        /// </summary>
#if NET5_0_OR_GREATER
        private AutoResetEvent WaitSignal = new(false);
#else
        private AutoResetEvent WaitSignal = new AutoResetEvent(false);
#endif
        /// <summary>
        /// 判断是否启用超时时间设置
        /// </summary>
        private bool IsEnableTimeOut
        {
            get
            {
                return (this.TimeOut > 0);
            }
        }
        /// <summary>
        /// 判断是否启用次数限制设置
        /// </summary>
        private bool IsEnableLoopTimes
        {
            get
            {
                return (this.LoopTimes > 0);
            }
        }

        /// <summary>
        /// 执行检查任务（同步调用）
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        public ConditionCheckResult Run(Func<bool> @checker)
        {
            this.IsAsyncCall = false;
            this.Execute = @checker;
            // 会把线程信号量设置为无信号状态，然后启动任务（任务在异步执行）
            this.Enable = true;

            // 任务运行状态时由于是无信号状态，导致线程阻塞，等待任务完成后会收到信号量从而继续往下运行，返回结果
            this.WaitSignal.WaitOne();

            return this.Result;
        }
        /// <summary>
        /// 执行检查任务(异步调用)
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="callback"></param>
        public void RunAsync(Func<bool> @checker, Action<ConditionCheckResult> callback)
        {
            this.IsAsyncCall = true;
            this.Execute = @checker;
            this.LoopEnded = callback;
            this.Enable = true;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            // 停止计时器
            this.StopTimer();

            if (null != this.WaitSignal)
            {
#if !NET35
                this.WaitSignal.Dispose();
#endif
                this.WaitSignal = null;
            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 启动任务（创建并启动计时器）
        /// </summary>
        /// <param name="callback"></param>
        protected virtual void Start(System.Threading.TimerCallback callback)
        {
            if ((null != this.JobTimer))
            {
                // 停止计时器
                this.StopTimer();
            }
            // 重置运行状态
            this.Reset();

            // System.Threading.Timer轻量，精度相对较高，与Windows操作系统时钟精度一致，大约15毫秒
            this.JobTimer = new System.Threading.Timer(callback, null, (this.IsFirstTimeDelay ? this.Interval : 0), this.Interval);
        }
        /// <summary>
        /// 重置运行状态
        /// </summary>
        protected virtual void Reset()
        {
            this.Result = ConditionCheckResult.None;
            this.IsInWorking = false;

            if (!this.IsAsyncCall)
            {
                // 设置为无信号状态，导致外部线程阻塞，等待任务完成或超时
                this.WaitSignal.Reset();
            }

            if (this.IsEnableTimeOut)
            {
                if (null == this.TotalTimeWatch)
                {
                    this.TotalTimeWatch = new Stopwatch();
                }
                else
                {
                    this.TotalTimeWatch.Reset();
                }

                this.TotalTimeWatch.Start();
            }

            this.ExecuteTimes = 0;
            this.TriggerWorkTimes = 0;
            this.IsRunning = true;
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        protected virtual void Stop()
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.IsRunning = false;

            // 停止计时器
            this.StopTimer();

            this.TotalTimeWatch?.Stop();

            // 信号放行
            if (!this.IsAsyncCall)
            {
                // 设置为有信号状态，使等待现场继续执行
                this.WaitSignal?.Set();
            }
        }
        /// <summary>
        /// 停止计时器
        /// </summary>
        private void StopTimer()
        {
            if (null != this.JobTimer)
            {
                this.JobTimer.Dispose();
                this.JobTimer = null;
            }
        }
        /// <summary>
        /// 触发结束事件
        /// </summary>
        /// <param name="result">结果</param>
        protected virtual void LoopEndedInternal(ConditionCheckResult result)
        {
            // 保存结果
            this.Result = result;
            // 执行停止
            this.Stop();

            // 触发结束事件
            this.LoopEnded?.Invoke(result);
        }
        /// <summary>
        /// 日志记录器
        /// </summary>
        public ILogger Logger
        {
            get;
            set;
        } = SimpleService.Logger;
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="content"></param>
        //[Conditional("DEBUG")]
        protected virtual void Log(string content)
        {
            if (this.Logger.IsNull())
            {
                return;
            }

            var msg = $"{string.Format("{0,-4}", Environment.ManagedThreadId)} -> {content}";
            this.Logger.LogDebug(msg);
        }
    }
    internal interface IConditionMonitor
    {
        /// <summary>
        /// 获取状态，启用/停用
        /// </summary>
        bool Enable { get; }
        /// <summary>
        /// 时间间隔，默认100毫秒（以毫秒为单位），建议不要小于15毫秒
        /// </summary>
        int Interval { get; set; }
        /// <summary>
        /// 首次调用是否延迟<see cref="Interval"/>指定的时间间隔，默认为 false
        /// </summary>
        bool IsFirstTimeDelay { get; set; }
        /// <summary>
        /// 是否正在运行
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        /// 循环执行次数，默认为 -1 不启用
        /// </summary>
        int LoopTimes { get; set; }
        /// <summary>
        /// 超时时间（以毫秒为单位），默认为 -1 不启用
        /// </summary>
        int TimeOut { get; set; }

        /// <summary>
        /// 结束循环调用触发的事件
        /// </summary>
        event Action<ConditionCheckResult> LoopEnded;
        /// <summary>
        /// 周期循环的检查任务，返回 true 时则会终止循环检测
        /// </summary>
        event Func<bool> Execute;

        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();
        /// <summary>
        /// 执行检查任务（同步调用）
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        ConditionCheckResult Run(Func<bool> checker);
        /// <summary>
        /// 执行检查任务(异步调用)
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="callback"></param>
        void RunAsync(Func<bool> checker, Action<ConditionCheckResult> callback);
    }

    /// <summary>
    /// 条件检查结果
    /// </summary>
    public enum ConditionCheckResult
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        None = 0,
        /// <summary>
        /// 匹配
        /// </summary>
        Matched = 1,
        /// <summary>
        /// 超时
        /// </summary>
        TimeOut = 2,
        /// <summary>
        /// 超过次数限制
        /// </summary>
        TimesLimited = 3
    }
}
