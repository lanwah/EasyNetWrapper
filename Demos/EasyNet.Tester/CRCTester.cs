using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;
using EasyNet.Security;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Tester
// CLR版本：4.0.30319.42000
// 运行要求：4.8
// 文件名称：CRCTester.cs
// 创建用户：lanwah
// 创建日期：2024/8/28 14:15:54
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Tester
{
    public class CRCTester
    {
        public static void Run()
        {
            //var sourceList = new List<string> { "123456789", "134689", "1346598" };

            //foreach (var line in sourceList)
            //{
            //    Console.WriteLine($"Source = {line}");
            //    var buffer = Encoding.ASCII.GetBytes(line);
            //    var crc8 = buffer.ComputeCrc8();
            //    var crc32 = buffer.ComputeCrc32();

            //    // 格式化输出结果，ToString 都加上了 ToUpper 转为大写
            //    Console.WriteLine($"CRC8 MAXIM = {Convert.ToString(crc8, 16).ToUpper()}");// A1
            //    Console.WriteLine($"CRC32 = {Convert.ToString(crc32, 16).ToUpper()}");// CBF43926
            //    Console.WriteLine($"CRC16 Ansi = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Ansi), 16).ToUpper()}");// FEE8
            //    Console.WriteLine($"CRC16 Arc = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Arc), 16).ToUpper()}");// BB3D CRC-16 CRC16/ARC
            //    Console.WriteLine($"CRC16 Maxim = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Maxim), 16).ToUpper()}");// 44C2 CRC16/Maxim
            //    Console.WriteLine($"CRC16 Usb = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Usb), 16).ToUpper()}");// B4C8 CRC16/Usb
            //    Console.WriteLine($"CRC16 ModBus = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.ModBus), 16).ToUpper()}");// 4B37 CRC16/MODBUS
            //    Console.WriteLine($"CRC16 CCITT_FALSE = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.CCITT_FALSE), 16).ToUpper()}");// 29B1 CRC16/CCITT-FALSE
            //    Console.WriteLine($"CRC16 XModem = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.XModem), 16).ToUpper()}");// 31C3 CRC16/XMODEM
            //    Console.WriteLine($"CRC16 CCITT_0x1D0F = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.CCITT_0x1D0F), 16).ToUpper()}");// E5CC
            //    Console.WriteLine($"CRC16 Kermit = {Convert.ToString(buffer.ComputeCrc16(Crc16Type.Kermit), 16).ToUpper()}");// 2189
            //    Console.WriteLine();
            //}

            var hexString = "03 0F 00 04 00 00 00";
            hexString = hexString.Replace(" ", "");
            var buffer = hexString.FromHexString("");
            var crc16 = buffer.ComputeCrc16(Crc16Type.ModBus);
            // 4位的 CRC16 值
            var crc16Hex = crc16.ToString("X4");
        }
    }
}
