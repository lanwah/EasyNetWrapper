using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;

namespace Converter
{
    /// <summary>
    /// Object转字符串
    /// </summary>
    [MemoryDiagnoser] // 输出内存使用情况及垃圾回收信息
    public class ObjectToString
    {
        [Params(100)]
        public int Size
        {
            get; set;
        }
        public List<object> ObjList = new List<object>();

        [GlobalSetup]
        public void Setup()
        {
            var items = Enumerable.Range(0, Size);

            foreach (var item in items)
            {
                this.ObjList.Add(item);
            }
        }

        [Benchmark(Baseline = true)]
        public new void ToString()
        {
            foreach (var obj in this.ObjList)
            {
                _ = obj?.ToString();
            }
        }
        [Benchmark]
        public void ToStringEx()
        {
            foreach (var obj in this.ObjList)
            {
                _ = obj.ToStringEx();
            }
        }
        [Benchmark]
        public void ToStringFormat()
        {
            foreach (var obj in this.ObjList)
            {
                _ = $"{obj}";
            }
        }
        [Benchmark]
        public void ConvertToString()
        {
            foreach (var obj in this.ObjList)
            {
                _ = Convert.ToString(obj);
            }
        }
        [Benchmark]
        public void ChangeToString()
        {
            foreach (var obj in this.ObjList)
            {
                _ = (string)Convert.ChangeType(obj, typeof(string));
            }
        }
    }
}
