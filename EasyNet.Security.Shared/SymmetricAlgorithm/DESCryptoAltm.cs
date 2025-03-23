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
    
}
