using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Extension
// 文件名称：ReflectionExtension
// 创 建 者：lanwah
// 创建日期：2021/02/22 16:17:20
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Extension
{
    /// <summary>
    /// 反射相关扩展类
    /// </summary>
    public static class ReflectionExtension
    {
        /// <summary>
        /// 获取成员特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否集成</param>
        /// <returns>返回元数据</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            ArgChecker.NotNull(member, nameof(member));
            T[] attributes = member.GetCustomAttributes(typeof(T), inherit) as T[];

            if ((attributes == null) || (attributes.Length == 0))
            {
                return null;
            }

            return attributes[0];
        }
        /// <summary>
        /// 得到成员特性数组
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否集成</param>
        /// <returns>返回成员元数据数组</returns>
        public static T[] GetAttributes<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            ArgChecker.NotNull(member, nameof(member));
            return member.GetCustomAttributes(typeof(T), inherit) as T[];
        }
        /// <summary>
        /// 判断成员是否包含特点的特性
        /// </summary>
        /// <typeparam name="T">元数据类型</typeparam>
        /// <param name="member">成员</param>
        /// <param name="inherit">是否继承</param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this ICustomAttributeProvider member, bool inherit = false)
            where T : Attribute
        {
            ArgChecker.NotNull(member, nameof(member));
            return member.IsDefined(typeof(T), inherit);
        }
    }
}
