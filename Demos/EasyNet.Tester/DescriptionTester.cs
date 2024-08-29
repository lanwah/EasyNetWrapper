using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extensions;
using System.Linq;
using EasyNet.Models.Models;

namespace EasyNet.Tester
{
    internal class DescriptionTester
    {
        public static void Run()
        {
            var type = typeof(Product);
            _ = type.Description();
            var categoryDesc = type.GetMember("Category").FirstOrDefault()?.Description();
            Console.WriteLine(categoryDesc);
            _ = type.GetMember("Category").FirstOrDefault()?.GetMemberType();
        }
    }
}
