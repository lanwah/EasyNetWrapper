using EasyNet.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.Core
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：MethodMetaData.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 13:36:14
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core
{
    /// <summary>
    /// 方法的元数据信息
    /// </summary>
    public class MethodMetaData
    {
        /// <summary>
        /// 方法信息
        /// </summary>
        public MethodBase Method { get; private set; }
        /// <summary>
        /// 方法返回值类型
        /// </summary>
        public Type ReturnType { get; private set; }
        /// <summary>
        /// 方法参数列表
        /// </summary>
        public ParameterInfo[] Parameters { get; private set; }
        /// <summary>
        /// 方法参数类型列表
        /// </summary>
        public List<Type> ParameterTypes { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ctor"></param>
        public MethodMetaData(ConstructorInfo ctor)
        {
            this.Method = ctor;
            this.ReturnType = ctor.ReflectedType;
            this.InitParameters();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="method"></param>
        public MethodMetaData(MethodInfo method)
        {
            this.Method = method;
            this.ReturnType = method.ReturnType;
            this.InitParameters();
        }

        private void InitParameters()
        {
            this.Parameters = this.Method.GetParameters();
            this.ParameterTypes = this.Parameters.GetParameterTypes();
        }
    }
}
