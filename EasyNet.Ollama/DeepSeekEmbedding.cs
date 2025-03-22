using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNet.Ollama
{
    /// <summary>
    /// DeepSeek API 用于生成嵌入向量
    /// </summary>
    public class DeepSeekEmbedding : DeepSeek
    {
        /// <summary>
        /// 初始化 DeepSeek API
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="apiKey"></param>
        /// <param name="modelName"></param>
        public DeepSeekEmbedding(string baseUrl, string apiKey, string modelName) : base(baseUrl, apiKey, modelName)
        {
        }

        /// <summary>
        /// 获取用于文本补全的地址
        /// </summary>
        /// <returns></returns>
        protected override string GetApiUrl()
        {
            return $"{this.Host}/v1/embeddings";
        }
    }
}