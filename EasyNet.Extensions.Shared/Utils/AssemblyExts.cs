using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions
// 文件名称：AssemblyExts
// 创 建 者：lanwah
// 创建日期：2022/7/2 9:41:18
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// 程序集相关扩展方法
    /// </summary>
    public static class AssemblyExts
    {
        /// <summary>
        /// 获取程序集中实现指定接口的类型，并进行实例化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static List<T> GetImplementTypes<T>(this Assembly assembly)
        {
            // 获取程序集中实现接口的类型，并进行实例化
            var types = assembly.GetTypes().Where(type => !type.IsAbstract && type.IsClass && typeof(T).IsAssignableFrom(type)).Select(type => (T)Activator.CreateInstance(type)).ToList();
            return types;
        }
    }
}
