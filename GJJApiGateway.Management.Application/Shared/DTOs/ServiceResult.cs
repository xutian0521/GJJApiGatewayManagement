namespace GJJApiGateway.Management.Application.Shared.DTOs
{
    /// <summary>
    /// 业务层通用操作结果 DTO，用于封装操作结果数据，不依赖 v_ApiResult。
    /// </summary>
    public class ServiceResult<T>
    {
        /// <summary>
        /// 代码，1 表示成功，0 表示失败
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息说明
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 创建成功的操作结果
        /// </summary>
        public static ServiceResult<T> Success(T data, string message = "操作成功")
        {
            return new ServiceResult<T> { Code = 1, Message = message, Data = data };
        }

        /// <summary>
        /// 创建失败的操作结果
        /// </summary>
        public static ServiceResult<T> Fail(string message)
        {
            return new ServiceResult<T> { Code = 0, Message = message, Data = default(T) };
        }
    }
}

