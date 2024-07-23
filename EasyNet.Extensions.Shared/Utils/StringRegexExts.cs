using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extension.Utils
// 文件名称：IntPtrExtn
// 创 建 者：lanwah
// 创建日期：2022/7/2 9:41:18
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// Regex处理string匹配相关的扩展方法
    /// </summary>
    public static partial class StringExts
    {
        /*************************************************************************************************************************
         * 
         * 1、正则表达式在线测试工具：https://www.osgeo.cn/app/sb207
         * 2、Regex 类：https://learn.microsoft.com/zh-cn/dotnet/api/system.text.regularexpressions.regex?view=net-7.0   
         * 3、正则表达式——详情版+常用表达式：https://blog.csdn.net/BLWY_1124/article/details/127133108?csdn_share_tail=%7B%22type%22%3A%22blog%22%2C%22rType%22%3A%22article%22%2C%22rId%22%3A%22127133108%22%2C%22source%22%3A%22BLWY_1124%22%7D
         *
         *************************************************************************************************************************/

        /// <summary>
        /// 使用<see cref="Regex"/>判断输入字符串中是否有<paramref name="pattern"/>所指定的正则表达式匹配的项（部分匹配）
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <see desc="正则表达式在线测试工具" href="https://www.osgeo.cn/app/sb207"/>
        /// <see desc="Regex 类" href="https://learn.microsoft.com/zh-cn/dotnet/api/system.text.regularexpressions.regex?view=net-7.0"/>
        /// <returns>true - 匹配，否则不匹配</returns>
        public static bool IsFind(this string @this, string pattern)
        {
            // 参数检查
            pattern.ThrowIfNull(nameof(pattern));

            // 匹配正则表达式
            if (@this.IsNullOrEmpty())
            {
                return false;
            }

            // 匹配正则表达式
            return Regex.IsMatch(@this, pattern);
        }
        /// <summary>
        /// 使用<see cref="Regex"/>判断输入字符串与<paramref name="pattern"/>所指定的正则表达式是否完全匹配
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <see desc="正则表达式在线测试工具" href="https://www.osgeo.cn/app/sb207"/>
        /// <see desc="Regex 类" href="https://learn.microsoft.com/zh-cn/dotnet/api/system.text.regularexpressions.regex?view=net-7.0"/>
        /// <returns>true - 匹配，否则不匹配</returns>
        public static bool IsMatch(this string @this, string pattern)
        {
            // 参数检查
            pattern.ThrowIfNull(nameof(pattern));

            // 匹配正则表达式
            if (@this.IsNullOrEmpty())
            {
                return false;
            }

            var boundaryWord = @"\b";
            var patternBuilder = new StringBuilder();
            //if (pattern.IndexOf(boundaryWord) != 0)
            if (!pattern.StartsWith(boundaryWord))
            {
                // 开始位置未找到，则加上
                patternBuilder.Append(boundaryWord);
            }
            patternBuilder.Append(pattern);
            if (pattern.LastIndexOf(boundaryWord) != (pattern.Length - 2))
            {
                // 末尾位置未找到，则加上
                patternBuilder.Append(boundaryWord);
            }

            // 匹配正则表达式
            return @this.IsFind(patternBuilder.ToString());
        }
        /// <summary>
        /// 使用<paramref name="pattern"/>所指定的正则表达式拆分输入字符串
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <returns>拆分后的字符串数组</returns>
        public static string[] Split(this string @this, string pattern)
        {
            // 参数检查
            pattern.ThrowIfNull(nameof(pattern));

            // 执行分割
            if (@this.IsNullOrEmpty())
            {
                return new string[0];
            }

            return Regex.Split(@this, pattern);
        }
        /// <summary>
        /// 从输入字符串中的第一个字符开始，用替换字符串替换指定的正则表达式模式的所有匹配项。
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="replacement">用于替换的字符串</param>
        /// <returns>返回替换处理后的字符串</returns>
        public static string Replace(this string @this, string pattern, string replacement)
        {
            if (@this.IsNullOrEmpty())
            {
                return @this;
            }

            // 参数检查
            pattern.ThrowIfNull(nameof(pattern));

            //// 执行替换

#if NETCOREAPP2_0_OR_GREATER
            replacement ??= "";
#else
            replacement = replacement ?? "";
#endif

            return Regex.Replace(@this, pattern, replacement);
        }
        /// <summary>
        /// 从输入字符串中的第一个字符开始，用<paramref name="dealer"/>所指定的处理函数替换指定的正则表达式模式的所有匹配项。
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="pattern">正则表达式</param>
        /// <param name="dealer">字符串替换处理函数，输入为正则表达式匹配到的key，返回为key对应的替换值。</param>
        /// <returns>返回替换处理后的字符串</returns>
        public static string Replace(this string @this, string pattern, Func<string, string> dealer)
        {
            if (@this.IsNullOrEmpty())
            {
                return @this;
            }

            // 参数检查
            pattern.ThrowIfNull(nameof(pattern));

            // 执行替换
            if (dealer.IsNull())
            {
                return @this;
            }
            @this = Regex.Replace(@this, pattern, new MatchEvaluator(match =>
            {
                var value = dealer?.Invoke(match.Groups[1].Value);
                return value.IsNull() ? match.Value : value;
            }));
            return @this;
        }
        /// <summary>
        /// 把字符串中"{varName}"形式的值替换成对应的数值
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="dealer">字符串替换处理函数，输入为正则表达式匹配到的key，返回为key对应的替换值。</param>
        /// <returns>处理后的字符串</returns>
        public static string Replace(this string @this, Func<string, string> dealer)
        {
            var pattern = @"\{(.*?)\}";

            return @this.Replace(pattern, dealer);
        }

        /// <summary>
        /// 计算字符串的字节长度，一个汉字字符将被计算为两个字符
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>返回字符串的字节长度</returns>
        public static int GetByteCount(this string @this)
        {
            if (@this.IsNullOrEmpty())
            {
                return 0;
            }

            // 计算字符串长度
            return Regex.Replace(@this, @"[\u4e00-\u9fa5/g]", "aa").Length;
        }
        /// <summary>
        /// 判断输入的字符串是否为一个有效的IP地址
        /// </summary>
        /// <see desc="验证IP地址" href="https://blog.csdn.net/weixin_48703800/article/details/126848554"/>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsIPv4(this string @this)
        {
            var pattern = @"\b([1-9]?\d|1\d{2}|2[0-4]\d|25[0-5])(\.([1-9]?\d|1\d{2}|2[0-4]\d|25[0-5])){3}$\b";

            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为一个有效的IPv6地址
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsIPV6(this string @this)
        {
            /* *******************************************************************
             * 1、通过“:”来分割字符串看得到的字符串数组长度是否小于等于8
             * 2、判断输入的IPV6字符串中是否有“::”。
             * 3、如果没有“::”采用 ^([\da-f]{1,4}:){7}[\da-f]{1,4}$ 来判断
             * 4、如果有“::” ，判断"::"是否止出现一次
             * 5、如果出现一次以上 返回false
             * 6、^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$
             * ******************************************************************/
            var temp = @this;
            var strs = temp.Split(':');
            if (strs.Length > 8)
            {
                return false;
            }
            var count = GetStringCount(@this, "::");

            // 匹配正则表达式
            string pattern;
            if (count > 1)
            {
                return false;
            }
            else if (count == 0)
            {
                pattern = @"^([\da-f]{1,4}:){7}[\da-f]{1,4}$";
                return @this.IsMatch(pattern);
            }
            else
            {
                pattern = @"^([\da-f]{1,4}:){0,5}::([\da-f]{1,4}:){0,5}[\da-f]{1,4}$";
                return @this.IsMatch(pattern);
            }
        }
        /// <summary>
        /// 判断输入的字符串是否为一个超链接
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsURL(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"\b[a-zA-z]+://[^\s]*\b";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为字符或数字
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsLetterOrNum(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^[A-Za-z0-9]+$";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为一个合法的Email地址
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsEmail(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否只包含英文字母
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsLetter(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^[A-Za-z]+$";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为数字，可以匹配整数和浮点数
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsNumber(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^-?\d+$|^(-?\d+)(\.\d+)?$";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为固定电话（匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，也可以不用，区号与本地号间可以用连字号或空格间隔，也可以没有间隔）
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsTelephone(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^\(0\d{2}\)[- ]?\d{8}$|^0\d{2}[- ]?\d{8}$|^\(0\d{3}\)[- ]?\d{7}$|^0\d{3}[- ]?\d{7}$";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为移动电话
        /// </summary>
        /// <see desc="电话号码正则表达式（标准）" href="https://blog.csdn.net/gxzhaha/article/details/108115777"/>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsMobilePhone(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^1([358][0-9]|4[579]|66|7[0135678]|9[89])[0-9]{8}$";
            return @this.IsMatch(pattern);
        }
        /// <summary>
        /// 判断输入的字符串是否为汉字
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <returns>true - 是，否则不是</returns>
        public static bool IsChineseCh(this string @this)
        {
            // 匹配正则表达式
            var pattern = @"^[\u4e00-\u9fa5]+$";
            return @this.IsMatch(pattern);
        }

        /// <summary>
        /// 判断指定字符串<paramref name="value"/>在输入字符串中出现的次数
        /// </summary>
        /// <param name="this">输入字符串</param>
        /// <param name="value">指定字符串</param>
        /// <param name="startIndex">搜索的索引，从0开始</param>
        /// <returns>出现的次数</returns>
        public static int GetStringCount(this string @this, string value, int startIndex = default)
        {
            var index = @this.IndexOf(value, startIndex);
            if (index != -1)
            {
                return 1 + @this.GetStringCount(value, index + value.Length);
            }
            else
            {
                return 0;
            }
        }
    }
}
