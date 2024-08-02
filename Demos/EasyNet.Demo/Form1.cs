using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyNet.Runner;

namespace EasyNet.Demo
{
    /// <summary>
    /// EasyNet.Runner 测试程序
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void BtnRun_Click(object sender, EventArgs e)
        {
            // 通过cmd.exe /c 执行cmd命令
            // /C 执行字符串指定的命令然后终断
            // /K 执行字符串指定的命令但保留
            // https://bbs.csdn.net/topics/270017090

            Debug.WriteLine("btnRun_Click Begin ....");
            var runner = new EasyNet.Runner.ProcessRunner("cmd");
            //runner.CreateNoWindow = false;
            //runner.RedirectStandardOutput = false;
            var result = await runner.ExecuteAsync($"/c dir");
            Debug.WriteLine(result);
            Debug.WriteLine("btnRun_Click End.");
        }
        /// <summary>
        /// 同步执行Cmd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            // 通过cmd.exe /c 执行cmd命令
            // /C 执行字符串指定的命令然后终断
            // /K 执行字符串指定的命令但保留
            // https://bbs.csdn.net/topics/270017090

            Debug.WriteLine("btnRun_Click Begin ....");
            var runner = new EasyNet.Runner.ProcessRunner("cmd");
            //runner.CreateNoWindow = false;
            //runner.RedirectStandardOutput = false;
            var result = runner.Execute($"/c dir");
            Debug.WriteLine(result);
            Debug.WriteLine("btnRun_Click End.");
        }
        /// <summary>
        /// 同步执行Cmd，并显示执行窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            // 通过cmd.exe /c 执行cmd命令
            // /C 执行字符串指定的命令然后终断
            // /K 执行字符串指定的命令但保留
            // https://bbs.csdn.net/topics/270017090

            Debug.WriteLine("btnRun_Click Begin ....");
            var runner = new EasyNet.Runner.ProcessRunner("cmd")
            {
                CreateNoWindow = false,
                RedirectStandardOutput = false
            };
            var result = runner.Execute($"/k dir");
            Debug.WriteLine(result);
            Debug.WriteLine("btnRun_Click End.");
        }

        /// <summary>
        /// 异步执行Git
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button5_Click(object sender, EventArgs e)
        {
            // 通过git.exe执行git命令
            Debug.WriteLine("button5_Click Begin ....");
            var runner = new EasyNet.Runner.ProcessRunner("git", @"E:\Ewell\StandNuringProj\BinzhouNew\BinzhouNew_debug");
            var result = await runner.ExecuteAsync("status");
            Debug.WriteLine(result);
            Debug.WriteLine("button5_Click End.");
        }
        /// <summary>
        /// 同步执行Git
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_Click(object sender, EventArgs e)
        {
            // 通过git.exe执行git命令
            Debug.WriteLine("button4_Click Begin ....");
            var runner = new EasyNet.Runner.ProcessRunner("git", @"E:\Ewell\StandNuringProj\BinzhouNew\BinzhouNew_debug");
            var result = runner.Execute("status");
            Debug.WriteLine(result);
            Debug.WriteLine("button4_Click End.");
        }

        /// <summary>
        /// 同步执行Git，并显示执行窗口
        /// 【有问题，没实现！！！】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button3_Click(object sender, EventArgs e)
        {
            // 通过git.exe执行git命令
            Debug.WriteLine("button3_Click Begin ....");
            var runner = new EasyNet.Runner.ProcessRunner("git", @"E:\Ewell\StandNuringProj\BinzhouNew\BinzhouNew_debug")
            {
                CreateNoWindow = false,
                RedirectStandardOutput = false
            };
            var result = runner.Execute("status");
            //var result = runner.Execute("-p");
            Debug.WriteLine(result);
            Debug.WriteLine("button3_Click End.");

            //// 通过cmd.exe /c 执行cmd命令
            //// /C 执行字符串指定的命令然后终断
            //// /K 执行字符串指定的命令但保留
            //// https://bbs.csdn.net/topics/270017090

            //Debug.WriteLine("button3_Click Begin ....");
            //var runner = new EasyNet.Runner.ProcessRunner("cmd");
            //runner.CreateNoWindow = false;
            //runner.RedirectStandardOutput = false;
            //var result = runner.Execute($"/k git status");
            //Debug.WriteLine(result);
            //Debug.WriteLine("button3_Click End.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
