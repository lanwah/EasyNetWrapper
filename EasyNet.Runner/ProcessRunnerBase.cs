using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Runner
{
    /// <summary>
    /// 程序启动器基类
    /// </summary>
    public class ProcessRunnerBase
    {
        /// <summary>
        /// 可执行文件路径或名称
        /// </summary>
        public string ExecutablePath { get; }
        /// <summary>
        /// 可执行文件的工作目录
        /// </summary>
        public string WorkingDirectory { get; }
        /// <summary>
        /// 输出重定向，默认为true，开启
        /// </summary>
        public bool RedirectStandardOutput { get; set; } = true;
        /// <summary>
        /// 不创建窗口，默认为true，不创建
        /// </summary>
        public bool CreateNoWindow { get; set; } = true;
        /// <summary>
        /// 等待程序退出，默认为true，等待
        /// </summary>
        public bool WaitForExit { get; set; } = true;
        /// <summary>
        /// 输出命令参数
        /// </summary>
        public bool ConsoleCommand { get; set; } = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executablePath">可执行文件路径或名称</param>
        /// <param name="workingDirectory">可执行文件的工作目录</param>
        public ProcessRunnerBase(string executablePath, string workingDirectory = default)
        {
            this.ExecutablePath = executablePath ?? throw new ArgumentNullException(nameof(executablePath));
            this.WorkingDirectory = workingDirectory ?? Path.GetDirectoryName(executablePath);
        }

        /// <summary>
        /// 输出命令参数
        /// </summary>
        /// <param name="startInfo"></param>
        protected virtual void ConsoleCommandDetail(ProcessStartInfo startInfo)
        {
            Console.WriteLine($"============================== COMMAND DETAIL ==============================");
            Console.WriteLine($"OSVersion: {Environment.OSVersion}");
            Console.WriteLine($"Version: {Environment.Version}");
            Console.WriteLine($"Command EXE: {startInfo.FileName}");
            if (startInfo.Arguments != null)
            {
                Console.WriteLine($"Arguments: {startInfo.Arguments}");
            }
#if !NETFRAMEWORK
            else if (startInfo.ArgumentList != null)
            {
                Console.WriteLine($"Arguments: {string.Join(" ", startInfo.ArgumentList)}");
            }
#endif
            Console.WriteLine($"WorkingDirectory: {startInfo.WorkingDirectory}");
            Console.WriteLine($"RedirectStandardInput: {startInfo.RedirectStandardInput}");
            Console.WriteLine($"RedirectStandardOutput: {startInfo.RedirectStandardOutput}");
            Console.WriteLine($"CreateNoWindow: {startInfo.CreateNoWindow}");
            Console.WriteLine($"UseShellExecute: {startInfo.UseShellExecute}");
            Console.WriteLine($"=============================================================================");
            Console.WriteLine();
        }
        /// <summary>
        /// 设置启动参数
        /// </summary>
        /// <param name="startInfoSetting"></param>
        /// <param name="startInfo"></param>
        protected virtual void OnStartInfoSetting(Action<ProcessStartInfo> startInfoSetting, ProcessStartInfo startInfo)
        {
            if (startInfoSetting is null)
            {
                return;
            }

            startInfoSetting.Invoke(startInfo);
        }
        /// <summary>
        /// 命令输入事件处理
        /// </summary>
        /// <param name="process"></param>
        /// <param name="standardInput"></param>
        protected virtual void OnStandardInput(Process process, Action<StreamWriter> standardInput)
        {
            if (standardInput is null)
            {
                return;
            }
            if (process is null)
            {
                return;
            }
            if (!process.StartInfo.RedirectStandardInput)
            {
                return;
            }

            // 处理输入，写入命令
            using (var writer = process.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    standardInput(writer);
                }
            }
        }
        /// <summary>
        /// 命令输出事件处理
        /// </summary>
        /// <param name="process"></param>
        /// <param name="standardOutput"></param>
        protected virtual void OnStandardOutput(Process process, Action<StreamReader> standardOutput)
        {
            if (standardOutput is null)
            {
                return;
            }
            if (process is null)
            {
                return;
            }
            if (!process.StartInfo.RedirectStandardOutput)
            {
                return;
            }

            // 处理输出，获取结果
            using (var reader = process.StandardOutput)
            {
                if (reader.BaseStream.CanRead)
                {
                    standardOutput(reader);
                }
            }
        }
        /// <summary>
        /// 等待程序退出事件处理
        /// </summary>
        /// <param name="process"></param>
        protected virtual void OnWaitForExit(Process process)
        {
            if (process is null)
            {
                return;
            }

            if (this.WaitForExit)
            {
                process.WaitForExit();
            }
        }
        /// <summary>
        /// 获取ProcessStartInfo
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        protected virtual ProcessStartInfo GetProcessStartInfo(params string[] arguments)
        {
            ProcessStartInfo startInfo = null;

            if ((arguments != null) && (arguments.Length > 0))
            {
                if (arguments.Length == 1)
                {
                    startInfo = new ProcessStartInfo(ExecutablePath, arguments[0]);
                }
                else
                {
#if NETFRAMEWORK
                    throw new NotSupportedException("Multiple arguments are not supported in .NET Framework.");
#else
                    startInfo = new ProcessStartInfo(ExecutablePath, arguments);
#endif
                }
            }
            if (startInfo is null)
            {
                startInfo = new ProcessStartInfo(ExecutablePath);
            }

            startInfo.CreateNoWindow = this.CreateNoWindow;
            startInfo.RedirectStandardOutput = this.RedirectStandardOutput;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = WorkingDirectory;

            return startInfo;
        }
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="startInfoSetting">ProcessStartInfo参数设置</param>
        /// <param name="standardInput">输入流处理</param>
        /// <param name="standardOutput">输出流处理</param>
        /// <param name="arguments">程序运行参数</param>
        /// <returns></returns>
        protected ProcessRunnerBase Execute(Action<ProcessStartInfo> startInfoSetting, Action<StreamWriter> standardInput, Action<StreamReader> standardOutput, params string[] arguments)
        {
            var startInfo = this.GetProcessStartInfo(arguments);
            // 设置启动参数
            this.OnStartInfoSetting(startInfoSetting, startInfo);
            var process = new Process
            {
                StartInfo = startInfo,
            };
            // 输出命令参数
            this.ConsoleCommandDetail(startInfo);
            process.Start();
            // 处理输入
            this.OnStandardInput(process, standardInput);
            // 等待程序退出
            this.OnWaitForExit(process);
            // 处理输出
            this.OnStandardOutput(process, standardOutput);
            return this;
        }

        /// <summary>
        /// 命令输入事件处理
        /// </summary>
        /// <param name="process"></param>
        /// <param name="standardInput"></param>
        protected virtual async Task OnStandardInputAsync(Process process, Func<StreamWriter, Task> standardInput)
        {
            if (standardInput is null)
            {
                return;
            }
            if (process is null)
            {
                return;
            }
            if (!process.StartInfo.RedirectStandardInput)
            {
                return;
            }

            // 处理输入，写入命令
            using (var writer = process.StandardInput)
            {
                if (writer.BaseStream.CanWrite)
                {
                    await standardInput(writer);
                }
            }
        }
        /// <summary>
        /// 等待程序退出事件处理
        /// </summary>
        /// <param name="process"></param>
        protected virtual async Task OnWaitForExitAsync(Process process)
        {
            if (process is null)
            {
                return;
            }

            if (this.WaitForExit)
            {
#if !NETFRAMEWORK
                await process.WaitForExitAsync();
#else
                await Task.Delay(1);
#endif
            }
        }
        /// <summary>
        /// 命令输出事件处理
        /// </summary>
        /// <param name="process"></param>
        /// <param name="standardOutput"></param>
        protected virtual async Task OnStandardOutputAsync(Process process, Func<StreamReader, Task> standardOutput)
        {
            if (standardOutput is null)
            {
                return;
            }
            if (process is null)
            {
                return;
            }
            if (!process.StartInfo.RedirectStandardOutput)
            {
                return;
            }

            // 处理输出，获取结果
            using (var reader = process.StandardOutput)
            {
                if (reader.BaseStream.CanRead)
                {
                    await standardOutput(reader);
                }
            }
        }
        /// <summary>
        /// 异步执行程序
        /// </summary>
        /// <param name="startInfoSetting">ProcessStartInfo参数设置</param>
        /// <param name="standardInput">输入流处理</param>
        /// <param name="standardOutput">输出流处理</param>
        /// <param name="arguments">程序运行参数</param>
        /// <returns></returns>
        protected async Task<ProcessRunnerBase> ExecuteAsync(Action<ProcessStartInfo> startInfoSetting, Func<StreamWriter, Task> standardInput, Func<StreamReader, Task> standardOutput, params string[] arguments)
        {
            var startInfo = this.GetProcessStartInfo(arguments);
            // 设置启动参数
            this.OnStartInfoSetting(startInfoSetting, startInfo);
            var process = new Process
            {
                StartInfo = startInfo,
            };
            // 输出命令参数
            this.ConsoleCommandDetail(startInfo);
            process.Start();
            // 处理输入
            await this.OnStandardInputAsync(process, standardInput);
            // 等待程序退出
            await this.OnWaitForExitAsync(process);
            // 处理输出
            await this.OnStandardOutputAsync(process, standardOutput);
            return this;
        }
    }
}
