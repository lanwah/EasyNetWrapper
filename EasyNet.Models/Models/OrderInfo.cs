using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Models.Models
// CLR版本：4.0.30319.42000
// 运行要求：4.0
// 文件名称：OrderInfo.cs
// 创建用户：lanwah
// 创建日期：2024/8/29 13:14:45
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Models.Models
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderID { get; set; }
        /// <summary>
        /// 订单日期时间
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderId"></param>
        public OrderInfo(int orderId)
        {
            this.OrderID = orderId;
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OrderInfo() : this(0)
        {

        }

        /// <summary>
        /// 设置订单ID
        /// </summary>
        /// <param name="orderId"></param>
        public void SetOrderID(int orderId)
        {
            this.OrderID = orderId;
        }
        /// <summary>
        /// 获取订单ID
        /// </summary>
        /// <returns></returns>
        public int GetOrderID()
        {
            return this.OrderID;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.OrderID}";
        }

        /// <summary>
        /// 订单列表
        /// </summary>
        public static List<OrderInfo> OrderList { get; set; } = new List<OrderInfo>()
        {
            new OrderInfo(1),
            new OrderInfo(2),
            new OrderInfo(3),
            new OrderInfo(4),
            new OrderInfo(5),
            new OrderInfo(6),
            new OrderInfo(7),
            new OrderInfo(8),
            new OrderInfo(9),
            new OrderInfo(10),
        };
    }
}
