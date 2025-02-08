using EasyNet.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EasyNet.InstallChecker
{
    /// <summary>
    /// 通过程序名称检查程序是否安装
    /// </summary>
    public class ProgramNameInstallCheck : InstallChecker
    {
        /// <summary>
        /// 安装程序名称
        /// </summary>
        public string ProgramName { get; set; }
        /// <summary>
        /// 是否包含
        /// </summary>
        public bool IsContains { get; set; }



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="programName"></param>
        public ProgramNameInstallCheck(string programName)
        {
            this.SetProgramName(programName);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProgramNameInstallCheck() : this(string.Empty)
        {

        }


        /// <summary>
        /// 是否安装内部实现
        /// </summary>
        /// <returns></returns>
        protected override bool IsInstalledInternal()
        {
            if (this.ProgramName.IsNullOrEmpty())
            {
                return false;
            }

            // 先获取已安装的程序信息
            var installedPrograms = GetInstalledPrograms();
            if (installedPrograms.HasNoData())
            {
                return false;
            }

            // 根据 IsContains 判断是否包含，还是完全相等
            var isExist = installedPrograms.Exists(x => x.DisplayName.IsEqual(ProgramName, this.IsContains));
            return isExist;
        }

        /// <summary>
        /// 设置安装程序名称
        /// </summary>
        /// <param name="programName"></param>
        public void SetProgramName(string programName)
        {
            ProgramName = programName;
        }        
    }
}
