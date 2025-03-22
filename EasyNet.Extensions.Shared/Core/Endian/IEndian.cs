using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// 字节存储顺序接口
    /// </summary>
    public interface IEndian
    {
        /// <summary>
        /// 将 UInt16 转换为2个字节的字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        byte[] GetBytes(UInt16 value);
        /// <summary>
        /// 将 bool 转换为字节数组（用于Modbus中，功能码 0x05：写单个线圈（Write Single Coil）时的值）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        byte[] GetBytes(bool value);


        /// <summary>
        /// 将数组转换为 UInt16
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex">起始索引</param>
        /// <returns></returns>
        UInt16 ToShort(byte[] bytes, int startIndex = 0);
    }

    /// <summary>
    /// 大端字节序（高位在前，低位在后）
    /// </summary>
    public class BigEndian : IEndian
    {
        /// <inheritdoc />
        public byte[] GetBytes(UInt16 value)
        {
            // 高位在前，低位在后
            return new byte[] { (byte)(value >> 8), (byte)(value & 0xFF) };
        }
        /// <inheritdoc />
        public byte[] GetBytes(bool value)
        {
            // 高位在前，低位在后
            return new byte[] { 0xFF, 0x00 };
        }

        /// <inheritdoc />
        public UInt16 ToShort(byte[] bytes, int startIndex = 0)
        {
            //// BitConverter.ToUInt16 适合低位在前，高位在后的数据
            //BitConverter.ToUInt16(buffer, index);

            // 高位在前，低位在后
            return (UInt16)(bytes[startIndex] << 8 | bytes[startIndex + 1]);
        }
    }

    /// <summary>
    /// 小端字节序（低位在前，高位在后）
    /// </summary>
    public class LittleEndian : IEndian
    {
        /// <inheritdoc />
        public byte[] GetBytes(UInt16 value)
        {
            //// 低位在前，高位在后
            //return BitConverter.GetBytes(@this);

            // 低位在前，高位在后
            return new byte[] { (byte)(value & 0xFF), (byte)(value >> 8) };
        }
        /// <inheritdoc />
        public byte[] GetBytes(bool value)
        {
            // 低位在前，高位在后
            return new byte[] { 0x00, 0xFF };
        }

        /// <inheritdoc />
        public UInt16 ToShort(byte[] bytes, int startIndex = 0)
        {
            //// 低位在前，高位在后
            //return BitConverter.ToUInt16(bytes, startIndex);

            // 低位在前，高位在后
            return (UInt16)(bytes[startIndex] | bytes[startIndex + 1] << 8);
        }
    }
}
