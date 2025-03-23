using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyNet.Security
{
    /// <summary>
    /// Sm4算法, 对标国际DES算法。
    /// 在线验证：https://btool.cn/sm4
    /// </summary>
    public class Sm4CryptoAltm
    {
        /// <summary>
        /// 编码
        /// </summary>
        public System.Text.Encoding Encoding
        {
            get; set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Sm4CryptoAltm()
        {
            Key = "NIH3Ff10aP8Bbbeh";
            Iv = "0000000000000000";
            HexString = false;
            CryptoMode = Sm4CryptoEnum.ECB;
            this.Encoding = System.Text.Encoding.UTF8;
        }
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 秘钥
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        public string Iv { get; set; }
        /// <summary>
        /// 明文是否是十六进制
        /// </summary>
        public bool HexString { get; set; }
        /// <summary>
        /// 加密模式(默认ECB)
        /// </summary>
        public Sm4CryptoEnum CryptoMode { get; set; }

        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public byte[] Encrypt(Sm4CryptoAltm entity)
        {
            return entity.CryptoMode == Sm4CryptoEnum.CBC ? EncryptCBC(entity) : EncryptECB(entity);
        }
        /// <summary>
        /// ECB加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static byte[] EncryptECB(Sm4CryptoAltm entity)
        {
            var ctx = new Sm4Context
            {
                IsPadding = true
            };

            var keyBytes = entity.HexString ? HexToByte(entity.Key) : entity.Encoding.GetBytes(entity.Key);

            var sm4 = new Sm4Core();
            sm4.Sm4_setKey_enc(ctx, keyBytes);
            var encrypted = sm4.Sm4_crypt_ecb(ctx, entity.Encoding.GetBytes(entity.Data));

            return encrypted;
        }
        /// <summary>
        /// ECB加密
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static byte[] EncryptECB(string plaintext)
        {
            return EncryptECB(new Sm4CryptoAltm() { Data = plaintext });
        }

        /// <summary>
        /// CBC加密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static byte[] EncryptCBC(Sm4CryptoAltm entity)
        {
            var ctx = new Sm4Context
            {
                IsPadding = true
            };

            var keyBytes = entity.HexString ? HexToByte(entity.Key) : entity.Encoding.GetBytes(entity.Key);
            var ivBytes = entity.HexString ? HexToByte(entity.Iv) : entity.Encoding.GetBytes(entity.Iv);

            var sm4 = new Sm4Core();
            sm4.Sm4_setKey_enc(ctx, keyBytes);
            var encrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, entity.Encoding.GetBytes(entity.Data));

            return encrypted;
        }
        /// <summary>
        /// CBC加密
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static byte[] EncryptCBC(string plaintext)
        {
            return EncryptECB(new Sm4CryptoAltm() { Data = plaintext });
        }

        #endregion


        #region 解密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public byte[] Decrypt(Sm4CryptoAltm entity)
        {
            return entity.CryptoMode == Sm4CryptoEnum.CBC ? DecryptCBC(entity) : DecryptECB(entity);
        }
        /// <summary>
        ///  ECB解密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static byte[] DecryptECB(Sm4CryptoAltm entity)
        {
            var ctx = new Sm4Context
            {
                IsPadding = true,
                Mode = 0
            };

            var keyBytes = entity.HexString ? HexToByte(entity.Key) : entity.Encoding.GetBytes(entity.Key);

            var sm4 = new Sm4Core();
            sm4.Sm4_setKey_dec(ctx, keyBytes);
            var decrypted = sm4.Sm4_crypt_ecb(ctx, HexToByte(entity.Data));
            return decrypted;
        }
        /// <summary>
        /// CBC解密
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static byte[] DecryptCBC(Sm4CryptoAltm entity)
        {
            var ctx = new Sm4Context
            {
                IsPadding = true,
                Mode = 0
            };

            var keyBytes = entity.HexString ? HexToByte(entity.Key) : entity.Encoding.GetBytes(entity.Key);
            var ivBytes = entity.HexString ? HexToByte(entity.Iv) : entity.Encoding.GetBytes(entity.Iv);

            var sm4 = new Sm4Core();
            sm4.Sm4_setKey_dec(ctx, keyBytes);
            var decrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Convert.FromBase64String(entity.Data));
            return decrypted;
        }
        #endregion

        /// <summary>
        /// 十六进制字符串转字节数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        internal protected static byte[] HexToByte(string hex)
        {
            return hex.ToBytes("");
        }

        /// <summary>
        /// 加密类型
        /// </summary>
        public enum Sm4CryptoEnum
        {
            /// <summary>
            /// ECB(电码本模式)
            /// </summary>
            [Description("ECB模式")]
            ECB = 0,
            /// <summary>
            /// CBC(密码分组链接模式)
            /// </summary>
            [Description("CBC模式")]
            CBC = 1
        }
    }
}
