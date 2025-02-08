using EasyNet.Extensions;
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

        /// <summary>
        /// 获取已安装的程序信息
        /// </summary>
        /// <returns></returns>
        public static List<InstalledPrograms> GetInstalledPrograms()
        {
            var installedPrograms = new List<InstalledPrograms>();

            // 32-bit applications on both 32-bit and 64-bit systems
            var subKeyPaths = new string[]
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string subKeyPath in subKeyPaths)
            {
                using (RegistryKey uninstallKey = Registry.LocalMachine.OpenSubKey(subKeyPath))
                {
                    if (uninstallKey != null)
                    {
                        foreach (string subKeyName in uninstallKey.GetSubKeyNames())
                        {
                            using (RegistryKey subKey = uninstallKey.OpenSubKey(subKeyName))
                            {
                                if (subKey != null)
                                {
                                    var displayName = subKey.GetValue("DisplayName");
                                    var displayVersion = subKey.GetValue("DisplayVersion");
                                    var installDate = subKey.GetValue("InstallDate");

                                    if (displayName != null)
                                    {
                                        if (installedPrograms.Exists(x => x.DisplayName.Compare(displayName.ToString())))
                                        {
                                            continue;
                                        }

                                        installedPrograms.Add(new InstalledPrograms
                                        {
                                            DisplayName = displayName.ToString(),
                                            DisplayVersion = displayVersion?.ToString(),
                                            InstallDate = installDate?.ToString(),
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return installedPrograms;
        }
    }
}
