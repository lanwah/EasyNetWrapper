using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.InstallChecker
{
    /// <summary>
    /// 程序是否安装接口
    /// </summary>
    public interface IInstallChecker
    {
        /// <summary>
        /// 是否安装
        /// </summary>
        bool IsInstalled
        {
            get;
        }
    }    
}
