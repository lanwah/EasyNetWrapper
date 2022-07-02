using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：IntPtrExtn
// 创 建 者：lanwah
// 创建日期：2022/7/2 9:41:18
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extension
{
    /// <summary>
    /// IO 扩展方法
    /// </summary>
    public static class IOExtn
    {
        /// <summary>
        /// 读取Stream中内容到的Byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Stream stream)
        {
            if (stream.IsNull())
            {
                return null;
            }

            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            // Read the source file into a byte array.
            var buffer = new byte[stream.Length];
            var remainLen = (int)stream.Length;
            var readedLen = 0;
            while (remainLen > 0)
            {
                // Read may return anything from 0 to numBytesToRead.
                var len = stream.Read(buffer, readedLen, remainLen);

                // Break when the end of the file is reached.
                if (len == 0)
                {
                    break;
                }

                readedLen += len;
                remainLen -= len;
            }
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            remainLen = buffer.Length;

            // 生成副本
            var bytes = new byte[buffer.Length];
            Array.Copy(buffer, bytes, buffer.Length);
            return bytes;
        }

        /// <summary>
        /// 把<paramref name="stream"/>写入<paramref name="filePath"/>指定的文件。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool ToFile(this Stream stream, string filePath)
        {
            filePath.NotNullOrEmptyCheck(nameof(filePath));
            if (stream.IsNull())
            {
                return false;
            }

            using (var fstream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new BinaryWriter(fstream))
                {
                    writer.Write(stream.ToBytes());

                    stream.Close();
                    writer.Close();
                }
            }
            return true;
        }
    }
}
