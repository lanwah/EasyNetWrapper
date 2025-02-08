using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EasyNet.WinForm
{
    /// <summary>
    /// 点距离检查器V2
    /// </summary>
    public class DistancePointCheckerV2 : DefaultPointChecker
    {
        /// <inheritdoc />
        public override bool IsPointInLine(Point point, Point start, Point end, int tolerance = 1)
        {
            return this.IsPointOnLineSegment(start, end, point, tolerance);
        }
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="testPoint"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        protected virtual bool IsPointOnLineSegment(Point p1, Point p2, Point testPoint, int tolerance)
        {
            // 计算点到线段的距离
            var distance = PointToLineDistance(p1, p2, testPoint);

            // 如果距离小于1像素，则认为点在线段上
            //return distance < 1.0;
            return distance < tolerance;
        }
        /// <summary>
        /// 计算点到线段的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="testPoint"></param>
        /// <returns></returns>
        protected virtual double PointToLineDistance(Point p1, Point p2, Point testPoint)
        {
            var x0 = (double)testPoint.X;
            var y0 = testPoint.Y;
            var x1 = p1.X;
            var y1 = p1.Y;
            var x2 = p2.X;
            var y2 = p2.Y;

            var numerator = Math.Abs((y2 - y1) * x0 - (x2 - x1) * y0 + x2 * y1 - y2 * x1);
            var denominator = Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));

            return numerator / denominator;
        }
    }
}
