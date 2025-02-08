using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EasyNet.WinForm
{
    /// <summary>
    /// 点距离检查器，<see href="https://www.cnblogs.com/object360/p/5788966.html"/>
    /// </summary>
    public class DistancePointChecker : DefaultPointChecker
    {
        /// <inheritdoc />
        public override bool IsPointInLine(Point point, Point start, Point end, int tolerance = 2)
        {
            // 点在线段首尾两端之外则return false
            var cross = (double)((end.X - start.X) * (point.X - start.X) + (end.Y - start.Y) * (point.Y - start.Y));
            if (cross <= 0)
            {
                return false;
            }
            var d2 = (double)((end.X - start.X) * (end.X - start.X) + (end.Y - start.Y) * (end.Y - start.Y));
            if (cross >= d2)
            {
                return false;
            }

            double r = cross / d2;
            double px = start.X + (end.X - start.X) * r;
            double py = start.Y + (end.Y - start.Y) * r;

            // 计算点到线段的距离
            var distance = Math.Sqrt((point.X - px) * (point.X - px) + (py - point.Y) * (py - point.Y));
            //if (distance <= tolerance)
            //{
            //    Console.WriteLine($"DistancePointChecker -> IsPointInLine.distance = {distance}");
            //}

            //判断距离是否小于误差
            return distance <= tolerance;
        }
    }
}
