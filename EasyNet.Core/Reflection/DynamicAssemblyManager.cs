using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：DynamicAssemblyManager
// 创 建 者：lanwah
// 创建日期：2021/07/01 11:27:50
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
    /// 动态程序集管理
    /// </summary>
    public class DynamicAssemblyManager
    {
        /// <summary>
        /// 保存动态程序集
        /// </summary>
        public static void SaveAssembly()
        {
            lock (typeof(DynamicAssemblyManager))
            {
                assemblyBuilder.Save(assemblyName.Name + ".dll");
            }
        }

        private static AssemblyName assemblyName;
        private static AssemblyBuilder assemblyBuilder;
        internal static readonly ModuleBuilder moduleBuilder;
        internal static readonly Module Module;

        static DynamicAssemblyManager()
        {
            assemblyName = new AssemblyName("NLiteDynamicAssembly");
            assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, $"{assemblyName.Name}.dll", true);
            Module = assemblyBuilder.GetModules().FirstOrDefault();
        }

        private static string CorrectTypeName(string typeName)
        {
            if (typeName.Length >= 1042)
            {
                typeName = $"type_{typeName.Substring(0, 900)}{Guid.NewGuid().ToString().Replace("-", "")}";
            }
            return typeName;
        }

        internal static TypeBuilder DefineType(string typeName, Type parent)
        {
            lock (typeof(DynamicAssemblyManager))
            {
                return moduleBuilder.DefineType(CorrectTypeName(typeName), TypeAttributes.Public, parent, null);
            }
        }
    }
}
