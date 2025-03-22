using System;
using System.Collections.Generic;
using System.Text;

namespace EasyNet.JsonConvertProvider
{
    /// <summary>
    /// Json 转换器扩展类
    /// </summary>
    public static class JsonConvertProviderExts
    {
        private static IJsonConvertProvider Provider
        {
            get; set;
        }

        static JsonConvertProviderExts()
        {
#if NET5_0_OR_GREATER
            Provider = new TextJsonConvertProvider();
#else
            Provider = new NewtonsoftJsonConvertProvider();
#endif
        }

        /// <summary>
        /// 设置 Json 转换器
        /// </summary>
        /// <param name="provider"></param>
        public static void SetProvider(IJsonConvertProvider provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// 将对象转换为 Json 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return Provider.SerializeObject(obj);
        }
        /// <summary>
        /// 将 Json 字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return Provider.DeserializeObject<T>(json);
        }
    }
}
