using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Test
// 文件名称：Form2
// 创 建 者：lanwah
// 创建日期：2022/3/12 13:45:33
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private delegate void SetTextHandle(string text);

        private void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                this.textBox1.Invoke(new SetTextHandle(SetText), text);
            }
            else
            {
                this.textBox1.Text = text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> 启动控制台");
            var i = 0;
            ConditionMonitor.WaitAsync(() =>
            {
                i++;
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {i}.");
                if (i >= 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (result) =>
            {
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {result}.");
                this.SetText(result.ToString());
            }, 1000, 5000);



            //var result = ConditionMonitor.Wait(() =>
            //{
            //    i++;
            //    LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {i}.");
            //    if (i >= 10)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}, 1000, 5000);
            //this.textBox1.Text = result.ToString();
            //LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> {result}.");


            //PrintA();
        }


        private const int LoopTimes = 10;
        /// <summary>
        /// 同步调用
        /// </summary>
        private static void PrintA()
        {
            int i = 0;
            while (true)
            {
                i++;
                LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> A {i}");
                System.Threading.Thread.Sleep(1000);

                if (i >= LoopTimes)
                {
                    break;
                }
            }

            LogProvider.Log.Debug($"{string.Format("{0,-4}", System.Threading.Thread.CurrentThread.ManagedThreadId)} -> A end.");
        }
    }
}
