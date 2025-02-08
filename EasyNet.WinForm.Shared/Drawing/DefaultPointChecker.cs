using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EasyNet.WinForm
{
    /// <summary>
    /// 默认的点检查器
    /// </summary>
    public class DefaultPointChecker : IPointChecker
    {
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="point">要检查的点</param>
        /// <param name="start">线段起点</param>
        /// <param name="end">线段终点</param>
        /// <param name="tolerance">容差</param>
        /// <returns></returns>
        public virtual bool IsPointInLine(Point point, Point start, Point end, int tolerance = 2)
        {
            // 计算直线的斜率
            var slope = (end.Y - start.Y) / (double)(end.X - start.X);
            // 如果直线是垂直的（斜率不存在），则检查x坐标是否相等
            // double.Epsilon 表示大于零的最小正 Double 值
            if (Math.Abs(end.X - start.X) < double.Epsilon)
            {
                return Math.Abs(point.X - start.X) < double.Epsilon;
            }
            // 计算直线的截距（y = mx + b 形式）
            var intercept = start.Y - slope * start.X;
            // 计算点到直线的距离公式（d = |Ax + By + C| / sqrt(A^2 + B^2)）     
            // 在这里，A = slope, B = -1, C = intercept - slope*x0（但x0是任意的，因为我们用p1的x坐标来求C会简化计算）      
            // 但由于我们已经知道点在直线上应该满足 y = mx + b，我们可以直接比较 y 值与 mx + b 的差异
            var calculatedY = slope * point.X + intercept;
            // Y 轴误差
            var differY = Math.Abs(point.Y - calculatedY);

            //if (differY <= tolerance)
            //{
            //    Console.WriteLine($"DefaultPointChecker -> IsPointInLine.differY = {differY}");
            //}

            return differY <= tolerance;
        }
    }
}
