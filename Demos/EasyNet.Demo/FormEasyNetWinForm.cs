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
    public partial class FormEasyNetWinForm : Form
    {
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
            // 方式二，只设置 SourceItems，采用内部查找匹配算法，从头开始逐字匹配
            {
                IEnumerable<AutocompleteItem> items = PersonEx.PersonList.Select(c =>
                {
                    return c.AutocompleteItemBuilder();
                });
                this.historyTextBox1.SourceItems = items;
            }
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
