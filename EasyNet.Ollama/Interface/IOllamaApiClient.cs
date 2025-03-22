using EasyNet.JsonConvertProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNet.Ollama.Interface
{
    /// <summary>
    /// Ollama API 客户端接口
    /// </summary>
    public interface IOllamaApiClient
    {
        /// <summary>
        /// 地址，格式：http://host:port
        /// </summary>
        string Host { get; }
        /// <summary>
        /// API 密钥
        /// </summary>
        string ApiKey { get; }
        /// <summary>
        /// 模型名称
        /// </summary>
        string ModelName { get; }
        /// <summary>
        /// Http 请求客户端
        /// </summary>
        HttpClient HttpRequestClient { get; }
        /// <summary>
        /// Http 响应处理器
        /// </summary>
        IResponseHander ResponseHandler { get; set; }
        /// <summary>
        /// 数据接收器
        /// </summary>
        event DataReceivedHandler DataReceived;

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> SendRequestAsync(string requestData, CancellationToken? cancellationToken = null);

        //        /// <summary>
        //        /// 生成对话回复
        //        /// </summary>
        //        /// <param name="userInput"></param>
        //        /// <returns></returns>
        //        string Answer(string userInput);

        //#if NET45_OR_GREATER || NET5_0_OR_GREATER
        //        /// <summary>
        //        /// 生成对话回复
        //        /// </summary>
        //        /// <param name="userInput"></param>
        //        /// <returns></returns>
        //        Task<string> AnswerAsync(string userInput);
        //#endif
    }
}
