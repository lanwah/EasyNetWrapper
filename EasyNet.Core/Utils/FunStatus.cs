
namespace EasyNet.Core.Utils
{
    /// <summary>
    /// 操作状态
    /// </summary>
    public struct FunStatus
    {
        /// <summary>
        /// 结果，true - 成功；false - 失败
        /// </summary>
        public bool IsSuccess
        {
            get; set;
        }
        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message
        {
            get; set;
        }


        /// <summary>
        /// 成功信息
        /// </summary>
        public static FunStatus Ok
        {
            get
            {
                return new FunStatus()
                {
                    IsSuccess = true,
                };
            }
        }
        /// <summary>
        /// 成功信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static FunStatus Success(string message)
        {
            return new FunStatus()
            {
                IsSuccess = true,
                Message = message
            };
        }
        /// <summary>
        /// 失败信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static FunStatus Failure(string message)
        {
            return new FunStatus()
            {
                IsSuccess = false,
                Message = message
            };
        }

    }
}
