using EasyNet.WinForm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EasyNet.Extensions
{
    /// <summary>
    /// Point扩展类
    /// </summary>
    public static class PointExts
    {
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="point">要检查的点</param>
        /// <param name="start">线段起点</param>
        /// <param name="end">线段终点</param>
        /// <param name="tolerance">容差</param>
        /// <returns>true：在线段上；false：不在线段上</returns>
        public static bool IsPointInLine(Point point, Point start, Point end, int tolerance = 2)
        {
            return PointCheckerManager.Default.IsPointInLine(point, start, end, tolerance);
        }
    }
}
