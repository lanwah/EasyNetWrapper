#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace EasyNet.JsonConvertProvider
{
    /// <summary>
    /// System.Text.Json 转换器提供程序
    /// </summary>
    public class TextJsonConvertProvider : IJsonConvertProvider
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string SerializeObject(object value)
        {
            return JsonSerializer.Serialize(value);
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

            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
#endif
