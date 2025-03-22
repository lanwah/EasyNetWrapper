using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable IDE1006 // 命名样式

namespace EasyNet.Ollama.Models
{
    /// <summary>
    /// https://api-docs.deepseek.com/zh-cn/api/create-chat-completion
    /// </summary>
    public class ChatData
    {
        /// <summary>
        /// 该对话的唯一标识符。
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 对象的类型, 其值为 chat.completion。
        /// </summary>
        public string @object { get; set; }
        /// <summary>
        /// 创建聊天完成时的 Unix 时间戳（以秒为单位）。
        /// </summary>
        public long created { get; set; }
        /// <summary>
        /// 生成该 completion 的模型名。
        /// </summary>
        public string model { get; set; }
        /// <summary>
        /// This fingerprint represents the backend configuration that the model runs with.
        /// </summary>
        public string system_fingerprint { get; set; }
        /// <summary>
        /// 模型生成的 completion 的选择列表。
        /// </summary>
        public List<Choice> choices { get; set; }
    }
}
#pragma warning restore IDE1006 // 命名样式