using EasyNet.Extensions;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace EasyNet.InstallChecker
{
    /// <summary>
    /// 通过文件来判断是否安装
    /// </summary>
    public class FileInstallCheck : InstallChecker
    {
        /// <summary>
        /// 安装程序的目录
        /// </summary>
        public virtual string FileDirectory { get; set; }
        /// <summary>
        /// 安装程序的文件名
        /// </summary>
        public virtual List<string> FileNames { get; set; }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileDirectory"></param>
        /// <param name="fileNames"></param>
        public FileInstallCheck(string fileDirectory, List<string> fileNames)
        {
            this.SetFileDirectory(fileDirectory);
            this.SetFileNames(fileNames);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileDirectory"></param>
        public FileInstallCheck(string fileDirectory) : this(fileDirectory, new List<string>())
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileInstallCheck() : this(string.Empty, new List<string>())
        {

        }


        /// <summary>
        /// 通过文件检查程序是否已安装
        /// </summary>
        /// <returns></returns>
        protected override bool IsInstalledInternal()
        {
            if (!this.FileDirectory.IsDirectoryExists())
            {
                return false;
            }
            if (this.FileNames.HasNoData())
            {
                return true;
            }

            foreach (var fileName in this.FileNames)
            {
                var filePath = System.IO.Path.Combine(this.FileDirectory, fileName);
                if (!filePath.IsFileExists())
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 设置安装程序的目录
        /// </summary>
        /// <param name="fileDirectory"></param>
        protected void SetFileDirectory(string fileDirectory)
        {
            this.FileDirectory = fileDirectory;
        }
        /// <summary>
        /// 设置安装程序的文件名
        /// </summary>
        /// <param name="fileNames"></param>
        protected void SetFileNames(List<string> fileNames)
        {
            this.FileNames = fileNames;
        }
    }
}
