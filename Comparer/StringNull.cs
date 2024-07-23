using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;

namespace Comparer
{
    /// <summary>
    /// 字符串非空验证
    /// </summary>
    [MemoryDiagnoser]
    public class StringNull
    {
        [Params("NULL", "空", "空格", "abc")]
        public string Key
        {
            get; set;
        }
        public Dictionary<string, string> StringMapper = new Dictionary<string, string>()
        {
            {"NULL",null },
            {"空",string.Empty },
            {"空格","  " },
            {"abc", "abc" }
        };
        private string Value
        {
            get
            {
                return this.StringMapper[this.Key];
            }
        }

        [Benchmark(Baseline = true)]
        public bool IsNullOrEmpty()
        {
            return string.IsNullOrEmpty(Value);
        }

        [Benchmark]
        public bool IsNullOrWhiteSpace()
        {
            return string.IsNullOrWhiteSpace(Value);
        }

        [Benchmark]
        public bool IsNullOrEmptyByLength()
        {
            return this.Value.IsNullOrEmpty();
        }

        [Benchmark]
        public bool IsNullOrEmptyEx()
        {
            return this.Value.IsNullOrEmptyEx();
        }
    }
}
