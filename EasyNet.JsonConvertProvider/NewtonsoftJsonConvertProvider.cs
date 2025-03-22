using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.JsonConvertProvider
{
    /// <summary>
    /// Newtonsoft 转换器提供程序
    /// </summary>
    public class NewtonsoftJsonConvertProvider : IJsonConvertProvider
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SerializeObject(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
