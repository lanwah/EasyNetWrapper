using EasyNet.Extensions;
using EasyNet.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Tester
// CLR版本：4.0.30319.42000
// 运行要求：4.8
// 文件名称：EmitTester.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 13:43:07
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Tester
{
    public class EmitTester
    {
        public static void Run()
        {
            var type = typeof(OrderInfo);
            // 获取默认构造函数委托
            var creator = type.GetDefaultConstructorHandler();
            // 调用委托创建对象
            var target = creator();
            var propertyInfo = type.GetProperty("OrderID");
            // 获取属性的Get方法委托
            var getter = propertyInfo.GetGetHandler();
            // 调用委托获取OrderID属性值
            var orderId = getter(target);
            Console.WriteLine($"OrderID: {orderId}");

            // 获取属性的Set方法委托
            var setter = propertyInfo.GetSetHandler();
            // 调用委托设置OrderID属性值
            setter(target, 1);
            // 重新取值
            orderId = getter(target);
            Console.WriteLine($"OrderID: {orderId}");



            var constructorInfo = type.GetConstructor(new Type[] { typeof(int) });
            // 获取指定类型的构造函数委托
            var creator2 = constructorInfo.GetConstructorHandler();
            // 调用委托创建对象
            var target2 = creator2(6);
            var methodInfo1 = type.GetMethod("SetOrderID");
            var methodInfo2 = type.GetMethod("GetOrderID");
            // 获取指定方法的委托
            var proc1 = methodInfo2.GetMethodHandler();
            orderId = proc1(target2);
            Console.WriteLine($"OrderID: {orderId}");
            // 获取方法委托并调用委托设置值
            methodInfo1.GetMethodHandler()(target2, 8);
            orderId = proc1(target2);
            Console.WriteLine($"OrderID: {orderId}");
        }
    }
}
