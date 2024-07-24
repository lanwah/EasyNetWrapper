using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Tester.Models;
using EasyNet.Extensions;
using System.Linq;

namespace EasyNet.Tester
{
    internal class DescriptionTester
    {
        public static void Run()
        {
            var type = typeof(Product);
            _ = type.Description();
            _ = type.GetMember("Category").FirstOrDefault()?.Description();
            _ = type.GetMember("Category").FirstOrDefault()?.GetMemberType();
        }
    }
}
