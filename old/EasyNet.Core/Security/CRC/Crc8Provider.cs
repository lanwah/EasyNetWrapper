using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyNet.Core.Security.CRC
{
    /// <summary>
    /// CRC8算法实现(CRC-8MAXIM)
    /// </summary>
    public partial class Crc8Provider : CrcProvider<byte>
    {
        /// <summary>
        /// 计算指定字节的CRC8校验码
        /// </summary>
        /// <param name="sourceCRCCode">源CRC8校验码</param>
        /// <param name="number">字节数据</param>
        /// <returns>CRC8校验码</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal override byte ComputeCore(byte sourceCRCCode, byte number)
        {
            return this.CrcTable[sourceCRCCode ^ number];
        }
    }

    /// <summary>
    /// CRC8算法实现(CRC-8MAXIM)
    /// </summary>
    public partial class Crc8Provider
    {
        /// <summary>
        /// 生成多项式
        /// </summary>
        public override string Polynomial => POLYNOMIAL;
        /// <summary>
        /// CRC 8 位校验表
        /// </summary>
        public override byte[] CrcTable => CRC_TABLE;


        /// <summary>
        /// 生成多项式
        /// </summary>
        public static readonly string POLYNOMIAL = "x^8 + x^5 + x^4 + 1";
        /// <summary> 
        /// CRC 8 位校验表 
        /// </summary> 
        public static readonly byte[] CRC_TABLE = new byte[]
        {
            0, 94, 188, 226, 97, 63, 221, 131, 194, 156, 126, 32, 163, 253, 31, 65,
            157 ,195, 33, 127, 252, 162, 64, 30, 95, 1, 227, 189, 62, 96, 130, 220,
            35, 125, 159, 193, 66, 28, 254, 160, 225, 191, 93, 3, 128, 222, 60, 98,
            190, 224, 2, 92, 223, 129, 99, 61, 124, 34, 192, 158, 29, 67, 161, 255,
            70, 24, 250, 164, 39, 121, 155, 197, 132, 218, 56, 102, 229, 187, 89, 7,
            219, 133, 103, 57, 186, 228, 6, 88, 25, 71, 165, 251, 120, 38, 196, 154,
            101, 59, 217, 135, 4, 90, 184, 230, 167, 249, 27, 69, 198, 152, 122, 36,
            248, 166, 68, 26, 153, 199, 37, 123, 58, 100, 134, 216, 91, 5, 231, 185,
            140, 210, 48, 110, 237, 179, 81, 15, 78, 16, 242, 172, 47, 113, 147, 205,
            17, 79, 173, 243, 112, 46, 204, 146, 211, 141, 111, 49, 178, 236, 14, 80,
            175, 241, 19, 77, 206, 144, 114, 44, 109, 51, 209, 143, 12,82,176, 238,
            50, 108, 142, 208, 83, 13, 239, 177, 240, 174, 76, 18, 145, 207, 45, 115,
            202, 148, 118, 40, 171, 245, 23, 73, 8, 86, 180, 234, 105, 55, 213, 139,
            87, 9, 235, 181, 54, 104, 138, 212, 149, 203, 41, 119, 244, 170, 72, 22,
            233, 183, 85, 11, 136, 214, 52, 106, 43, 117, 151, 201, 74, 20, 246, 168,
            116, 42, 200, 150, 21, 75, 169, 247, 182, 232, 10, 84, 215, 137, 107, 53
        };
    }

    /// <summary>
    ///  CRC8算法(CRC-8MAXIM)相关扩展
    /// </summary>
    public static partial class Crc
    {
        /// <summary>
        /// 从字节数组中生成8位CRC校验码（CRC-8MAXIM）
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>CRC8校验码</returns>
        public static byte ComputeCrc8(this byte[] buffer)
        {
            return (new Crc8Provider()).Compute(buffer);
        }
        /// <summary>
        /// 从字节数组中生成8位CRC校验码（CRC-8MAXIM）
        /// </summary>
        /// <param name="buffer">要计算Crc8的输入</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据</param>
        /// <param name="count">数据中用作数据的字节数</param>
        /// <returns>CRC8校验码</returns>
        public static byte ComputeCrc8(this byte[] buffer, int offset, int count)
        {
            return (new Crc8Provider()).Compute(buffer, offset, count);
        }
        /// <summary>
        /// 从System.IO.Stream中生成8位CRC校验码（CRC-8MAXIM）
        /// </summary>
        /// <param name="inputStream">System.IO.Stream</param>
        /// <returns>CRC8校验码</returns>
        public static byte ComputeCrc8(this Stream inputStream)
        {
            return (new Crc8Provider()).Compute(inputStream);
        }
    }
}
