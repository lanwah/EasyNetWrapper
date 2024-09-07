using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Security;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Tester
// CLR版本：4.0.30319.42000
// 运行要求：4.8
// 文件名称：DESCryptoTester.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 16:21:02
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
    internal class DESCryptoTester
    {
        public static void Run()
        {
            var random = new Random();

            // 密钥
            var rgbKey = new byte[8];
            // 明文数据
            var encryptData = new byte[8];
            // 密文数据
            byte[] encryptedData;

            random.NextBytes(rgbKey);
            random.NextBytes(encryptData);

            // 加密
            Console.WriteLine($"rgbKey = {rgbKey.ToHexString()}");
            Console.WriteLine($"encryptData = {encryptData.ToHexString()}");
            encryptedData = encryptData.DESEncrypt(rgbKey);
            Console.WriteLine($"encryptedData = {encryptedData.ToHexString()}");
            // 解密
            encryptData = encryptedData.DESDecrypt(rgbKey);
            Console.WriteLine($"encryptData = {encryptData.ToHexString()}");

            var sRgbKey = "11000000";
            var sRgbIV = "00000000";
            var sEncryptData = "888888";


            Console.WriteLine($"sRgbKey = {sRgbKey}");
            Console.WriteLine($"sRgbIV = {sRgbIV}");
            Console.WriteLine($"sEncryptData = {sEncryptData}");
            // 加密
            string sEncryptedData = sEncryptData.DESEncrypt(CipherMode.CBC, PaddingMode.PKCS7, sRgbKey, sRgbIV);
            Console.WriteLine($"sEncryptedData = {sEncryptedData}");

            // 解密
            sEncryptData = sEncryptedData.DESDecrypt(CipherMode.CBC, PaddingMode.PKCS7, sRgbKey, sRgbIV);
            Console.WriteLine($"sEncryptData = {sEncryptData}");
        }

        public static void Run3DES()
        {
            var random = new Random();

            // 密钥
            var drgbKey = new byte[16];
            // 明文数据
            var EncryptData = new byte[8];
            // 密文数据
            byte[] EncryptedData;

            random.NextBytes(drgbKey);
            random.NextBytes(EncryptData);
            drgbKey = "52-AC-BA-E9-4A-63-2A-FC-1A-DB-EA-D2-85-68-ED-D9".FromHexString();
            EncryptData = "E2-8E-22-89-39-01-86-8C".FromHexString(); ;

            // 加密
            Console.WriteLine($"drgbKey = {drgbKey.ToHexString()}");
            Console.WriteLine($"EncryptData = {EncryptData.ToHexString()}");
            EncryptedData = EncryptData.TripleDESEncrypt(drgbKey);
            Console.WriteLine($"EncryptedData = {EncryptedData.ToHexString()}");
            // 解密
            EncryptData = EncryptedData.TripleDESDecrypt(drgbKey);
            Console.WriteLine($"EncryptData = {EncryptData.ToHexString()}");

            // 支付宝的3DES加密解密
            string sKey = drgbKey.ToBase64String();
            string sEncryptData = EncryptData.ToBase64String();

            Console.WriteLine($"sKey = {sKey}");
            Console.WriteLine($"sEncryptData = {sEncryptData}");
            // 加密
            string sEncryptedData = EncryptData.TripleDESEncrypt(CipherMode.CBC, PaddingMode.PKCS7, drgbKey, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }).ToBase64String();
            Console.WriteLine($"sEncryptedData = {sEncryptedData}");
            // 解密
            sEncryptData = sEncryptedData.FromBase64String().TripleDESDecrypt(CipherMode.CBC, PaddingMode.PKCS7, drgbKey, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }).ToBase64String();
            Console.WriteLine($"sEncryptData = {sEncryptData}");
        }
    }
}
