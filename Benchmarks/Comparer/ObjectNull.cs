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
    /// 判断对象是否未空
    /// </summary>
    [MemoryDiagnoser]
    public class ObjectNull
    {
        [Params("NULL", "空", "空格", "abc")]
        public string Key
        {
            get; set;
        }

        public Dictionary<string, object> ObjectMapper = new Dictionary<string, object>()
        {
            {"NULL",null },
            {"空",string.Empty },
            {"空格","  " },
            {"abc", "abc" },
        };

        [Benchmark(Baseline = true)]
        public bool IsEqualNull()
        {
            return (this.ObjectMapper[Key] == null);
        }
        [Benchmark]
        public bool IsNull()
        {
            return this.ObjectMapper[Key].IsNull();
        }
    }
}
