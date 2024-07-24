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
    /// 字符串转整型
    /// </summary>
    [MemoryDiagnoser]
    public class StringToInt
    {
        string str = "123456";

        //[GlobalSetup]
        //public void Setup()
        //{

        //}

        [Benchmark(Baseline = true)]
        public int Parse()
        {
            int result = int.Parse(str);
            return result;
        }

        [Benchmark]
        public int Convert()
        {
            int result = System.Convert.ToInt32(str);
            return result;
        }

        [Benchmark]
        public int ChangeType()
        {
            int result = (int)System.Convert.ChangeType(str, typeof(int));
            return result;
        }

        [Benchmark]
        public int ConvertTo()
        {
            int result = str.ConvertTo<int>();
            return result;
        }
    }
}
