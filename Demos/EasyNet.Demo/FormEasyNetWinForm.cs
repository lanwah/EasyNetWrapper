using EasyNet.Extensions;
using EasyNet.Models.Models;
using EasyNet.WinForm.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyNet.Demo
{
    /// <summary>
    /// historyTextBox1 - 支持选择与自定义输入；
    /// historyTextBox2 - 支持历史记录输入，每次自定义输入回车后加入历史记录列表；
    /// </summary>
    public partial class FormEasyNetWinForm : Form
    {
        private List<string> HistoryTextList
        {
            get; set;
        } = new List<string>();

        public FormEasyNetWinForm()
        {
            InitializeComponent();
        }

        private void FormEasyNetWinForm_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            //// 方式一，通过委托自定义进行匹配
            //{
            //    AutocompleteItem[] Callback(string keyword)
            //    {
            //        // 自定义查找
            //        var persons = (from r in PersonEx.PersonList where r.Gender.ToLower() == (keyword) select r).ToArray();
            //        if (null == persons)
            //        {
            //            return new AutocompleteItem[0];
            //        }

            //        // 转换
            //        IEnumerable<AutocompleteItem> items = persons.Select(c =>
            //        {
            //            return c.AutocompleteItemBuilder();
            //        });

            //        return items.ToArray();
            //    }
            //    this.historyTextBox1.SourceItems = new AutocompleteItem[0];
            //    this.historyTextBox1.SearchCallback = Callback;
            //}

            // 方式二，只设置 SourceItems，采用内部查找匹配算法，从头开始逐字匹配
            {
                IEnumerable<AutocompleteItem> items = PersonEx.PersonList.Select(c =>
                {
                    return c.AutocompleteItemBuilder();
                });
                this.historyTextBox1.SourceItems = items;
            }

            //// 方式三，查找与转换分开
            //{
            //    Person[] CallBack(string keyword)
            //    {
            //        var persons = (from r in PersonEx.PersonList where r.Gender.ToLower() == (keyword) select r).ToArray();
            //        if (null == persons)
            //        {
            //            persons = new PersonEx[0];
            //        }
            //        return persons;
            //    };
            //    AutocompleteItem Converter(Person p)
            //    {
            //        var item = new AutocompleteItem()
            //        {
            //            Key = p.Gender,
            //            ItemText = $"{p.Name}",
            //            DisplayText = $"{p.Name} {p.Gender} {p.Age}",
            //            ToolTipTitle = "详情",
            //            ToolTipText = $"姓名 {p.Name},性别 {p.Gender},年龄 {p.Age}",
            //            Tag = p
            //        };
            //        return item;
            //    }
            //    this.historyTextBox1.SetSearchCallbackArgs<Person>(CallBack, Converter);
            //}

            // 初始化选中项
            this.historyTextBox1.SelectedItem = PersonEx.PersonList[1].AutocompleteItemBuilder();

            //historyTextBox2

            // 方式一，通过委托自定义进行匹配
            {
                AutocompleteItem[] Callback(string keyword)
                {
                    // 自定义查找
                    var matched = this.HistoryTextList.Where(a => a.Contains(keyword) || GetFirstPinYin(a).Contains(keyword.ToUpper())).ToArray();
                    if (null == matched)
                    {
                        return new AutocompleteItem[0];
                    }

                    // 转换
                    IEnumerable<AutocompleteItem> items = matched.Select(c =>
                    {
                        return c.ToAutocompleteItem();
                    });

                    return items.ToArray();
                }
                this.historyTextBox2.SourceItems = new AutocompleteItem[0];
                this.historyTextBox2.SearchCallback = Callback;
            }
        }

        private void HistoryTextBox1_DataChanged(object sender, EventArgs e)
        {
            if (this.historyTextBox1.SelectedItem != null)
            {
                var person = this.historyTextBox1.SelectedItem.Tag as Person;
                Console.WriteLine(person);
            }
        }

        private void HistoryTextBox2_DataChanged(object sender, EventArgs e)
        {
            if (this.historyTextBox2.SelectedItem != null)
            {
                var input = this.historyTextBox2.SelectedItem.DisplayText;
                Console.WriteLine(input);
                if (!input.IsNullOrEmpty() && !this.HistoryTextList.Contains(input))
                {
                    this.HistoryTextList.Add(input);
                }
            }
        }


        /// <summary>
        /// 获取汉字拼首字母（大写）
        /// </summary>
        /// <param name="chinese">汉子词组</param>
        /// <param name="encodingName">编码名称</param>
        /// <returns></returns>
        public static string GetFirstPinYin(string chinese, string encodingName = "GB2312")
        {
            if (string.IsNullOrEmpty(chinese))
            {
                return string.Empty;
            }
            return NPinyin.Pinyin.GetInitials(chinese, Encoding.GetEncoding(encodingName));
        }
    }

    public class PersonEx : Person, IAutoCompleteConverter
    {
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


        public static List<PersonEx> PersonList = new List<PersonEx>
        {
            new PersonEx
            {
                Name = "P1", Age = 18, Gender = "Male"

            },
            new PersonEx
            {
                Name = "P2", Age = 19, Gender = "Mele",
            },
            new PersonEx
            {
                Name = "P3", Age = 18, Gender = "Mafe"

            },
            new PersonEx
            {
                Name = "P4", Age = 19, Gender = "Mald",
            },
            new PersonEx
            {
                Name = "P5", Age = 17,Gender = "Female",
            }
        };
    }
}
