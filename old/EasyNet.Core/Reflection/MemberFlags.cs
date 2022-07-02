using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：MemberFlags
// 创 建 者：lanwah
// 创建日期：2021/06/25 13:43:41
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Reflection
{
    /// <summary>
    /// 成员的绑定标识。
    /// </summary>
    public static class MemberFlags
    {
        #region Fields

        /// <summary>
        /// 所有实例成员和静态成员（区分大小写）。
        /// </summary>
        public const BindingFlags AnyFlags = InstanceFlags | BindingFlags.Static;

        /// <summary>
        /// 所有实例成员（区分大小写）。
        /// </summary>
        public const BindingFlags InstanceFlags = DefaultFlags | BindingFlags.Instance;

        /// <summary>
        /// 所有可获取的实例属性（区分大小写）。
        /// </summary>
        public const BindingFlags InstanceGetPropertyFlags = InstanceFlags | BindingFlags.GetProperty;

        /// <summary>
        /// 所有可设置的实例属性（区分大小写）。
        /// </summary>
        public const BindingFlags InstanceSetPropertyFlags = InstanceFlags | BindingFlags.SetProperty;

        /// <summary>
        /// 所有静态成员（区分大小写）。
        /// </summary>
        public const BindingFlags StaticFlags = DefaultFlags | BindingFlags.Static;

        /// <summary>
        /// 所有可获取的静态属性（区分大小写）。
        /// </summary>
        public const BindingFlags StaticGetPropertyFlags = StaticFlags | BindingFlags.GetProperty;

        /// <summary>
        /// 所有可设置的静态属性（区分大小写）。
        /// </summary>
        public const BindingFlags StaticSetPropertyFlags = StaticFlags | BindingFlags.SetProperty;

        /// <summary>
        /// 默认绑定标识（共有|私有）
        /// </summary>
        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic;

        #endregion Fields
    }
}
