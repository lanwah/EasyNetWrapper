using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using EasyNet.Extensions;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// CLR版本：4.0.30319.42000
// 运行要求：3.5
// 文件名称：ObjectProperties.cs
// 创建用户：lanwah
// 创建日期：2024/8/28 15:08:16
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修改用户：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Reflection
{
    /// <summary>
    /// 对象属性
    /// </summary>
    public partial class ObjectProperties
    {
        /// <summary>
        /// 属性类型
        /// </summary>
        public PropertyInfo PropertyType
        {
            get; set;
        }
        /// <summary>
        /// 固定前缀
        /// </summary>        
        public string Prefix
        {
            get; set;
        }
        /// <summary>
        /// 显示名/显示字段
        /// </summary>
        public string DisplayName
        {
            get; set;
        }
        /// <summary>
        /// 字段
        /// </summary>
        public string Field
        {
            get; set;
        }
        /// <summary>
        /// 类型
        /// </summary>        
        public string DataType
        {
            get; set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get; set;
        }
        /// <summary>
        /// Prefix + Field
        /// </summary>
        public string FullField
        {
            get
            {
                return $"{Prefix}.{Field}";
            }
        }
        /// <summary>
        /// 子属性
        /// </summary>
        public List<ObjectProperties> Children
        {
            get; set;
        }
    }

    public partial class ObjectProperties
    {
        /// <summary>
        /// 获取对象属性信息，支持对属性中 ReflectionIngore<see cref="ReflectionIgnoreAttribute"/>，Description<see cref="DescriptionAttribute"/>
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="prefix">前缀</param>
        /// <param name="displayName">显示名</param>
        /// <param name="desc">描述</param>
        /// <returns></returns>
        public static ObjectProperties GetProperties(Type type, string prefix = "", string displayName = "", string desc = "")
        {
            var root = new ObjectProperties()
            {
                Prefix = prefix,
                DisplayName = displayName,
                Field = "",
                Description = desc,
                DataType = type.Name,
                PropertyType = null,
#if NET8_0_OR_GREATER
                Children = [],
#else
                    Children = new List<ObjectProperties>(),
#endif
            };
            ObjectPropertyInformation(type, root);
            return root;
        }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyInfo"></param>
        private static void ObjectPropertyInformation(Type type, ObjectProperties propertyInfo)
        {
            var propertyInfos = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var property in propertyInfos)
            {
                // 忽略取值
                if (property.IsDefined(typeof(ReflectionIgnoreAttribute), false))
                {
                    continue;
                }
                // 完整字段
                var fullField = propertyInfo.Field;
                if (!fullField.IsNullOrEmpty())
                {
                    fullField += ".";
                }
                fullField += property.Name;
                // 解析描述
                var desc = property.Description();

                var child = new ObjectProperties()
                {
                    PropertyType = property,
                    Prefix = propertyInfo.Prefix,
                    DisplayName = property.Name,
                    Field = fullField,
                    DataType = property.PropertyType.Name,
                    Description = desc,
#if NET8_0_OR_GREATER
                    Children = [],
#else
                    Children = new List<ObjectProperties>(),
#endif

                };
                propertyInfo.Children.Add(child);

                var propertyType = property.PropertyType;
                if (propertyType.IsList())
                {
                    // List类型
                    propertyType = propertyType.GetProperty("Item").PropertyType;
                }
                else if (propertyType.IsArray)
                {
                    // 数组类型
                    propertyType = propertyType.GetElementType();
                }

                if (property.PropertyType.IsPrimitive || property.PropertyType.IsValueType || property.PropertyType == typeof(string))
                {
                    // 值类型，没有子节点
                }
                else
                {
                    // 复杂类型，Enumerables 类型
                    ObjectPropertyInformation(propertyType, child);
                }
            }
        }
    }

    /// <summary>
    /// 忽略反射解析特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ReflectionIgnoreAttribute : Attribute
    {
        /// <summary>
        /// 是否忽略反射解析
        /// </summary>
        public bool IsIgnore
        {
            get; set;
        } = false;
        private ReflectionIgnoreAttribute(bool ingore)
        {
            this.IsIgnore = ingore;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReflectionIgnoreAttribute() : this(true)
        {

        }
    }

    /// <summary>
    /// ObjectProperties 扩展方法
    /// </summary>
    public static class ObjectPropertiesExts
    {
        /// <summary>
        /// 获取对象属性信息，支持对属性中 ReflectionIngore<see cref="ReflectionIgnoreAttribute"/>，Description<see cref="DescriptionAttribute"/>
        /// </summary>
        /// <param name="this"></param>
        /// <param name="prefix">前缀</param>
        /// <param name="displayName">显示名</param>
        /// <param name="desc">描述</param>
        /// <returns></returns>
        public static ObjectProperties GetProperties(this Type @this, string prefix = "", string displayName = "", string desc = "")
        {
            return ObjectProperties.GetProperties(@this, prefix, displayName, desc);
        }
    }
}
