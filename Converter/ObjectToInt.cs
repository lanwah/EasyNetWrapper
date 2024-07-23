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
    /// Object转Int
    /// </summary>
    [MemoryDiagnoser] // 输出内存使用情况及垃圾回收信息
    public class ObjectToInt
    {
        [Params(10, 100)]
        public int Size
        {
            get; set;
        }
        public List<object> ObjList
        {
            get; set;
        }

        [GlobalSetup]
        public void Setup()
        {
            var items = Enumerable.Range(0, Size);
            if (this.ObjList.IsNull())
            {
                this.ObjList = new List<object>();
            }
            this.ObjList.Clear();

            foreach (var item in items)
            {
                this.ObjList.Add(item);
            }
        }


        [Benchmark(Baseline = true)]
        public void ToInt()
        {
            foreach (var obj in this.ObjList)
            {
                _ = (int)obj;
            }
        }
        [Benchmark]
        public void CastToInt()
        {
            foreach (var obj in this.ObjList)
            {
                _ = obj.CastTo<int>();
            }
        }
        [Benchmark]
        public void ChangeToInt()
        {
            foreach (var obj in this.ObjList)
            {
                _ = (int)Convert.ChangeType(obj, typeof(int));
            }
        }
        [Benchmark]
        public void ConvertToInt()
        {
            foreach (var obj in this.ObjList)
            {
                _ = Convert.ToInt32(obj);
            }
        }
    }
}
