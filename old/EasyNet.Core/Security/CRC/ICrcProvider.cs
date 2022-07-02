using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyNet.Core.Security.CRC
{
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
        /// CRC 校验表
        /// </summary>
        T[] CrcTable
        {
            get;
        }


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

    /// <summary>
    /// CRC算法抽象类
    /// </summary>
    public class CrcProvider<T> : ICrcProvider<T>
    {
        /// <summary>
        /// 生成多项式
        /// </summary>
        public virtual string Polynomial
        {
            get;
        }
        /// <summary>
        /// CRC 校验表
        /// </summary>
        public virtual T[] CrcTable
        {
            get;
        }
        /// <summary>
        /// 计算指定字节数组的指定区域的CRC校验码
        /// </summary>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据</param>
        /// <param name="count">数据中用作数据的字节数</param>
        /// <returns>CRC校验码</returns>
        public T Compute(byte[] buffer, int offset, int count)
        {
            return this.ComputeFinal(default(T), buffer, offset, count);
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
            return this.ComputeFinal(default(T), inputStream);
        }
        /// <summary>
        /// 计算指定字节的CRC校验码
        /// </summary>
        /// <param name="sourceCRCCode">源CRC校验码</param>
        /// <param name="number">字节数据</param>
        /// <returns>CRC校验码</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal virtual T ComputeCore(T sourceCRCCode, byte number)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 计算指定字节数组的指定区域的CRC校验码
        /// </summary>
        /// <param name="sourceCRCCode">源CRC校验码</param>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据</param>
        /// <param name="count">数据中用作数据的字节数</param>
        /// <returns>CRC校验码</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal T ComputeFinal(T sourceCRCCode, byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if ((offset < 0) || (count < 0) || ((offset + count) > buffer.Length))
            {
                throw new ArgumentOutOfRangeException("offset或count");
            }

            var crc = sourceCRCCode;
            for (int i = offset; i < count; i++)
            {
                crc = this.ComputeCore(crc, buffer[i]);
            }
            return crc;
        }
        /// <summary>
        /// 计算指定字节数组的CRC校验码
        /// </summary>
        /// <param name="sourceCRCCode">源CRC校验码</param>
        /// <param name="buffer">要计算CRC的输入</param>
        /// <returns>CRC校验码</returns>
        internal T ComputeFinal(T sourceCRCCode, byte[] buffer)
        {
            return this.ComputeFinal(sourceCRCCode, buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 计算指定 Stream 对象的CRC校验码
        /// </summary>
        /// <param name="sourceCRCCode">源CRC校验码</param>
        /// <param name="inputStream">要计算CRC校验码的输入</param>
        /// <returns>CRC校验码</returns>
        internal T ComputeFinal(T sourceCRCCode, Stream inputStream)
        {
            var array = new byte[4096];
            var num = 0;
            var result = sourceCRCCode;
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
}
