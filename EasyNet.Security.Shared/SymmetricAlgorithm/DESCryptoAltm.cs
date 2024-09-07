using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static EasyNet.Security.SymmetricAlgorithmBase;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Security.Shared.SymmetricAlgorithm
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：DESCryptoAltm.cs
// 创建用户：lanwah
// 创建日期：2024/9/6 14:14:56
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
    /// DES 对称加密算法
    /// </summary>
    public class DESCryptoAltm : SymmetricAlgorithmBase
    {
        /// <summary>
        /// DES 对称加密算法
        /// </summary>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        public DESCryptoAltm(CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null) : base(cipherMode, paddingMode, rgbKey, rgbIV)
        {

        }
        /// <summary>
        /// DES 对称加密算法
        /// </summary>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        public DESCryptoAltm(byte[] rgbKey, byte[] rgbIV = null) : base(rgbKey, rgbIV)
        {

        }

        /// <inheritdoc />
        protected override System.Security.Cryptography.SymmetricAlgorithm CreateSymmetricAlgorithm(CipherMode cipherMode, PaddingMode paddingMode)
        {
#if NET8_0_OR_GREATER
            var des = System.Security.Cryptography.DES.Create();
            des.Mode = cipherMode;
            des.Padding = paddingMode;
            return des;
#else
            return new DESCryptoServiceProvider()
            {
                Mode = cipherMode,
                Padding = paddingMode
            };
#endif
        }
    }


    /// <summary>
    /// 3DES加解密算法
    /// </summary>
    public class TripleDESCryptoAltm : SymmetricAlgorithmBase
    {
        /// <summary>
        /// 3DES 对称加密算法
        /// </summary>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        public TripleDESCryptoAltm(CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null) : base(cipherMode, paddingMode, rgbKey, rgbIV)
        {

        }
        /// <summary>
        /// 3DES 对称加密算法
        /// </summary>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        public TripleDESCryptoAltm(byte[] rgbKey, byte[] rgbIV = null) : base(rgbKey, rgbIV)
        {

        }

        /// <inheritdoc />
        protected override System.Security.Cryptography.SymmetricAlgorithm CreateSymmetricAlgorithm(CipherMode cipherMode, PaddingMode paddingMode)
        {
#if NET8_0_OR_GREATER
            var des = System.Security.Cryptography.TripleDES.Create();
            des.Mode = cipherMode;
            des.Padding = paddingMode;
            return des;
#else
            return new TripleDESCryptoServiceProvider()
            {
                Mode = cipherMode,
                Padding = paddingMode
            };
#endif
        }

        /// <inheritdoc />
        protected override void ValidateKey(byte[] rgbKey)
        {
            rgbKey.ThrowIfNull(nameof(rgbKey));
            if ((16 != rgbKey.Length) && (24 != rgbKey.Length))
            {
                throw new ArgumentException($"密钥{nameof(rgbKey)}的长度不合法，有效数据长度为16或24字节。");
            }
        }
        /// <inheritdoc />
        protected override void ValidateIV(byte[] rgbIV)
        {
            if ((CipherMode.ECB != this.CipherMode) && (null == rgbIV))
            {
                throw new ArgumentNullException(nameof(rgbIV));
            }
            if ((CipherMode.ECB != this.CipherMode) && (8 != rgbIV.Length))
            {
                throw new ArgumentException($"方向向量{nameof(rgbIV)}的长度不合法，有效数据长度为8字节。");
            }
        }
        /// <inheritdoc />
        protected override void ValidateEncryptBuffer(byte[] encryptBuffer)
        {
            if ((PaddingMode.None == this.PaddingMode) && (encryptBuffer.Length % 8 != 0))
            {
                throw new ArgumentException("需加密数据内容的长度不合法，当填充模式为PaddingMode.None时加密数据的长度必须为8字节的整数倍。");
            }
        }
        /// <inheritdoc />
        protected override void ValidateDecryptBuffer(byte[] decryptBuffer)
        {
            if ((PaddingMode.None == this.PaddingMode) && (decryptBuffer.Length % 8 != 0))
            {
                throw new ArgumentException("需解密数据内容的长度不合法，当填充模式为PaddingMode.None时解密数据的长度必须为8的整数倍。");
            }
        }
        /// <inheritdoc />
        public override byte[] Decrypt(byte[] decryptBuffer)
        {
            // 参数合法性检查
            decryptBuffer.ThrowIfNull(nameof(decryptBuffer));
            this.ValidateDecryptBuffer(decryptBuffer);

            // 加密操作

#if NET5_0_OR_GREATER
            using var msStream = new MemoryStream(decryptBuffer);
            // Create a CryptoStream using the MemoryStream and the passed Key and initialization vector (IV).  
            using var csStream = new CryptoStream(msStream, this.GetDecryptor(), CryptoStreamMode.Read);
            // Create buffer to hold the decrypted data.  
            var decryptedBuffer = new byte[decryptBuffer.Length];
            // Read the decrypted data out of the crypto stream and place it into the temporary buffer.  
            csStream.Read(decryptedBuffer, 0, decryptedBuffer.Length);
            //Convert the buffer into a string and return it. 
            csStream.Close();
            msStream.Close();
            return decryptedBuffer;

#else
            using (var msStream = new MemoryStream(decryptBuffer))
            {
                using (var csStream = new CryptoStream(msStream, this.GetDecryptor(), CryptoStreamMode.Read))
                {
                    // Create buffer to hold the decrypted data.  
                    var decryptedBuffer = new byte[decryptBuffer.Length];
                    // Read the decrypted data out of the crypto stream and place it into the temporary buffer.  
                    csStream.Read(decryptedBuffer, 0, decryptedBuffer.Length);
                    //Convert the buffer into a string and return it. 
                    csStream.Close();
                    msStream.Close();
                    return decryptedBuffer;
                }
            }
#endif
        }
    }

    /// <summary>
    /// 对称加密算法扩展
    /// </summary>
    public static class CryptoExts
    {
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] DESEncrypt(this byte[] @this, CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null)
        {
            return new DESCryptoAltm(cipherMode, paddingMode, rgbKey, rgbIV).Encrypt(@this);
        }
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] DESEncrypt(this byte[] @this, byte[] rgbKey, byte[] rgbIV = null)
        {
            return @this.DESEncrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV);
        }
        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] DESDecrypt(this byte[] @this, CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null)
        {
            return new DESCryptoAltm(cipherMode, paddingMode, rgbKey, rgbIV).Decrypt(@this);
        }
        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] DESDecrypt(this byte[] @this, byte[] rgbKey, byte[] rgbIV = null)
        {
            return @this.DESDecrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV);
        }

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns>Base64编码的字符串</returns>
        public static string DESEncrypt(this string @this, CipherMode cipherMode, PaddingMode paddingMode, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
#if NET5_0_OR_GREATER
            encoding ??= Encoding.UTF8;
#else
            encoding = encoding ?? Encoding.UTF8;
#endif

            var rgbKeyBytes = rgbKey.GetBytes(encoding);
            var rgbIVBytes = rgbIV.GetBytes(encoding);
            var encryptBuffer = @this.GetBytes(encoding);
            var encryptBytes = encryptBuffer.DESEncrypt(cipherMode, paddingMode, rgbKeyBytes, rgbIVBytes);
            return encryptBytes.ToBase64String();
        }
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESEncrypt(this string @this, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
            return @this.DESEncrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV, encoding);
        }
        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESEncrypt(this string @this, string rgbKey, Encoding encoding = null)
        {
            return @this.DESEncrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, null, encoding);
        }
        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="this">Base64编码的字符串</param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESDecrypt(this string @this, CipherMode cipherMode, PaddingMode paddingMode, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
#if NET5_0_OR_GREATER
            encoding ??= Encoding.UTF8;
#else
            encoding = encoding ?? Encoding.UTF8;
#endif

            var rgbKeyBytes = rgbKey.GetBytes(encoding);
            var rgbIVBytes = rgbIV.GetBytes(encoding);
            var encryptBuffer = @this.FromBase64String();
            var encryptBytes = encryptBuffer.DESDecrypt(cipherMode, paddingMode, rgbKeyBytes, rgbIVBytes);
            return encryptBytes.GetString(encoding);
        }
        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESDecrypt(this string @this, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
            return @this.DESDecrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV, encoding);
        }
        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DESDecrypt(this string @this, string rgbKey, Encoding encoding = null)
        {
            return @this.DESDecrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, null, encoding);
        }



        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] TripleDESEncrypt(this byte[] @this, CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null)
        {
            return new TripleDESCryptoAltm(cipherMode, paddingMode, rgbKey, rgbIV).Encrypt(@this);
        }
        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] TripleDESEncrypt(this byte[] @this, byte[] rgbKey, byte[] rgbIV = null)
        {
            return @this.TripleDESEncrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV);
        }
        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] TripleDESDecrypt(this byte[] @this, CipherMode cipherMode, PaddingMode paddingMode, byte[] rgbKey, byte[] rgbIV = null)
        {
            return new TripleDESCryptoAltm(cipherMode, paddingMode, rgbKey, rgbIV).Decrypt(@this);
        }
        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <returns></returns>
        public static byte[] TripleDESDecrypt(this byte[] @this, byte[] rgbKey, byte[] rgbIV = null)
        {
            return @this.TripleDESDecrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV);
        }

        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns>Base64编码的字符串</returns>
        public static string TripleDESEncrypt(this string @this, CipherMode cipherMode, PaddingMode paddingMode, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
#if NET5_0_OR_GREATER
            encoding ??= Encoding.UTF8;
#else
            encoding = encoding ?? Encoding.UTF8;
#endif

            var rgbKeyBytes = rgbKey.GetBytes(encoding);
            var rgbIVBytes = rgbIV.GetBytes(encoding);
            var encryptBuffer = @this.GetBytes(encoding);
            var encryptBytes = encryptBuffer.TripleDESEncrypt(cipherMode, paddingMode, rgbKeyBytes, rgbIVBytes);
            return encryptBytes.ToBase64String();
        }
        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string TripleDESEncrypt(this string @this, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
            return @this.TripleDESEncrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV, encoding);
        }
        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string TripleDESEncrypt(this string @this, string rgbKey, Encoding encoding = null)
        {
            return @this.TripleDESEncrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, null, encoding);
        }
        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="this">Base64编码的字符串</param>
        /// <param name="cipherMode"></param>
        /// <param name="paddingMode"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string TripleDESDecrypt(this string @this, CipherMode cipherMode, PaddingMode paddingMode, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
#if NET5_0_OR_GREATER
            encoding ??= Encoding.UTF8;
#else
            encoding = encoding ?? Encoding.UTF8;
#endif

            var rgbKeyBytes = rgbKey.GetBytes(encoding);
            var rgbIVBytes = rgbIV.GetBytes(encoding);
            var encryptBuffer = @this.FromBase64String();
            var encryptBytes = encryptBuffer.TripleDESDecrypt(cipherMode, paddingMode, rgbKeyBytes, rgbIVBytes);
            return encryptBytes.GetString(encoding);
        }
        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="rgbIV"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string TripleDESDecrypt(this string @this, string rgbKey, string rgbIV = null, Encoding encoding = null)
        {
            return @this.TripleDESDecrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, rgbIV, encoding);
        }
        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="this"></param>
        /// <param name="rgbKey"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string TripleDESDecrypt(this string @this, string rgbKey, Encoding encoding = null)
        {
            return @this.TripleDESDecrypt(DefaultCipherMode, DefaultPaddingMode, rgbKey, null, encoding);
        }
    }
}
