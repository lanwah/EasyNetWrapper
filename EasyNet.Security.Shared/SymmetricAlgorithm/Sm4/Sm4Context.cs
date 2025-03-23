using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.Security
{
    internal class Sm4Context
    {
        /// <summary>
        /// 1 - 加密模式，0 - 解密模式
        /// </summary>
        public int Mode
        {
            get; set;
        }
        public long[] Sk
        {
            get; set;
        }

        /// <summary>
        /// 是否需要填充
        /// </summary>
        public bool IsPadding
        {
            get; set;
        }

        public Sm4Context()
        {
            this.Mode = 1;
            this.IsPadding = true;
            this.Sk = new long[32];
        }
    }
}
