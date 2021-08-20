using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：DynamicMethodFactory
// 创 建 者：lanwah
// 创建日期：2021/07/01 11:09:57
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
    /// 通过反射动态方法调用
    /// </summary>
    public static class DynamicMethodFactory
    {
        /// <summary>
        /// 得到访问器委托
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static Getter GetGetter(this System.Reflection.MemberInfo member)
        {
            member.NotNullCheck(nameof(member));

            Getter getter = null;
            if (member.DeclaringType.IsValueType)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        getter = (target) => (member as FieldInfo).GetValue(target);
                        break;
                    case MemberTypes.Property:
                        getter = (target) => (member as PropertyInfo).GetValue(target, null);
                        break;
                    case MemberTypes.Method:
                        getter = (target) => (member as MethodInfo).Invoke(target, new object[] { });
                        break;
                }

            }
            else
            {
                getter = DefaultDynamicMethodFactory.CreateGetter(member);
            }

            return target =>
            {
                try
                {
                    return getter(target);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }
        /// <summary>
        /// 得到设置器委托
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns>返回设置器委托</returns>
        public static Setter GetSetter(this System.Reflection.MemberInfo member)
        {
            member.NotNullCheck(nameof(member));

            Setter setter = null;
            if (member.DeclaringType.IsValueType)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        setter = (target, value) => (member as FieldInfo).SetValue(target, value);
                        break;
                    case MemberTypes.Property:
                        setter = (target, value) => (member as PropertyInfo).SetValue(target, value, null);
                        break;
                    case MemberTypes.Method:
                        setter = (target, value) => (member as MethodInfo).Invoke(target, new object[] { value });
                        break;
                }
            }
            else
            {
                setter = DefaultDynamicMethodFactory.CreateSetter(member);
            }

            return (target, value) =>
            {
                setter?.Invoke(target, value);
            };
        }
        /// <summary>
        /// 得到函数委托（有返回值函数）
        /// </summary>
        /// <param name="method">方法对象</param>
        /// <returns>返回函数委托</returns>
        public static Method GetMethod(this System.Reflection.MethodInfo method)
        {
            method.NotNullCheck(nameof(method));

            var func = method.DeclaringType.IsValueType
                ? (target, args) => method.Invoke(target, args)
                : DefaultDynamicMethodFactory.CreateMethod(method);

            return (target, args) =>
            {
                if (args == null)
                {
                    args = new object[method.GetParameters().Length];
                }

                try
                {
                    return func(target, args);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }
        /// <summary>
        /// 得到默认无参构造函数委托
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DefaultConstructorHandler GetDefaultCreator(this Type type)
        {
            type.NotNullCheck(nameof(type));
            var ctor = DefaultDynamicMethodFactory.CreateDefaultConstructorMethod(type);

            DefaultConstructorHandler handler = () =>
            {
                try
                {
                    return ctor();
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };

            return handler;
        }
        /// <summary>
        /// 得到构造函数委托
        /// </summary>
        /// <param name="constructor">构造函数</param>
        /// <returns>返回构造函数委托</returns>
        public static ConstructorHandler GetCreator(this System.Reflection.ConstructorInfo constructor)
        {
            constructor.NotNullCheck(nameof(constructor));

            ConstructorHandler ctor = constructor.DeclaringType.IsValueType ?
                (args) => constructor.Invoke(args)
                : DefaultDynamicMethodFactory.CreateConstructorMethod(constructor);

            ConstructorHandler handler = args =>
            {
                if (args == null)
                {
                    args = new object[constructor.GetParameters().Length];
                }

                try
                {
                    return ctor(args);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };

            return handler;
        }
    }

    #region // 测试代码段
    //public class OrderInfo
    //{
    //    public OrderInfo(int orderId)
    //    {
    //        this.OrderID = orderId;
    //    }
    //    public OrderInfo() : this(0)
    //    {

    //    }

    //    public int OrderID { get; set; }

    //    public void SetOrderID(int orderId)
    //    {
    //        this.OrderID = orderId;
    //    }
    //    public int GetOrderID()
    //    {
    //        return this.OrderID;
    //    }

    //    /// <summary>
    //    /// 测试代码段
    //    /// </summary>
    //    public void Test()
    //    {
    //        var type = typeof(OrderInfo);
    //        var creator = type.GetDefaultCreator();
    //        var target = creator();
    //        var propertyInfo = type.GetProperty("OrderID");
    //        var setter = propertyInfo.GetSetter();
    //        setter(target, 1);
    //        var getter = propertyInfo.GetGetter();
    //        var orderId = getter(target);

    //        var constructorInfo = type.GetConstructor(new Type[] { typeof(int) });
    //        var creator2 = constructorInfo.GetCreator();
    //        var target2 = creator2(2);
    //        var methodInfo1 = type.GetMethod("SetOrderID");
    //        var methodInfo2 = type.GetMethod("GetOrderID");
    //        var proc1 = methodInfo2.GetMethod();
    //        var orderId2 = proc1(target2);
    //        methodInfo1.GetMethod()(target2, 6);
    //        var orderId3 = proc1(target2);
    //    }
    //}
    #endregion
}
