using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#pragma warning disable IDE1006 // 命名样式

namespace EasyNet.Ollama.Models
{
    /// <summary>
    /// 模型生成的 completion
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// 该 completion 在模型生成的 completion 的选择列表中的索引。
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 模型停止生成 token 的原因，可能值[stop, length, content_filter, tool_calls, insufficient_system_resource]   
        /// stop：模型自然停止生成，或遇到 stop 序列中列出的字符串。
        /// length ：输出长度达到了模型上下文长度限制，或达到了 max_tokens 的限制。
        /// content_filter：输出内容因触发过滤策略而被过滤。
        /// insufficient_system_resource：系统推理资源不足，生成被打断。。
        /// </summary>
        public string finish_reason { get; set; }
        /// <summary>
        /// 模型生成的 completion 消息。
        /// </summary>
        public Delta delta { get; set; }
    }
}
#pragma warning restore IDE1006 // 命名样式
