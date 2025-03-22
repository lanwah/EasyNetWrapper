using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNet.Ollama
{
    /// <summary>
    /// DeepSeek API 生成对话回复
    /// </summary>
    public class DeepSeek : OllamaApiBase
    {
        /// <summary>
        /// 初始化 DeepSeek API
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="apiKey"></param>
        /// <param name="modelName"></param>
        public DeepSeek(string baseUrl, string apiKey, string modelName) : base(baseUrl, apiKey, modelName)
        {
        }

        /// <summary>
        /// 发送对话生成请求
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<string> SendRequestAsync(string requestData, CancellationToken? cancellationToken = null)
        {
            return AnswerAsync(requestData, cancellationToken);
        }
        /// <summary>
        /// 获取对话生成请求的地址
        /// </summary>
        /// <returns></returns>
        protected virtual string GetApiUrl()
        {
            // DeepSeek 用于对话生成的API 地址
            var chatApiUrl = $"{this.Host}/v1/chat/completions";
            return chatApiUrl;
        }
        /// <summary>
        /// 发送对话生成请求
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<string> AnswerAsync(string input, CancellationToken? cancellationToken = null)
        {
            return PostAsync(this.GetApiUrl(), input, cancellationToken);
        }
    }
}
