using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyNet.Tester.Models;
using EasyNet.Extensions;

namespace EasyNet.Tester
{
    internal class DescriptionTester
    {
        public static void Run()
        {
            var type = typeof(Product);
            var desc = type.Description();
            desc = type.GetMember("Category").FirstOrDefault()?.Description();
            var categoryType = type.GetMember("Category").FirstOrDefault()?.GetMemberType();

            List<byte> list = new List<byte>();
        }
    }
}
