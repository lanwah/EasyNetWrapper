using EasyNet.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：MethodMataData
// 创 建 者：lanwah
// 创建日期：2021/07/01 13:24:03
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
    internal class MethodMetaData
    {
        public MethodBase Method { get; private set; }
        public Type ReturnType { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }
        public Type[] ParameterTypes { get; private set; }

        public MethodMetaData(ConstructorInfo ctor)
        {
            Method = ctor;
            ReturnType = ctor.ReflectedType;
            InitParameters();
        }

        public MethodMetaData(MethodInfo method)
        {
            Method = method;
            ReturnType = method.ReturnType;
            InitParameters();
        }

        private void InitParameters()
        {
            Parameters = Method.GetParameters();
            ParameterTypes = Parameters.GetParameterTypes();
        }
    }
}
