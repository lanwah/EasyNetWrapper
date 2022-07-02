using EasyNet.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Test.Model
// 文件名称：Person
// 创 建 者：lanwah
// 创建日期：2021/02/23 16:05:48
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Test.Model
{
    internal class Person : IAutoCompleteConverter
    {
        public string Name { set; get; }
        public int Age { set; get; }
        public string Gender { set; get; }

        public AutocompleteItem AutocompleteItemBuilder()
        {
            var item = new AutocompleteItem()
            {
                Key = this.Gender,
                ItemText = $"{this.Name}",
                DisplayText = $"{this.Name} {this.Gender} {this.Age}",
                ToolTipTitle = "详情",
                ToolTipText = $"name is {this.Name},Gender is {this.Gender},Age is {this.Age}",
                Tag = this
            };

            return item;
        }

        public override string ToString() => Name;
    }

    
}
