namespace GJJApiGateway.Management.Api.DTOs
{
    /// <summary>
    /// API信息传输对象，用于API管理的数据传输。
    /// </summary>
    public class C_ApiInfoDto
    {
        /// <summary>
        /// API的唯一标识符。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// API的中文名称。
        /// </summary>
        public string? ApiChineseName { get; set; }

        /// <summary>
        /// API的描述信息。
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 调用API的渠道。
        /// </summary>
        public string? AuthorizedApplications { get; set; }

        /// <summary>
        /// API的访问路径。
        /// </summary>
        public string? ApiPath { get; set; }

        /// <summary>
        /// API的类型。
        /// </summary>
        public string? ApiType { get; set; }

        /// <summary>
        /// API的GET请求参数。
        /// </summary>
        public string? Parameter { get; set; }

        /// <summary>
        /// API的POST请求体。
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// API返回结果的结构描述。
        /// </summary>
        public string? ResultStruct { get; set; }

        /// <summary>
        /// API的服务状态（例如：1启动，0不启用）。
        /// </summary>
        public int ServiceStatus { get; set; }

        /// <summary>
        /// API的创建时间。
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// API的业务标识符。
        /// </summary>
        public string? BusinessIdentifier { get; set; }

        /// <summary>
        /// API的来源信息。
        /// </summary>
        public string? ApiSource { get; set; }

        /// <summary>
        /// API的当前状态（例如：待上线，已上线等）。
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// 标识API是否启用。
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// API支持的请求方法（例如：GET, POST）。
        /// </summary>
        public string? RequestMethod { get; set; }

        /// <summary>
        /// API的认证方式（例如：免认证，应用程序认证）。
        /// </summary>
        public string? AuthMethod { get; set; }

        /// <summary>
        /// API的调用模式（例如：同步调用，异步调用）。
        /// </summary>
        public string? CallMode { get; set; }

        /// <summary>
        /// API的数据源类型（例如：关系数据库，API，socket）。
        /// </summary>
        public string? DataSourceType { get; set; }

        /// <summary>
        /// API调用的超时时间，单位为毫秒。
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// API单次请求的数据条数上限。
        /// </summary>
        public int RequestLimit { get; set; }

        /// <summary>
        /// API的访问频率限制（次/秒）。
        /// </summary>
        public decimal RateLimit { get; set; }

        /// <summary>
        /// 版本号，记录API的版本信息，便于管理和维护。
        /// </summary>
        public string? VersionNumber { get; set; }
        /// <summary>
        /// 参数协议：1. http  2. https
        /// </summary>
        public string? Protocol { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string? Tags { get; set; }
    }
}
