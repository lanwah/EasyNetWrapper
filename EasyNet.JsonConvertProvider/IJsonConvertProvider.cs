using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.JsonConvertProvider
{
    /// <summary>
    /// Json 数据转换器
    /// </summary>
    public interface IJsonConvertProvider
    {
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string SerializeObject(object value);
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        T DeserializeObject<T>(string json);
    }
}
