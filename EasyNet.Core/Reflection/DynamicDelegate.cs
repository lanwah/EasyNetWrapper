using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：DynamicDelegate
// 创 建 者：lanwah
// 创建日期：2021/07/01 11:14:23
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
    /// 访问器委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <returns></returns>
    public delegate object Getter(object target);
    /// <summary>
    /// 设置器委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="value">设置器参数</param>
    public delegate void Setter(object target, object value);
    /// <summary>
    /// 过程委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="arguments">过程参数</param>
    public delegate void Proc(object target, params object[] arguments);
    /// <summary>
    /// 函数委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="arguments">函数参数</param>
    /// <returns>函数返回值</returns>
    public delegate object Meth(object target, params object[] arguments);
    /// <summary>
    /// 缺省构造函数委托
    /// </summary>
    /// <returns></returns>
    public delegate object DefaultConstructorHandler();
    /// <summary>
    /// 构造函数委托
    /// </summary>
    /// <param name="arguments">构造函数参数列表</param>
    /// <returns>返回创建的对象</returns>
    public delegate object ConstructorHandler(params object[] arguments);
}
