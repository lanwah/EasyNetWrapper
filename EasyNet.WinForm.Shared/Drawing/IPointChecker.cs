using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EasyNet.WinForm
{
    /// <summary>
    /// Point 检查器接口
    /// </summary>
    public interface IPointChecker
    {
        /// <summary>
        /// 判断点是否在线段上
        /// </summary>
        /// <param name="point">要检查的点</param>
        /// <param name="start">线段起点</param>
        /// <param name="end">线段终点</param>
        /// <param name="tolerance">容差</param>
        /// <returns>true：在线段上；false：不在线段上</returns>
        bool IsPointInLine(Point point, Point start, Point end, int tolerance = 2);
    }
}
