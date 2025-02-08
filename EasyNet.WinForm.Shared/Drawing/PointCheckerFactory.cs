using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.WinForm
{
    /// <summary>
    /// 点检查器工厂
    /// </summary>
    public class PointCheckerFactory

    {
        /// <summary>
        /// 检查器实例
        /// </summary>
        private static IPointChecker Checker
        {
            get; set;
        }

        static PointCheckerFactory()
        {
            SetChecker(new DefaultPointChecker());
        }

        /// <summary>
        /// 设置检查器
        /// </summary>
        /// <param name="checker"></param>
        public static void SetChecker(IPointChecker checker)
        {
            Checker = checker;
        }
    }
}
