using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EasyNet.Extensions;

namespace EasyNet.Security
{
    /// <summary>
    /// CRC算法抽象类
    /// 在线CRC工具：http://www.metools.info/code/c15.html
    /// </summary>
    public abstract class CrcProvider<T> : ICrcProvider<T>
    {
        /// <summary>
        /// 生成多项式
        /// </summary>
        public abstract string Polynomial
        {
            get;
        }
        /// <summary>
        /// 初始值
        /// </summary>
        public abstract T Init { get; }
        /// <summary>
        /// 异或值XOROUT
        /// </summary>
        public abstract T Seed { get; }

        /// <summary>
        /// 计算指定字节数组的指定区域的CRC校验码
        /// </summary>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据</param>
        /// <param name="count">数据中用作数据的字节数</param>
        /// <returns>CRC校验码</returns>
        public T Compute(byte[] buffer, int offset, int count)
        {
            return this.ComputeFinal(this.Init, buffer, offset, count);
        }
        /// <summary>
        /// 计算指定字节数组的CRC校验码
        /// </summary>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <returns>CRC校验码</returns>
        public T Compute(byte[] buffer)
        {
            return this.Compute(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 计算指定 Stream 对象的CRC校验码
        /// </summary>
        /// <param name="inputStream">要计算CRC校验码的输入</param>
        /// <returns>CRC校验码</returns>
        public T Compute(Stream inputStream)
        {
            return this.ComputeFinal(this.Init, inputStream);
        }
        /// <summary>
        /// 计算指定字节的CRC校验码
        /// </summary>
        /// <param name="sourceCrc">源CRC校验码</param>
        /// <param name="number">字节数据</param>
        /// <returns>CRC校验码</returns>
        /// <exception cref="NotImplementedException"></exception>
        protected abstract T ComputeCore(T sourceCrc, byte number);
        /// <summary>
        /// 计算指定字节数组的指定区域的CRC校验码
        /// </summary>
        /// <param name="sourceCrc">源CRC校验码</param>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据</param>
        /// <param name="count">数据中用作数据的字节数</param>
        /// <returns>CRC校验码</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual T ComputeFinal(T sourceCrc, byte[] buffer, int offset, int count)
        {
            buffer.ThrowIfNull(nameof(buffer));

            if ((offset < 0) || (count < 0) || ((offset + count) > buffer.Length))
            {
                throw new Exception($"{nameof(offset)}或{nameof(count)}参数超出范围！");
            }

            var crc = sourceCrc;
            for (int i = offset; i < count; i++)
            {
                crc = this.ComputeCore(crc, buffer[i]);
            }
            return crc;
        }
        /// <summary>
        /// 计算指定字节数组的CRC校验码
        /// </summary>
        /// <param name="sourceCrc">源CRC校验码</param>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <returns>CRC校验码</returns>
        protected T ComputeFinal(T sourceCrc, byte[] buffer)
        {
            return this.ComputeFinal(sourceCrc, buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 计算指定 Stream 对象的CRC校验码
        /// </summary>
        /// <param name="sourceCrc">源CRC校验码</param>
        /// <param name="inputStream">要计算CRC校验码的输入</param>
        /// <returns>CRC校验码</returns>
        protected virtual T ComputeFinal(T sourceCrc, Stream inputStream)
        {
            // 缓冲区大小，默认4Kb
            var array = new byte[4096];
            var result = sourceCrc;
            int num;
            do
            {
                num = inputStream.Read(array, 0, 4096);
                if (num > 0)
                {
                    result = this.ComputeFinal(result, array, 0, num);
                }
            }
            while (num > 0);

            return result;
        }
    }

    /// <summary>
    /// CRC算法接口
    /// 在线CRC工具：http://www.metools.info/code/c15.html
    /// </summary>
    public interface ICrcProvider<T>
    {
        /// <summary>
        /// 生成多项式
        /// </summary>
        string Polynomial
        {
            get;
        }
        /// <summary>
        /// 初始值INIT
        /// </summary>
        T Init { get; }
        /// <summary>
        /// 异或值XOROUT
        /// </summary>
        T Seed { get; }

        /// <summary>
        /// 计算指定字节数组的指定区域的CRC校验码
        /// </summary>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据</param>
        /// <param name="count">数据中用作数据的字节数</param>
        /// <returns>CRC校验码</returns>
        T Compute(byte[] buffer, int offset, int count);
        /// <summary>
        /// 计算指定字节数组的CRC校验码
        /// </summary>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <returns>CRC校验码</returns>
        T Compute(byte[] buffer);
        /// <summary>
        /// 计算指定 Stream 对象的CRC校验码
        /// </summary>
        /// <param name="inputStream">要计算CRC校验码的输入</param>
        /// <returns>CRC校验码</returns>
        T Compute(Stream inputStream);
    }
}
