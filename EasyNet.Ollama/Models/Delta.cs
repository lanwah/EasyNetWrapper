#pragma warning disable IDE1006 // 命名样式
namespace EasyNet.Ollama.Models
{
    /// <summary>
    /// 模型生成的 completion 消息  
    /// </summary>
    public class Delta
    {
        /// <summary>
        /// 生成这条消息的角色，可能值[assistant]
        /// </summary>        
        public string role { get; set; }
        /// <summary>
        /// 该 completion 的内容。
        /// </summary>
        public string content { get; set; }
    }
}
#pragma warning restore IDE1006 // 命名样式