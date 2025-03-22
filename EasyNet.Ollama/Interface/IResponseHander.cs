using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EasyNet.Ollama.Interface
{
    /// <summary>
    /// Http 响应处理器
    /// </summary>
    public interface IResponseHander
    {
        /// <summary>
        /// 数据接收器
        /// </summary>
        event DataReceivedHandler DataReceived;
        /// <summary>
        /// 设置响应
        /// </summary>
        /// <param name="response"></param>
        void SetResponse(HttpResponseMessage response);
        /// <summary>
        /// 处理答案
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task HandleAsync(CancellationToken? cancellationToken = null);
    }
}