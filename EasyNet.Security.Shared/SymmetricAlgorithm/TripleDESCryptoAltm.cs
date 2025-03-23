using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EasyNet.Security
{
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
}
