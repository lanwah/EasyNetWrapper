using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using System.Xml;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Extensions.Shared.Utils
// CLR版本：4.0.30319.42000
// 运行要求：$targetframeworkversion$
// 文件名称：StringXmlExts.cs
// 创建用户：lanwah
// 创建日期：2024/9/2 17:03:02
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Extensions
{
    /// <summary>
    /// String 类型 xml 处理相关扩展方法
    /// </summary>
    public static partial class StringExts
    {
        /// <summary>
        /// 获取Xml节点值，找不到返回空字符串(获取第一个匹配项的值)
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static string GetXmlNodeValue(this string xml, string nodeName)
        {
            // 使用正则表达式获取节点值，格式例如：<enable>false</enable>
            var match = Regex.Match(xml, nodeName.GetXmlNodeMatchPattern());
            if (match.Success)
            {
                // match.Groups[0].Value - <enable>false</enable>
                // match.Groups[1].Value - false
                return match.Groups[1].Value;
            }

            return string.Empty;
        }
        /// <summary>
        /// 获取Xml节点值，找不到返回空字符串(获取指定索引的匹配项的值)
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetXmlNodeValue(this string xml, string nodeName, int index)
        {
            // 使用正则表达式获取节点值，格式例如：<enable>false</enable>
            var matches = Regex.Matches(xml, nodeName.GetXmlNodeMatchPattern());
            Match match = null;
            if ((index >= 0) && (index < matches.Count))
            {
                match = matches[index];
            }

            if (match != null && match.Success)
            {
                // match.Groups[0].Value - <enable>false</enable>
                // match.Groups[1].Value - false
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// 替换Xml节点值，找到了就替换(全部替换)，没找到就返回原xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="newValue">节点值</param>
        /// <returns></returns>
        public static string ReplaceXmlNode(this string xml, string nodeName, string newValue)
        {
            // 使用正则表达式替换节点值，格式例如：<enable>false</enable>
            var pattern = nodeName.GetXmlNodeMatchPattern();
            return Regex.Replace(xml, pattern, $"<{nodeName}>{newValue}</{nodeName}>");
        }
        /// <summary>
        /// 替换Xml节点值，找到了就替换(第一个匹配项替换)，没找到就返回原xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceFirstMatchXmlNode(this string xml, string nodeName, string newValue)
        {
            // 使用正则表达式替换节点值，格式例如：<enable>false</enable>
            return xml.ReplaceXmlNode(nodeName, newValue, 1);
        }
        /// <summary>
        /// 替换Xml节点值，找到了就替换(指定数量的匹配项替换)，没找到就返回原xml
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="nodeName"></param>
        /// <param name="newValue"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ReplaceXmlNode(this string xml, string nodeName, string newValue, int count)
        {
            // 使用正则表达式替换节点值，格式例如：<enable>false</enable>
            var pattern = GetXmlNodeMatchPattern(nodeName);
            var regex = new Regex(pattern);

            count = count < 1 ? 1 : count;
            return regex.Replace(xml, $"<{nodeName}>{newValue}</{nodeName}>", count);
        }

        /// <summary>
        /// 获取Xml节点匹配模式
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        private static string GetXmlNodeMatchPattern(this string nodeName)
        {
            return $"<{nodeName}>(.*?)</{nodeName}>";
        }


        /// <summary>
        /// 获取xml元素的值
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xpath">Integrate/Setting/enable或Integrate/API/enable[@from!="Harvard"](from 属性不等于 "Harvard" 的所有 enable 元素)</param>
        /// <see keyword="XPath 语法" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256471(v=vs.100)"/>
        /// <see keyword="XPath 示例" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)"/>
        /// <returns></returns>
        public static string GetElementValue(this string xml, string xpath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var node = xmlDoc.SelectSingleNode(xpath);
            if (node.IsNotNull())
            {
                return string.Empty;
            }

            return node.InnerText;
        }
        /// <summary>
        /// 获取xml元素的值，带命名空间的处理
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="action"></param>
        /// <param name="xpath">Integrate/Setting/enable或Integrate/API/enable[@from!="Harvard"](from 属性不等于 "Harvard" 的所有 enable 元素)</param>
        /// <see keyword="带命名空间的处理" href="https://www.cnblogs.com/cang12138/p/6133734.html"/>
        /// <see keyword="XPath 语法" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256471(v=vs.100)"/>
        /// <see keyword="XPath 示例" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)"/>
        /// <returns></returns>
        public static string GetElementValue(this string xml, Action<XmlNamespaceManager> action, string xpath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            //注册命名空间
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            #region 带命名空间的处理，xml中有名命名空间前缀则采用xml中的前缀，否则自己命名前缀并在 xpath 中的每个节点前添加前缀，详见例子
            //nsmgr.AddNamespace("x", "http://www.example.com/Integrate");
            //@"<?xml version=""1.0"" encoding=""UTF-8""?>
            //<Integrate xmlns=""http://www.example.com/Integrate"">
            //    <Setting>
            //        <enable>false1</enable>
            //        <certificateType>digest/WSSE</certificateType>
            //        <timeVerifyEnabled>false</timeVerifyEnabled>
            //    </Setting>
            //    <API>
            //        <enable>true2</enable>
            //        <certificateType>digest/WSSE</certificateType>
            //        <timeVerifyEnabled>false</timeVerifyEnabled>
            //    </API>
            //</Integrate>";
            //var node = xmlDoc.SelectSingleNode("x:Integrate/x:Setting/x:enable", nsmgr);
            #endregion

            action?.Invoke(nsmgr);
            var node = xmlDoc.SelectSingleNode(xpath);
            if (node.IsNotNull())
            {
                return string.Empty;
            }

            return node.InnerText;
        }

        /// <summary>
        /// 修改xml元素的值
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xpath">Integrate/Setting/enable或Integrate/API/enable[@from!="Harvard"](from 属性不等于 "Harvard" 的所有 enable 元素)</param>
        /// <param name="newValue"></param>
        /// <see keyword="XPath 语法" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256471(v=vs.100)"/>
        /// <see keyword="XPath 示例" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)"/>
        /// <returns></returns>
        public static string ModifyXml(this string xml, string xpath, string newValue)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var node = xmlDoc.SelectSingleNode(xpath);
            if (node.IsNotNull())
            {
                return xml;
            }

            // 修改节点值
            node.InnerText = newValue;
            return xmlDoc.GetFormatXmlString();
        }
        /// <summary>
        /// 修改xml元素的值，带命名空间的处理
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="action"></param>
        /// <param name="xpath">Integrate/Setting/enable或Integrate/API/enable[@from!="Harvard"](from 属性不等于 "Harvard" 的所有 enable 元素)</param>
        /// <param name="newValue"></param>
        /// <see keyword="带命名空间的处理" href="https://www.cnblogs.com/cang12138/p/6133734.html"/>
        /// <see keyword="XPath 语法" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256471(v=vs.100)"/>
        /// <see keyword="XPath 示例" href="https://learn.microsoft.com/zh-cn/previous-versions/dotnet/netframework-4.0/ms256086(v=vs.100)"/>
        /// <returns></returns>
        public static string ModifyXml(this string xml, Action<XmlNamespaceManager> action, string xpath, string newValue)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            //注册命名空间
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            #region 带命名空间的处理，xml中有名命名空间前缀则采用xml中的前缀，否则自己命名前缀并在 xpath 中的每个节点前添加前缀，详见例子
            //nsmgr.AddNamespace("x", "http://www.example.com/Integrate");
            //@"<?xml version=""1.0"" encoding=""UTF-8""?>
            //<Integrate xmlns=""http://www.example.com/Integrate"">
            //    <Setting>
            //        <enable>false1</enable>
            //        <certificateType>digest/WSSE</certificateType>
            //        <timeVerifyEnabled>false</timeVerifyEnabled>
            //    </Setting>
            //    <API>
            //        <enable>true2</enable>
            //        <certificateType>digest/WSSE</certificateType>
            //        <timeVerifyEnabled>false</timeVerifyEnabled>
            //    </API>
            //</Integrate>";
            //var node = xmlDoc.SelectSingleNode("x:Integrate/x:Setting/x:enable", nsmgr);
            #endregion

            action?.Invoke(nsmgr);
            var node = xmlDoc.SelectSingleNode(xpath);
            if (node.IsNotNull())
            {
                return xml;
            }

            // 修改节点值
            node.InnerText = newValue;
            return xmlDoc.GetFormatXmlString();
        }

        /// <summary>
        /// 获取缩进的Xml字符串
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <see keyword="C# 格式化XML方法" href="https://www.cnblogs.com/code1992/p/11461480.html"/>
        /// <returns></returns>
        private static string GetFormatXmlString(this XmlDocument xmlDoc)
        {
            // 格式化Xml, 并返回
            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };
            //// XmlTextWriter
            //var stream = new MemoryStream();
            //using (var writer = XmlWriter.Create(stream, settings))
            //{
            //    xmlDoc.Save(writer);
            //}
            //stream.Position = 0;
            //var reader = new StreamReader(stream, Encoding.UTF8);
            //var newXml = reader.ReadToEnd();

            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder, settings))
            {
                xmlDoc.Save(writer);
            }
            var newXml = builder.ToString();

            return newXml;
        }
    }
}
