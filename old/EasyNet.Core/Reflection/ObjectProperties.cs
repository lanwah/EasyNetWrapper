using EasyNet.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

// ------------------------------------------------------------- //
// 版权所有：CopyRight (C) lanwah
// 项目名称：EasyNet.Core.Reflection
// 文件名称：ObjectProperties
// 创 建 者：lanwah
// 创建日期：2021/08/09 16:13:46
// 功能描述：
// 调用依赖：
// -------------------------------------------------------------
// 修 改 者：
// 修改时间：
// 修改原因：
// 修改描述：
// ------------------------------------------------------------- //

namespace EasyNet.Core.Reflection
{
    /// <summary>
    /// 对象属性
    /// </summary>
    public class ObjectProperties
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
        public string FixPrefix
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
        public string Desc
        {
            get; set;
        }
        /// <summary>
        /// FixPrefix + Field
        /// </summary>
        public string FullField
        {
            get
            {
                return $"{FixPrefix}.{Field}";
            }
        }
        /// <summary>
        /// 子属性
        /// </summary>
        public List<ObjectProperties> Children
        {
            get; set;
        }


        /// <summary>
        /// 获取对象属性信息，支持对属性中 ReflectionIngore<see cref="ReflectionIngoreAttribute"/>，Description<see cref="DescriptionAttribute"/>
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="prefix">前缀</param>
        /// <param name="desc">描述</param>
        /// <returns></returns>
        public static ObjectProperties GetProperties(Type type, string prefix, string desc = "")
        {
            var root = new ObjectProperties()
            {
                FixPrefix = prefix,
                DisplayName = prefix,
                Field = "",
                Desc = desc,
                DataType = type.Name,
                PropertyType = null,
                Children = new List<ObjectProperties>(),
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
                if (property.IsDefined(typeof(ReflectionIngoreAttribute), false))
                {
                    continue;
                }
                // 完整字段
                var fullField = propertyInfo.Field;
                if (!fullField.IsNullOrEmptyEx())
                {
                    fullField += ".";
                }
                fullField += property.Name;
                // 解析描述
                var desc = property.Description();

                var child = new ObjectProperties()
                {
                    PropertyType = property,
                    FixPrefix = propertyInfo.FixPrefix,
                    DisplayName = property.Name,
                    Field = fullField,
                    DataType = property.PropertyType.Name,
                    Desc = desc,
                    Children = new List<ObjectProperties>()
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
    public class ReflectionIngoreAttribute : Attribute
    {
        private bool _isIngore = true;
        private ReflectionIngoreAttribute(bool ingore)
        {
            this._isIngore = ingore;
        }

        public ReflectionIngoreAttribute() : this(true)
        {

        }
    }
}
