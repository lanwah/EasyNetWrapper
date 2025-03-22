using BenchmarkDotNet.Attributes;
using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    /// <summary>
    /// Byte 转 16进制字符串
    /// </summary>
    [MemoryDiagnoser] // 输出内存使用情况及垃圾回收信息
    public class ByteToHex
    {
        [Params(10, 100)]
        public int Size
        {
            get; set;
        }

        public List<byte> ByteList
        {
            get; set;
        }

        [GlobalSetup]
        public void Setup()
        {
            var items = Enumerable.Range(0, Size);
            if (this.ByteList.IsNull())
            {
                this.ByteList = new List<byte>();
            }
            this.ByteList.Clear();

            foreach (var item in items)
            {
                this.ByteList.Add((byte)item);
            }
        }

        [Benchmark(Baseline = true)]
        public void ToHex()
        {
            foreach (var b in this.ByteList)
            {
                _ = b.ToString("X2");
            }
        }
        [Benchmark]
        public void ToHexString()
        {
            foreach (var b in this.ByteList)
            {
                //_ = new string(new char[] { (b / 16).GetHexValue(), (b % 16).GetHexValue() });

                _ = b.ToHexString();
            }
        }
    }
}
