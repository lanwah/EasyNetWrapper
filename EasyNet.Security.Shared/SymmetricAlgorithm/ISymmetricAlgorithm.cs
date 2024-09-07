using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Security.Shared.SymmetricAlgorithm
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：ISymmetricAlgorithm.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 13:17:06
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Security
{
    /// <summary>
    /// 对称加密算法接口
    /// </summary>
    public interface ISymmetricAlgorithm
    {
        /// <summary>
        /// 使用的对称加密算法
        /// </summary>
        SymmetricAlgorithm Algorithm { get; }
        /// <summary>
        /// 加密模式，系统默认值 CipherMode.CBC
        /// </summary>
        CipherMode CipherMode { get; }
        /// <summary>
        /// 填充模式，系统默认值 PaddingMode.PKCS7
        /// </summary>
        PaddingMode PaddingMode { get; }
        /// <summary>
        /// 对称算法的密钥
        /// </summary>
        byte[] Key { get; }
        /// <summary>
        /// 8字节对称算法的初始化向量
        /// </summary>
        byte[] IV { get; }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptBuffer">明文的需要加密的数据。</param>
        /// <returns>返回加密后密文数据</returns>
        byte[] Encrypt(byte[] encryptBuffer);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptBuffer">密文的需要解密的数据。</param>
        /// <returns>返回解密后明文数据</returns>
        byte[] Decrypt(byte[] decryptBuffer);
    }

    /// <summary>
    /// 对称加密算法基类
    /// </summary>
    public abstract class SymmetricAlgorithmBase : ISymmetricAlgorithm
    {
        /// <summary>
        /// 默认加密模式 CipherMode.ECB
        /// </summary>
        public static readonly CipherMode DefaultCipherMode = CipherMode.ECB;
        /// <summary>
        /// 默认填充模式 PaddingMode.Zeros
        /// </summary>
        public static readonly PaddingMode DefaultPaddingMode = PaddingMode.Zeros;
        /// <summary>
        /// 默认8个0的方向向量
        /// </summary>
        public static readonly byte[] DefaultIV = new byte[8];

        /// <inheritdoc />
        public SymmetricAlgorithm Algorithm
        {
            get;
            protected set;
        }
        private CipherMode _cipherMode = DefaultCipherMode;
        /// <inheritdoc />
        public CipherMode CipherMode
        {
            get => this._cipherMode;
            protected set => this._cipherMode = value;
        }
        private PaddingMode _paddingMode = DefaultPaddingMode;
        /// <inheritdoc />
        public PaddingMode PaddingMode
        {
            get => this._paddingMode;
            protected set => this._paddingMode = value;
        }
        /// <inheritdoc />
        public byte[] Key { get; protected set; }
        /// <summary>
        /// 使用默认8个0的方向向量
        /// </summary>
        private byte[] _rgbIV = new byte[8];
        /// <inheritdoc />
        public byte[] IV { get => this._rgbIV; protected set => this._rgbIV = value; }



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cipherMode">加密模式</param>
        /// <param name="paddingMode">填充模式</param>
        /// <param name="rgbKey">8字节对称算法的密钥</param>
        /// <param name="rgbIV">8字节对称算法的初始化向量</param>
        public SymmetricAlgorithmBase(CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null)
        {
            this.CipherMode = cipherMode;
            this.PaddingMode = paddingMode;

            // Key
            this.ValidateKey(rgbKey);
            this.Key = rgbKey;

            // IV
            if (rgbIV.HasData())
            {
                this.ValidateIV(rgbIV);
                this.IV = rgbIV;
            }

            this.Algorithm = this.CreateSymmetricAlgorithm(this.CipherMode, this.PaddingMode);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        public SymmetricAlgorithmBase(byte[] rgbKey, byte[] rgbIV = null) : this(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV)
        {

        }

        /// <summary>
        /// 验证密钥的有效性
        /// </summary>
        protected virtual void ValidateKey(byte[] rgbKey)
        {
            rgbKey.ThrowIfNull(nameof(rgbKey));
            if (8 != rgbKey.Length)
            {
                throw new ArgumentException($"密钥{nameof(rgbKey)}的长度不合法，有效数据长度为8字节。");
            }
        }
        /// <summary>
        /// 验证方向向量的有效性
        /// </summary>
        /// <param name="rgbIV"></param>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void ValidateIV(byte[] rgbIV)
        {
            if (rgbIV.HasData())
            {
                if (8 != rgbIV.Length)
                {
                    throw new ArgumentException($"方向向量{nameof(rgbIV)}的长度不合法，有效数据长度为8字节。");
                }
            }
        }

        /// <summary>
        /// 验证加密数据有效性
        /// </summary>
        /// <param name="encryptBuffer"></param>
        protected virtual void ValidateEncryptBuffer(byte[] encryptBuffer)
        {

        }
        /// <summary>
        /// 验证解密数据有效性
        /// </summary>
        /// <param name="decryptBuffer"></param>
        protected virtual void ValidateDecryptBuffer(byte[] decryptBuffer)
        {

        }

        /// <summary>
        /// 创建对称加密算法
        /// </summary>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <returns></returns>
        protected abstract SymmetricAlgorithm CreateSymmetricAlgorithm(CipherMode cipherMode, PaddingMode paddingMode);
        /// <inheritdoc />
        public virtual byte[] Encrypt(byte[] encryptBuffer)
        {
            // 参数合法性检查
            encryptBuffer.ThrowIfNull(nameof(encryptBuffer));
            this.ValidateEncryptBuffer(encryptBuffer);

            // 加密操作
#if NET5_0_OR_GREATER
            using var msStream = new MemoryStream();
            using var csStream = new CryptoStream(msStream, this.GetEncryptor(), CryptoStreamMode.Write);
            csStream.Write(encryptBuffer, 0, encryptBuffer.Length);
            csStream.FlushFinalBlock();
            var encryptedBuffer = msStream.ToArray();
            csStream.Close();
            msStream.Close();
            return encryptedBuffer;
#else
            using (var msStream = new MemoryStream())
            {
                using (var csStream = new CryptoStream(msStream, this.GetEncryptor(), CryptoStreamMode.Write))
                {
                    csStream.Write(encryptBuffer, 0, encryptBuffer.Length);
                    csStream.FlushFinalBlock();
                    var encryptedBuffer = msStream.ToArray();
                    csStream.Close();
                    msStream.Close();
                    return encryptedBuffer;
                }
            }
#endif
        }
        /// <inheritdoc />
        public virtual byte[] Decrypt(byte[] decryptBuffer)
        {
            // 参数合法性检查
            decryptBuffer.ThrowIfNull(nameof(decryptBuffer));
            this.ValidateDecryptBuffer(decryptBuffer);

            // 加密操作

#if NET5_0_OR_GREATER
            using var msStream = new MemoryStream();
            using var csStream = new CryptoStream(msStream, this.GetDecryptor(), CryptoStreamMode.Write);
            csStream.Write(decryptBuffer, 0, decryptBuffer.Length);
            csStream.FlushFinalBlock();
            var decryptedBuffer = msStream.ToArray();
            csStream.Close();
            msStream.Close();
            return decryptedBuffer;

#else
            using (var msStream = new MemoryStream())
            {
                using (var csStream = new CryptoStream(msStream, this.GetDecryptor(), CryptoStreamMode.Write))
                {
                    csStream.Write(decryptBuffer, 0, decryptBuffer.Length);
                    csStream.FlushFinalBlock();
                    var decryptedBuffer = msStream.ToArray();
                    csStream.Close();
                    msStream.Close();
                    return decryptedBuffer;
                }
            }
#endif
        }



        /// <summary>
        /// 获取加密器
        /// </summary>
        /// <returns></returns>
        protected virtual ICryptoTransform GetEncryptor() => this.Algorithm.CreateEncryptor(this.Key, this.IV);
        /// <summary>
        /// 获取解密器
        /// </summary>
        /// <returns></returns>
        protected virtual ICryptoTransform GetDecryptor() => this.Algorithm.CreateDecryptor(this.Key, this.IV);
    }
}
