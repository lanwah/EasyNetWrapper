using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using static EasyNet.Security.SymmetricAlgorithmBase;

namespace EasyNet.Security
{
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
