using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebBrowser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(txtUrl.Text);
        }

        private void btnV_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.webBrowser1.Version.ToString());
        }

        /// <summary>
        /// 未测试，待验证
        /// </summary>
        /// <param name="major"></param>
        private void UseLatestIE(int major)
        {
            try
            {
                int num = 0;
                switch (major)
                {
                    case 7:
                        // 0x1B58
                        num = 7000;
                        break;
                    case 8:
                        // 0x22B8
                        num = 8888;
                        break;
                    case 9:
                        // 0x270F
                        num = 9999;
                        break;
                    case 10:
                        // 0x2711
                        num = 10001;
                        break;
                    case 11:
                        // 0x2AF9
                        num = 11001;
                        break;
                }

                if (num != 0)
                {
                    using (var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true))
                    {
                        if (registryKey != null)
                        {
                            registryKey.SetValue(Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName), num, RegistryValueKind.DWord);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}
