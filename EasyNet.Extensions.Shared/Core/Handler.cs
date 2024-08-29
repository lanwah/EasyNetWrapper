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
// 文件名称：Handler.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 13:31:00
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
    /// 构造函数委托
    /// </summary>
    /// <param name="arguments">构造函数参数列表</param>
    /// <returns>返回创建的对象</returns>
    public delegate object ConstructorHandler(params object[] arguments);
    /// <summary>
    /// 函数委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="arguments">函数参数</param>
    /// <returns>函数返回值</returns>
    public delegate object MethodHandler(object target, params object[] arguments);
    /// <summary>
    /// 访问器委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <returns></returns>
    public delegate object GetHandler(object target);
    /// <summary>
    /// 设置器委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="value">设置器参数</param>
    public delegate void SetHandler(object target, object value);

    
}
