using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyNet.Ollama.Models
{
    /// <summary>
    /// 数据接收事件参数
    /// </summary>
    public class ChatReceivedArgs
    {
        /// <summary>
        /// 是否成功，true - 成功；false - 失败（异常）
        /// </summary>
        public bool IsSuccess { get; private set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        public string RawData { get; set; }
        /// <summary>
        /// 解析后的数据
        /// </summary>
        public ChatData ChatData { get; set; }
        /// <summary>
        /// 回答的文本信息
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 错误或异常信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否正在思考中
        /// </summary>
        public bool IsThinking { get; set; }

        /// <summary>
        /// 成功的构造函数
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="chatData"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual ChatReceivedArgs Success(string rawData, ChatData chatData, string content)
        {
            IsSuccess = true;
            RawData = rawData;
            ChatData = chatData;
            Content = content;
            UpdateThinking(content ?? "");
            return this;
        }
        /// <summary>
        /// 失败的构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual ChatReceivedArgs Fail(string message)
        {
            IsSuccess = false;
            Message = message;
            return this;
        }
        /// <summary>
        /// 更新是否正在思考中
        /// </summary>
        /// <param name="answer"></param>
        protected virtual void UpdateThinking(string answer)
        {
            if (answer.Contains("<think>"))
            {
                IsThinking = true;
            }
            else if (answer.Contains("</think>"))
            {
                IsThinking = false;
            }
        }
    }
}
