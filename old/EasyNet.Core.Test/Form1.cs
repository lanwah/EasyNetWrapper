using EasyNet.Controls;
using EasyNet.Core.Security.CRC;
using EasyNet.Core.Test.Model;
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
// 文件名称：Form1
// 创 建 者：lanwah
// 创建日期：2021/02/23 16:00:41
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitialControl();
                       
            var line = "134689";
            var buffer = Encoding.ASCII.GetBytes(line);
            var crc8 = buffer.ComputeCrc8();
            var crc32 = buffer.ComputeCrc32();
            //CRC16 = fee8
            //CRC16 = bb3d
            //CRC16 = b4c8
            //CRC16 = b4c8
            //CRC16 = 4b37
            //CRC16 = 31c3
            //CRC16 = 29b1
            //CRC16 = e5cc
            //CRC16 = 2189

            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Ansi), 16)}");// ddbf
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.IVAnsi), 16)}");// f5d5 CRC16/ARC
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Maxim), 16)}");// f5f1 CRC16/Maxim
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Usb), 16)}");// f5f1 CRC16/Usb
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Modbus), 16)}");// f5f1 CRC16/MODBUS
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.XModem), 16)}");// 471f CRC16/XMODEM
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.CCITT_FALSE), 16)}");// 5613 CRC16/CCITT-FALSE
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.CCITT_0x1D0F), 16)}");// b6d1
            Console.WriteLine($"CRC16 = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Kermit), 16)}");// 4efd
        }
    }
}
