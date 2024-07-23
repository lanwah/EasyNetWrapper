using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyNet.Extensions;

namespace Comparer
{
    /// <summary>
    /// 比较二进制内容是否一致
    /// </summary>
    [MemoryDiagnoser]
    public class SequenceEqual
    {
        [Params(8, 16)]
        public int Size
        {
            get; set;
        }

        public byte[] Data1
        {
            get; set;
        }

        public byte[] Data2
        {
            get; set;
        }

        public List<byte> List1
        {
            get; set;
        }
        public List<byte> List2
        {
            get; set;
        }


        [GlobalSetup]
        public void Setup()
        {
            this.Data1 = new byte[this.Size];
            this.Data2 = new byte[this.Size];

            var random = new Random();
            random.NextBytes(this.Data1);
            random.NextBytes(this.Data2);

            this.List1 = this.Data1.ToList();
            this.List2 = this.Data2.ToList();
        }

        [Benchmark(Baseline = true)]
        public bool EnumerableSequenceEqual()
        {
            return this.Data1.SequenceEqual(this.Data2);
        }

        [Benchmark]
        public bool CustomArrayEqual()
        {
            return this.Data1.IsSequenceEqual(this.Data2);
        }

        [Benchmark]
        public bool CustomListEqual()
        {
            return this.List1.IsSequenceEqual(this.List2);
        }

        [Benchmark]
        public bool IsEqual()
        {
            return IsEqual(this.Data1, this.Data2);
        }

        public static bool IsEqual(Array first, Array second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            for (var i = 0; i < first.Length; i++)
            {
                if (first.GetValue(i) != second.GetValue(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
