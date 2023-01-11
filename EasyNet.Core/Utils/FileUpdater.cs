
// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Utils
// 文件名称：FileUpdater
// 创 建 者：lanwah
// 创建日期：2022/8/5 11:20:35
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

using EasyNet.Core.IO;
using EasyNet.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Environment;

namespace EasyNet.Core.Utils
{
    /// <summary>
    /// 文件更新器
    /// </summary>
    public class FileUpdater
    {
        /// <summary>
        /// 更新参数
        /// </summary>
        public List<FileUpdateArgs> UpdateArgList
        {
            get; private set;
        }
        /// <summary>
        /// 更新进度
        /// </summary>
        public IProgress<string> UpdateProgress
        {
            get; private set;
        }
        private bool updateLimitToFile = true;
        /// <summary>
        /// 仅更新文件，默认为 true
        /// </summary>
        public bool UpdateLimitToFile
        {
            get => this.updateLimitToFile;
            set => this.updateLimitToFile = value;
        }
        private bool logOn = false;
        /// <summary>
        /// 启用日志，默认为 false
        /// </summary>
        public bool LogOn
        {
            get => this.logOn;
            set => this.logOn = value;
        }
        /// <summary>
        /// 成功的数量
        /// </summary>
        public int SuccessCount
        {
            get;
            private set;
        }
        /// <summary>
        /// 失败/跳过的数量
        /// </summary>
        public int FailedCount
        {
            get;
            private set;
        }
        /// <summary>
        /// 忽略的文件或文件夹数量
        /// </summary>
        public int IgnoreCount
        {
            get;
            private set;
        }
        /// <summary>
        /// 更新延迟，单位毫秒，用于测试，-1时不启用
        /// </summary>
        private readonly int UpdateDelay = -1;
        private readonly FileLog File;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateArgList"></param>
        /// <param name="updateProgress"></param>
        public FileUpdater(List<FileUpdateArgs> updateArgList, IProgress<string> updateProgress)
        {
            this.SetUpdateArgs(updateArgList, updateProgress);
            this.File = new FileLog();
            this.File.MarkFlag = MessageFlag.HeadFlag;
            this.File.Initial();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="updateArgList"></param>
        public FileUpdater(List<FileUpdateArgs> updateArgList) : this(updateArgList, null)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileUpdater() : this(null, null)
        {

        }



        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="updateArgList"></param>
        /// <param name="updateProgress"></param>
        public void SetUpdateArgs(List<FileUpdateArgs> updateArgList, IProgress<string> updateProgress = null)
        {
            UpdateArgList = updateArgList;
            UpdateProgress = updateProgress;
        }
        /// <summary>
        /// 更新文件
        /// </summary>
        public void Update()
        {
            if (null == this.UpdateArgList)
            {
                throw new Exception("请从构造函数中或调用SetUpdateArgs接口参数化更新目录信息。");
            }

            this.ClearCount();
            foreach (var arg in this.UpdateArgList)
            {
                this.Update(arg.SourceFilePath, arg.TargetFilePath, arg.IngoreFiles);
            }

            this.ReportEnd();
        }
        /// <summary>
        /// 异步更新文件
        /// </summary>
        public void UpdateAsync()
        {
            if (null == this.UpdateArgList)
            {
                throw new Exception("请从构造函数中或调用SetUpdateArgs接口参数化更新目录信息。");
            }

            Task.Run(() =>
            {

                this.ClearCount();
                foreach (var arg in this.UpdateArgList)
                {
                    this.Update(arg.SourceFilePath, arg.TargetFilePath, arg.IngoreFiles);
                }

                this.ReportEnd();
            });
        }



        /// <summary>
        /// 清除计数信息
        /// </summary>
        private void ClearCount()
        {
            this.SuccessCount = this.FailedCount = this.IgnoreCount = 0;
        }
        /// <summary>
        /// 更新进度信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="logLevel"></param>
        private void ReportProgress(string value, NLog.LogLevel logLevel)
        {
            this.UpdateProgress?.Report(value);

            if (this.logOn)
            {
                if (logLevel == NLog.LogLevel.Fatal)
                {
                    this.File.Fatal(value);
                }
                else if (logLevel == NLog.LogLevel.Error)
                {
                    this.File.Error(value);
                }
                else if (logLevel == NLog.LogLevel.Warn)
                {
                    this.File.Warn(value);
                }
                else if (logLevel == NLog.LogLevel.Info)
                {
                    this.File.Info(value);
                }
                else if (logLevel == NLog.LogLevel.Trace)
                {
                    this.File.Trace(value);
                }
                else
                {
                    this.File.Debug(value);
                }
            }
        }
        /// <summary>
        /// 记录最终日志
        /// </summary>
        private void ReportEnd()
        {
            this.ReportProgress($"成功处理 {this.SuccessCount} 个文件，失败/跳过处理 {this.FailedCount} 个文件，忽略了 {this.IgnoreCount} 个文件。{NewLine}{NewLine}{NewLine}", NLog.LogLevel.Info);
        }
        /// <summary>
        /// 以目标文件夹中的内容为目标用源文件夹中的内容覆盖目标文件夹内容
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="ingoreFiles"></param>
        /// <returns></returns>
        private FunStatus Update(string sourceFilePath, string targetFilePath, List<string> ingoreFiles)
        {
            targetFilePath.NotNullOrEmptyCheck(nameof(targetFilePath));
            targetFilePath.NotNullOrEmptyCheck(nameof(sourceFilePath));

            this.ReportProgress($"准备处理 {targetFilePath} 目录下的文件....", NLog.LogLevel.Info);

            if (!Directory.Exists(targetFilePath))
            {
                return FunStatus.Failure("targetFilePath(目标文件夹)不存在！");
            }
            if (!Directory.Exists(sourceFilePath))
            {
                return FunStatus.Failure("sourceFilePath(源文件夹)不存在！");
            }

            // 获取目标文件夹中的文件内容
            var targetFiles = targetFilePath.GetFilesAndDirectories();
            if (targetFiles.HasNoData())
            {
                return FunStatus.Failure($"目标文件夹 {targetFilePath} 为空！");
            }

            // 获取源文件夹内容
            var sourceFiles = sourceFilePath.GetFilesAndDirectories();
            if (sourceFiles.HasNoData())
            {
                return FunStatus.Failure($"源文件夹 {sourceFilePath} 为空！");
            }

            // 待处理的文件和文件夹数量
            var fileCount = targetFiles.Count;
            this.ReportProgress($"待处理文件 {fileCount} 个，开始处理。", NLog.LogLevel.Info);

            // 目标文件夹中存在文件，用源文件夹的文件覆盖目标文件夹中同名文件
            var overwriteCount = 0;
            var dealIndex = 1;

            foreach (var filePath in targetFiles)
            {
                // 获取末级文件夹或文件名
                var fileName = filePath.Replace(targetFilePath, "");

                this.ReportProgress($"正在处理 {fileName} 文件({dealIndex++}/{fileCount})....", NLog.LogLevel.Trace);
                this.Delay();

                // 处理忽略的文件
                if (ingoreFiles.Contains(fileName))
                {
                    this.ReportProgress($"忽略的文件 {fileName} 不处理。", NLog.LogLevel.Info);
                    this.IgnoreCount++;
                    this.Delay();
                    continue;
                }

                var sourceFileName = $"{sourceFilePath}{fileName}";
                if (!sourceFiles.Contains(sourceFileName))
                {
                    // 源文件夹不存在文件或文件夹，则不处理
                    this.ReportProgress($"源文件夹不存在文件或文件夹({sourceFileName})，不处理。", NLog.LogLevel.Error);
                    this.FailedCount++;
                    this.Delay();
                    continue;
                }

                // 判断是否为文件
                if (filePath.IsDirectory())
                {
                    if (this.UpdateLimitToFile)
                    {
                        this.ReportProgress($"目录文件夹({filePath})，不处理。", NLog.LogLevel.Trace);
                        this.FailedCount++;
                        this.Delay();
                        continue;
                    }
                    else
                    {
                        // 递归处理，处理的结果会记录在全局的统计参数FailedCount和SuccessCount中
                        this.Update(sourceFileName, filePath, ingoreFiles);
                        continue;
                    }
                }

                // 源文件夹存在文件，则用源文件夹中的文件覆盖目标文件夹中同名文件
                System.IO.File.Copy(sourceFileName, filePath, true);

                this.ReportProgress($"{fileName} 文件成功更新。", NLog.LogLevel.Trace);
                overwriteCount++;
                this.SuccessCount++;
                this.Delay();
            }

            this.ReportProgress($"{targetFilePath} 目录下的文件处理完毕。", NLog.LogLevel.Info);
            return FunStatus.Success($"成功更新 {overwriteCount} 个文件。");
        }
        /// <summary>
        /// 延迟，以便显示更新进度
        /// </summary>
        private void Delay()
        {
            if (this.UpdateDelay > 0)
            {
                System.Threading.Thread.Sleep(this.UpdateDelay);
            }
        }
    }

    /// <summary>
    /// 文件更新参数
    /// </summary>
    public struct FileUpdateArgs
    {
        /// <summary>
        /// 源文件目录
        /// </summary>
        public string SourceFilePath
        {
            get; set;
        }
        /// <summary>
        /// 目标文件目录
        /// </summary>
        public string TargetFilePath
        {
            get; set;
        }
        /// <summary>
        /// 忽略的文件
        /// </summary>
        public List<string> IngoreFiles
        {
            get; set;
        }



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="ingoreFiles"></param>
        public FileUpdateArgs(string sourceFilePath, string targetFilePath, List<string> ingoreFiles)
        {
            this.SourceFilePath = sourceFilePath;
            this.TargetFilePath = targetFilePath;
            this.IngoreFiles = ingoreFiles;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFilePath"></param>
        public FileUpdateArgs(string sourceFilePath, string targetFilePath) : this(sourceFilePath, targetFilePath, new List<string>())
        {

        }
    }
}
