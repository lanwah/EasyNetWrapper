using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Runner
{
    /// <summary>
    /// 程序启动器
    /// </summary>
    public class ProcessRunner : ProcessRunnerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="executablePath">可执行文件路径或名称</param>
        /// <param name="workingDirectory">可执行文件的工作目录</param>
        public ProcessRunner(string executablePath, string workingDirectory = default) : base(executablePath, workingDirectory)
        {
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="arguments">程序运行参数</param>
        /// <returns></returns>
        public string Execute(params string[] arguments)
        {
            var returnMsg = string.Empty;
            base.Execute(null, null, (reader) =>
            {
                if (this.RedirectStandardOutput)
                {
                    returnMsg = reader.ReadToEnd();
                }
            }, arguments);

            return returnMsg;
        }
        /// <summary>
        /// 异步执行程序
        /// </summary>
        /// <param name="arguments">程序运行参数</param>
        /// <returns></returns>
        public async Task<string> ExecuteAsync(params string[] arguments)
        {
            var returnMsg = string.Empty;
            await base.ExecuteAsync(null, null, async
                (reader) =>
            {
                returnMsg = await reader.ReadToEndAsync();
            }, arguments);

            return returnMsg;
        }

        ///// <summary>
        ///// 异步执行程序
        ///// </summary>
        ///// <param name="arguments">程序运行参数</param>
        ///// <returns></returns>
        //public async Task<string> ExecuteAsync(params string[] arguments)
        //{
        //    var startInfo = this.GetProcessStartInfo(arguments);
        //    var process = new Process
        //    {
        //        StartInfo = startInfo,
        //    };
        //    process.Start();

        //    await this.OnWaitForExitAsync(process);

        //    var returnMsg = string.Empty;
        //    if (this.RedirectStandardOutput)
        //    {
        //        using (var reader = process.StandardOutput)
        //        {
        //            returnMsg = await reader.ReadToEndAsync();
        //        }
        //    }
        //    return returnMsg;
        //}
    }
}
