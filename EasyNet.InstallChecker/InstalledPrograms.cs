using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.InstallChecker
{
    /// <summary>
    /// 安装的程序信息
    /// </summary>
    public class InstalledPrograms
    {
        /// <summary>
        /// 程序名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 程序版本
        /// </summary>
        public string DisplayVersion { get; set; }
        /// <summary>
        /// 安装日期
        /// </summary>
        public string InstallDate { get; set; }
    }
}
