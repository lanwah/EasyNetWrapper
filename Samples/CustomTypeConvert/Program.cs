using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;

namespace CustomTypeConvert
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = new Point { X = 10, Y = 20 };
            string s = TypeDescriptor.GetConverter(typeof(Point)).ConvertTo(p, typeof(string)).ToString();
            Console.WriteLine($"Point to string:");
            Console.WriteLine($"{p} -> {s}");

            var s2 = "30,40";
            var p2 = (Point)TypeDescriptor.GetConverter(typeof(Point)).ConvertFrom(s2);
            Console.WriteLine($"string to Point:");
            Console.WriteLine($"{s2} -> {p2}");

            var p3 = s2.ConvertTo<Point>();
            Console.WriteLine($"string to Point(using EasyNet.Extensions.ConvertTo):");
            Console.WriteLine($"{s2} -> {p3}");

            Console.ReadKey();
        }
    }

    [TypeConverter(typeof(PointConverter))]
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public void AddX(int added)
        {
            this.X += added;
        }
        public void AddY(int added)
        {
            this.Y += added;
        }

        public override string ToString()
        {
            return string.Format("X={0},Y={1}", this.X, this.Y);
        }

        public static string ToString(Point p)
        {
            return string.Format("{0},{1}", p.X, p.Y);
        }
    }

    /// <summary>
    /// 自定义Point类型转换器
    /// <see langword="How to: Implement a Type Converter" href="https://learn.microsoft.com/zh-cn/previous-versions/visualstudio/visual-studio-2013/ayybcxe5(v=vs.120)"/>
    /// <see langword="TypeConverter 类" href="https://learn.microsoft.com/zh-cn/dotnet/api/system.componentmodel.typeconverter?view=net-8.0"/>
    /// </summary>
    public class PointConverter : TypeConverter
    {
        /// <summary>
        /// 是否支持从字符串转换为Point类型（字符串转Point）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// 从字符串转换（字符串转Point）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] strs = ((string)value).Split(',');
                if (strs.Length == 2)
                {
                    int x, y;
                    if (int.TryParse(strs[0], out x) && int.TryParse(strs[1], out y))
                    {
                        return new Point { X = x, Y = y };
                    }
                }
            }

            return base.ConvertFrom(context, culture, value);
        }



        /// <summary>
        /// 是否支持从Point转换为字符串类型（Point转字符串）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            //// 转换成字符串类型时此方法可以不重写，因为基类已经实现了转换成字符串的判断
            //if (destinationType == typeof(string))
            //{
            //    return true;
            //}

            return base.CanConvertTo(context, destinationType);
        }
        /// <summary>
        /// 从Point转换（Point转字符串）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                Point p = (Point)value;
                return Point.ToString(p);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
