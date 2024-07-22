using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Extension;
using EasyNet.Tester.Models;

namespace EasyNet.Tester
{
    internal class DescriptionTester
    {
        public static void Run()
        {
            var type = typeof(Product);
            var desc = type.Description();
            var aa = type.GetMember("Category").FirstOrDefault()?.GetMemberType();

        }
    }
}
