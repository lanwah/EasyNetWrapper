using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Models.Models
{
    /// <summary>
    /// 产品信息类
    /// </summary>
    public class Product
    {
        [Description("Id")]
        public int Id
        {
            get; set;
        }
        [Description("产品名称")]
        [DisplayName("ProductName")]
        public string Name
        {
            get; set;
        }
        [Description("产品分类")]
        public ProductCategory Category
        {
            get; set;
        }
        [Description("产品价格")]
        public decimal Price
        {
            get; set;
        }
    }

    public class ProductCategory
    {
        [Description("分类名称")]
        public string Name
        {
            get; set;
        }
        [Description("创建时间")]
        public DateTime CreateTime
        {
            get; set;
        }
    }
}
