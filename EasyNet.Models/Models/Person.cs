using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Models.Models
// CLR版本：4.0.30319.42000
// 运行要求：5.0
// 文件名称：Person.cs
// 创建用户：lanwah
// 创建日期：2024/8/23 14:24:04
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
    /// 人员信息
    /// </summary>
    public class Person
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { set; get; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { set; get; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;


        /// <summary>
        /// 人员列表
        /// </summary>
        public static List<Person> PersonList { get; set; } = new List<Person>
        {
            new Person
            {
                Name = "P1", Age = 18, Gender = "Male"

            },
            new Person
            {
                Name = "P2", Age = 19, Gender = "Mele",
            },
            new Person
            {
                Name = "P3", Age = 18, Gender = "Mafe"

            },
            new Person
            {
                Name = "P4", Age = 19, Gender = "Mald",
            },
            new Person
            {
                Name = "P5", Age = 17,Gender = "Female",
            }
        };
    }
}
