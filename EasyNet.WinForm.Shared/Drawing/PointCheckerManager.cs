using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.WinForm
{
    /// <summary>
    /// 点检查器管理器
    /// </summary>
    public class PointCheckerManager
    {
        /// <summary>
        /// 检查器实例
        /// </summary>
        public static IPointChecker Default
        {
            get;
            private set;
        }

        static PointCheckerManager()
        {
            SetChecker(new DefaultPointChecker());
        }

        /// <summary>
        /// 设置检查器
        /// </summary>
        /// <param name="checker"></param>
        public static void SetChecker(IPointChecker checker)
        {
            Default = checker;
        }
    }
}
