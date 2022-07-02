using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Units
// 文件名称：ConditionLooper
// 创 建 者：lanwah
// 创建日期：2022/3/12 12:56:02
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core
{
    /// <summary>
    /// 条件监视器
    /// </summary>
    public static class ConditionMonitor
    {
        /// <summary>
        /// 周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或超时时间已到则退出。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="interval">检测时间间隔，默认100毫秒。</param>
        /// <param name="timeOut">检测超时时间，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为<seealso cref="false"/></param>
        /// <returns>检测结果</returns>
        public static ConditionLooperResult Wait(Func<bool> condition, int interval = 100, int timeOut = -1, bool isDelay = false)
        {
            using (var looper = new ConditionLooper() { Interval = interval, TimeOut = timeOut, IsFirstDelay = isDelay })
            {
                return looper.Run(condition);
            }
        }

        /// <summary>
        /// 周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或循环检测次数已到则退出。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="interval">检测时间间隔，默认1000毫秒即1秒。</param>
        /// <param name="loopTimes">循环检测的次数，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为<seealso cref="false"/></param>
        /// <returns></returns>
        public static ConditionLooperResult WaitForTimes(Func<bool> condition, int interval = 1000, int loopTimes = -1, bool isDelay = false)
        {
            using (var looper = new ConditionLooper() { Interval = interval, LoopTimes = loopTimes, IsFirstDelay = isDelay })
            {
                return looper.Run(condition);
            }
        }

        /// <summary>
        /// 异步周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或超时时间已到则退出。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="callback">退出时的回调函数。</param>
        /// <param name="interval">检测时间间隔，默认100毫秒。</param>
        /// <param name="timeOut">检测超时时间，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为<seealso cref="false"/></param>
        /// <returns>检测结果</returns>
        public static void WaitAsync(Func<bool> condition, Action<ConditionLooperResult> callback, int interval = 100, int timeOut = -1, bool isDelay = false)
        {
            var looper = new ConditionLooper() { Interval = interval, TimeOut = timeOut, IsFirstDelay = isDelay };

            looper.RunAsync(condition, (result) =>
            {
                callback?.Invoke(result);
                looper.Dispose();
            });
        }

        /// <summary>
        /// 异步周期性检测<paramref name="condition"/>条件表达式是否满足，直到满足条件或循环检测次数已到则退出，通过回调函数返回执行结果。
        /// </summary>
        /// <param name="condition">条件表达式。</param>
        /// <param name="callback">退出时的回调函数。</param>
        /// <param name="interval">检测时间间隔，默认1000毫秒即1秒。</param>
        /// <param name="loopTimes">循环检测的次数，-1 为不启用，<paramref name="condition"/>表示的条件表达满足时才会退出。</param>
        /// <param name="isDelay">首次调用是否延迟<paramref name="interval"/>指定的时间间隔，默认为<seealso cref="false"/></param>
        /// <returns></returns>
        public static void WaitForTimesAsync(Func<bool> condition, Action<ConditionLooperResult> callback, int interval = 100, int loopTimes = -1, bool isDelay = false)
        {
            var looper = new ConditionLooper() { Interval = interval, LoopTimes = loopTimes, IsFirstDelay = isDelay };

            looper.RunAsync(condition, (result) =>
            {
                callback?.Invoke(result);
                looper.Dispose();
            });
        }
    }

    /// <summary>
    /// 条件循环器
    /// </summary>
    internal class ConditionLooper : IDisposable, IConditionLooper
    {
        /// <summary>
        /// <see cref="WorkCheck"/>的执行结果，<seealso cref="ConditionLooperResult"/>类型枚举值。
        /// </summary>
        public ConditionLooperResult Result = ConditionLooperResult.None;
        /// <summary>
        /// 周期循环的检查任务，返回<see cref="true"/>时则会终止循环检测
        /// </summary>
        public event Func<bool> WorkCheck;
        /// <summary>
        /// 结束循环调用触发的事件
        /// </summary>
        public event Action<ConditionLooperResult> EndLoop;
        private int _interval = 100;
        /// <summary>
        /// 时间间隔，默认100毫秒（以毫秒为单位）
        /// </summary>
        public int Interval
        {
            get => this._interval;
            set => this._interval = value;
        }
        private bool _isFirstDelay = false;
        /// <summary>
        /// 首次调用是否延迟<see cref="Interval"/>指定的时间间隔，默认为<seealso cref="false"/>
        /// </summary>
        public bool IsFirstDelay
        {
            get => this._isFirstDelay;
            set => this._isFirstDelay = value;
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
        protected bool _enable;
        /// <summary>
        /// 启用/停用
        /// </summary>
        public virtual bool Enable
        {
            get
            {
                return this._enable;
            }
            set
            {
                if (this._enable != value)
                {
                    this._enable = value;
                }

                if (this._enable)
                {
                    this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 启动循环任务....");
                    this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)}Interval = {this.Interval}，TimeOut = {this.TimeOut}，LoopTimes = {this.LoopTimes}，IsFirstDelay = {this.IsFirstDelay}");
                    this.Start((state) =>
                    {
                        //Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} -> ManagedThreadId = {System.Threading.Thread.CurrentThread.ManagedThreadId}.");
                        // 执行任务
                        var matched = (this.WorkCheck?.Invoke()).GetValueOrDefault(false);
                        if (matched)
                        {
                            // 满足条件，则终止
                            this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 满足条件终止。");
                            this.EndLoopInternal(ConditionLooperResult.Matched);
                            return;
                        }

                        // 启用超时设置时进行超时判断
                        if (this.IsEnableTimeOut)
                        {
                            this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> ElapsedMilliseconds = {this.TotalTimeWatch.ElapsedMilliseconds}/{this.TimeOut}.");
                            if (this.TotalTimeWatch.ElapsedMilliseconds >= this.TimeOut)
                            {
                                // 超时
                                this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 超时退出。");
                                this.EndLoopInternal(ConditionLooperResult.TimeOut);
                                return;
                            }
                        }

                        if (this.IsEnableLoopTimes)
                        {
                            // 调用次数 +1
                            this.ExecuteTime++;

                            this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> ExecuteTime = {this.ExecuteTime}/{this.LoopTimes}.");
                            if (this.ExecuteTime >= this.LoopTimes)
                            {
                                // 超过次数限制
                                this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 超过次数限制退出。");
                                this.EndLoopInternal(ConditionLooperResult.TimesLimited);
                                return;
                            }
                        }
                    });
                }
                else
                {
                    this.Log($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 停止循环任务....");
                    this.Stop();
                }
            }
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
        /// 记录调用次数
        /// </summary>
        private int ExecuteTime = 0;
        /// <summary>
        /// 是否启用异步调用
        /// </summary>
        private bool IsAsyncCall = false;
        /// <summary>
        /// 等待信号，false - 默认等待；ture - 默认放行
        /// </summary>
        private AutoResetEvent WaitSignal = new AutoResetEvent(false);
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
        public ConditionLooperResult Run(Func<bool> @checker)
        {
            this.IsAsyncCall = false;
            this.WorkCheck = @checker;
            this.Enable = true;

            // 等待放行信号
            this.WaitSignal.WaitOne();

            return this.Result;
        }
        /// <summary>
        /// 执行检查任务(异步调用)
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="callback"></param>
        public void RunAsync(Func<bool> @checker, Action<ConditionLooperResult> callback)
        {
            this.IsAsyncCall = true;
            this.WorkCheck = @checker;
            this.EndLoop = callback;
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
                this.WaitSignal.Dispose();
                this.WaitSignal = null;
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
        /// 停止任务
        /// </summary>
        protected virtual void Stop()
        {
            this.IsRunning = false;

            // 停止计时器
            this.StopTimer();

            if (null != this.TotalTimeWatch)
            {
                this.TotalTimeWatch.Stop();
            }

            // 信号放行
            if (!this.IsAsyncCall)
            {
                this.WaitSignal.Set();
            }
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
            this.JobTimer = new System.Threading.Timer(callback, null, (this.IsFirstDelay ? this.Interval : 0), this.Interval);
        }
        /// <summary>
        /// 重置运行状态
        /// </summary>
        protected virtual void Reset()
        {
            this.Result = ConditionLooperResult.None;

            if (!this.IsAsyncCall)
            {
                // 设置为非终止，导致外部线程阻塞，等待任务完成或超时
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

            if (this.IsEnableLoopTimes)
            {
                this.ExecuteTime = 0;
            }
            this.IsRunning = true;
        }
        /// <summary>
        /// 触发结束事件
        /// </summary>
        /// <param name="result">结果</param>
        protected virtual void EndLoopInternal(ConditionLooperResult result)
        {
            // 保存结果
            this.Result = result;
            // 执行停止
            this.Stop();

            // 触发结束事件
            this.EndLoop?.Invoke(result);
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="content"></param>
        [Conditional("DEBUG")]
        protected virtual void Log(string content)
        {
            LogProvider.Log.Debug(content);

            //System.Diagnostics.Debug.WriteLine(content);
            //Console.WriteLine(content);
        }
    }

    internal interface IConditionLooper
    {
        /// <summary>
        /// 启用/停用
        /// </summary>
        bool Enable { get; set; }
        /// <summary>
        /// 时间间隔，默认100毫秒（以毫秒为单位）
        /// </summary>
        int Interval { get; set; }
        /// <summary>
        /// 首次调用是否延迟<see cref="Interval"/>指定的时间间隔，默认为<seealso cref="false"/>
        /// </summary>
        bool IsFirstDelay { get; set; }
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
        event Action<ConditionLooperResult> EndLoop;
        /// <summary>
        /// 周期循环的检查任务，返回<see cref="true"/>时则会终止循环检测
        /// </summary>
        event Func<bool> WorkCheck;

        /// <summary>
        /// 释放资源
        /// </summary>
        void Dispose();
        /// <summary>
        /// 执行检查任务（同步调用）
        /// </summary>
        /// <param name="checker"></param>
        /// <returns></returns>
        ConditionLooperResult Run(Func<bool> checker);
        /// <summary>
        /// 执行检查任务(异步调用)
        /// </summary>
        /// <param name="checker"></param>
        /// <param name="callback"></param>
        void RunAsync(Func<bool> checker, Action<ConditionLooperResult> callback);
    }
    /// <summary>
    /// 循环调用结果
    /// </summary>
    public enum ConditionLooperResult
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
