using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace EasyNet.Extensions
{
    // https://mp.weixin.qq.com/s?__biz=MzIxMTUzNzM5Ng==&mid=2247510505&idx=2&sn=cba9ba32d24070d1fc71028db1473b85&chksm=96214cf9b6fbca195ba0b031bc35df9cf25df8581fe25c17674505bd40557d70965d62b18774&scene=126&sessionid=1739753079#rd
    internal class Sscanf
    {
        /// <summary>
        /// 解析字符串，返回解析结果。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="format"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static int Scan(string input, string format, out object[] results)
        {
            results = new object[0];
            var regexPattern = new StringBuilder("^"); // 匹配字符串开始
            var types = new List<Type>();
            int index = 0;
            int length = format.Length;

            while (index < length)
            {
                if (format[index] == '%')
                {
                    index++;
                    if (index >= length)
                        throw new FormatException("Invalid format string: % at end.");

                    bool skip = false;
                    if (format[index] == '*')
                    {
                        skip = true;
                        index++;
                        if (index >= length)
                            throw new FormatException("Invalid format string: * without type.");
                    }

                    // 解析宽度
                    int width = 0;
                    while (index < length && char.IsDigit(format[index]))
                    {
                        width = width * 10 + (format[index] - '0');
                        index++;
                    }

                    if (index >= length)
                        throw new FormatException("Invalid format string: incomplete specifier.");

                    char specifier = format[index];
                    index++;

                    string pattern;
                    Type type;

                    switch (specifier)
                    {
                        case '%': // 匹配%字符
                            regexPattern.Append("%");
                            continue;
                        case 'd':
                        case 'i': // 整数（包括负数）
                            pattern = width > 0 ? $"-?\\d{{1,{width}}}" : "-?\\d+";
                            type = typeof(int);
                            break;
                        case 'u': // 无符号整数
                            pattern = width > 0 ? $@"\d{{1,{width}}}" : @"\d+";
                            type = typeof(uint);
                            break;
                        case 'f':
                        case 'g':
                        case 'e': // 浮点数
                            // () - 定义的是一个捕获组，可以提取出子表达式的值；(?:) - 定义的是一个非捕获组，不提取子表达式的值
                            //pattern = width > 0 ? $@"-?\d{{1,{width}}}(\.\d{{1,{width}}})?([eE][+-]?\d+)?" : @"-?\d+(\.\d+)?([eE][+-]?\d+)?";
                            pattern = width > 0 ? $@"-?\d{{1,{width}}}(?:\.\d{{1,{width}}})?(?:[eE][+-]?\d+)?" : @"-?\d+(?:\.\d+)?(?:[eE][+-]?\d+)?";
                            //pattern = width > 0 ? $@"-?\d+.?\d*" : @"-?\d+.?\d*";
                            type = typeof(float);
                            break;
                        case 's': // 字符串（非空白字符）
                            pattern = width > 0 ? $@"\S{{1,{width}}}" : @"\S+";
                            type = typeof(string);
                            break;
                        case 'c': // 字符（包括空格，固定宽度）
                            if (width == 0) width = 1;
                            pattern = $".{{{width}}}";
                            type = typeof(string); // 作为字符串返回，可能多个字符
                            break;
                        default:
                            throw new NotSupportedException($"Format specifier '%{specifier}' is not supported.");
                    }

                    if (!skip)
                    {
                        regexPattern.Append($"({pattern})");
                        //regexPattern.Append($@"\(?({pattern})\)?");
                        types.Add(type);
                    }
                    else
                    {
                        regexPattern.Append($"(?:{pattern})");
                    }
                }
                else if (char.IsWhiteSpace(format[index]))
                {
                    // 匹配任意数量的空白字符
                    regexPattern.Append(@"\s*");
                    index++;
                }
                else
                {
                    // 转义特殊字符并匹配原样字符
                    regexPattern.Append(Regex.Escape(format[index].ToString()));
                    index++;
                }
            }

            regexPattern.Append("$"); // 匹配字符串结束

            var match = Regex.Match(input, regexPattern.ToString());
            //var match = Regex.Match(input, regexPattern.ToString(), RegexOptions.ExplicitCapture);
            if (!match.Success)
            {
                return 0; // 无匹配项
            }

            var parsedResults = new List<object>();
            var groupMatchIndex = 1; // 匹配组从1开始
            for (int i = 0; i < types.Count; i++)
            {
                string valueStr = match.Groups[groupMatchIndex].Value; // 第0组是整个匹配，捕获组从1开始
                Type type = types[i];

                try
                {
                    if (type == typeof(int))
                    {
                        parsedResults.Add(int.Parse(valueStr, CultureInfo.InvariantCulture));
                        groupMatchIndex += 1;
                    }
                    else if (type == typeof(uint))
                    {
                        parsedResults.Add(uint.Parse(valueStr, CultureInfo.InvariantCulture));
                        groupMatchIndex += 1;
                    }
                    else if (type == typeof(float))
                    {
                        parsedResults.Add(float.Parse(valueStr, CultureInfo.InvariantCulture));
                        // 使用捕获组进行处理浮点数时此处需要跳过两个捕获组，因为前面已经提取了整数部分
                        // 第一个捕获组的内容（整个浮点数字）。
                        // 第二个捕获组的内容（小数部分，如果存在）。
                        // 第三个捕获组的内容（科学计数法部分，如果存在）。
                        // groupMatchIndex += 3; 

                        groupMatchIndex += 1;
                    }
                    else if (type == typeof(string))
                    {
                        parsedResults.Add(valueStr);
                        groupMatchIndex += 1;
                    }
                }
                catch (FormatException)
                {
                    // 转换失败，返回已解析的部分
                    break;
                }
            }

            results = parsedResults.ToArray();
            return parsedResults.Count;
        }

        /*
        /// <summary>
        /// 使用例子
        /// </summary>
        private void CallDemo()
        {
            var input = "42 -3.14 9.87e5 Hello 12345";
            //input = "42 -3.14 9.875 Hello 12345";
            var format = "%d %f %f %s %*d"; // 跳过最后一个整数
            format = "%d %f %f %s %d";

            object[] results;
            var count = Sscanf.Scan(input, format, out results);

            Console.WriteLine($"成功解析 {count} 项:");
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
        */
    }
}
