using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.InstallChecker
{
    /// <summary>
    /// 程序是否安装基类
    /// </summary>
    public abstract class InstallChecker : IInstallChecker
    {
        /// <summary>
        /// 是否安装
        /// </summary>
        public bool IsInstalled { get => this.IsInstalledInternal(); }

        /// <summary>
        /// 是否安装内部实现
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsInstalledInternal();

    }
}
