namespace GJJApiGateway.Management.Api.Controllers.Shared.ViewModels
{
    /// <summary>
    /// 公共查询放回实体 
    /// </summary>
    public class v_ApiResult
    {
        /// <summary>
        /// ctor
        /// </summary>
        public v_ApiResult()
        {

        }
        /// <summary>
        /// cotr2
        /// </summary>
        /// <param name="content"></param>
        public v_ApiResult(dynamic content)
        {
            this.Content = content;

            this.Code = 1;
        }
        /// <summary>
        /// ctor3
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public v_ApiResult(int code, string message, dynamic content)
        {
            this.Message = message;
            this.Content = content;
            this.Code = code;
        }
        /// <summary>
        /// ctor4
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public v_ApiResult(int code, string message)
        {
            this.Message = message;
            this.Code = code;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual dynamic Content { get; set; }
    }

    /// <summary>
    /// 用于swagger 显示的自定义实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class s_ApiResult<T>
    {
        public s_ApiResult(int code, string message, T content)
        {
            this.Message = message;
            this.Content = content;
            this.Code = code;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public T Content { get; set; }
    }


}
