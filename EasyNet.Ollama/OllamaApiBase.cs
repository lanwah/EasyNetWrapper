using EasyNet.JsonConvertProvider;
using EasyNet.Ollama.Interface;
using EasyNet.Ollama.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNet.Ollama
{
    /// <summary>
    /// Ollama Api 基类
    /// </summary>
    public abstract partial class OllamaApiBase : IOllamaApiClient, IDisposable
    {
        /// <summary>
        /// 地址，格式：http://host:port
        /// </summary>
        public string Host { get; protected set; }
        /// <summary>
        /// API 密钥
        /// </summary>
        public string ApiKey { get; protected set; }
        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; protected set; }
        /// <summary>
        /// Http 请求客户端
        /// </summary>
        public HttpClient HttpRequestClient { get; protected set; }
        /// <summary>
        /// Http 响应处理器
        /// </summary>
        public IResponseHander ResponseHandler { get; set; }
        /// <summary>
        /// 流数据接收器
        /// </summary>
        public event DataReceivedHandler DataReceived;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseUrl">http://host:port</param>
        /// <param name="apiKey"></param>
        /// <param name="modelName"></param>
        public OllamaApiBase(string baseUrl, string apiKey, string modelName)
        {
            this.ApiKey = apiKey;
            this.Host = baseUrl;
            this.ModelName = modelName;

            this.InitHttpClient();
        }
        /// <summary>
        /// 流数据接收器
        /// </summary>
        /// <param name="args"></param>
        protected void OnDataReceived(ChatReceivedArgs args)
        {
            this.DataReceived?.Invoke(args);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">true - 释放所有资源；false - 仅释放非托管资源</param>
        protected virtual void Dispose(bool disposing)
        {

        }
        /// <summary>
        /// 设置 Http 响应处理器
        /// </summary>
        /// <param name="responseHandler"></param>
        public void SetResponseHandler(IResponseHander responseHandler)
        {
            this.ResponseHandler = responseHandler;
            this.ResponseHandler.DataReceived -= ResponseHandler_DataReceived;
            this.ResponseHandler.DataReceived += ResponseHandler_DataReceived;
        }

        private void ResponseHandler_DataReceived(ChatReceivedArgs args)
        {
            this.OnDataReceived(args);
        }

        /// <summary>
        /// 初始化 HttpClient
        /// </summary>
        protected virtual void InitHttpClient()
        {
            // 设置请求头
            this.HttpRequestClient = new HttpClient();
            this.HttpRequestClient.DefaultRequestHeaders.Clear();
            this.HttpRequestClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
            this.HttpRequestClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<string> SendRequestAsync(string requestData, CancellationToken? cancellationToken = null);

        /// <summary>
        /// 异步版本的 Post 请求
        /// </summary>
        /// <param name="chatApiUrl"></param>
        /// <param name="userInput"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual async Task<string> PostAsync(string chatApiUrl, string userInput, CancellationToken? cancellationToken = null)
        {
            try
            {
                var cancelToken = cancellationToken ?? CancellationToken.None;

                // 获取 StringContent 内容
                var content = this.GetRequestContent(userInput);
                // 发送请求
                var response = await this.HttpRequestClient.PostAsync(chatApiUrl, content, cancelToken).ConfigureAwait(false);
                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 检查响应状态码
                if (response.IsSuccessStatusCode)
                {
                    this.ResponseHandler.SetResponse(response);
                    await this.ResponseHandler.HandleAsync(cancelToken).ConfigureAwait(false);
                    return string.Empty;
                }
                else
                {
                    // 返回错误信息
                    //return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                    return await FailedAsync(response, cancelToken).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                // 捕获并返回异常信息
                return $"HttpRequestException: {ex.Message}";
            }
            catch (Exception ex)
            {
                // 捕获并返回其他异常信息
                return $"Exception: {ex.Message}";
            }
        }
        /// <summary>
        /// 获取请求的 StringContent 内容
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        protected virtual StringContent GetRequestContent(string userInput)
        {
            // 构造请求数据
            var requestBody = this.GetRequestBody(userInput);
            // 序列化请求数据
            var jsonRequest = requestBody.ToJson();
            // 创建请求内容
            return new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        }
        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        public virtual object GetRequestBody(string userInput)
        {
            // https://github.com/deepseek-ai/DeepSeek-R1?tab=readme-ov-file#usage-recommendations
            // 构造请求数据
            var data = new
            {
                model = ModelName,
                messages = new List<dynamic>
                {
                    //new
                    //{
                    //    role="system",
                    //    content="Initiate your response with \"<think>\\n嗯\" at the beginning of every output."
                    //},
                    //new
                    //{
                    //    role="system",
                    //    content="关闭回答前的\"<think>\""
                    //},
                    new
                    {
                        role = "user",
                        content = userInput
                    }

                },
                temperature = 0.6,
                stream = true
            };

            return data;
        }
        /// <summary>
        /// 请求异常处理
        /// </summary>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
#pragma warning disable CRR0038 // CancellationToken parameter is never used.
        protected virtual async Task<string> FailedAsync(HttpResponseMessage response, CancellationToken? cancellationToken = null)
#pragma warning restore CRR0038 // CancellationToken parameter is never used.
        {
            //var cancelToken = cancellationToken ?? CancellationToken.None;
            var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return $"API请求失败：{response.StatusCode} - {errorContent}";
        }
    }
}
