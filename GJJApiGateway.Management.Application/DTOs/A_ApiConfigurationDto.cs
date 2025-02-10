using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_ApiConfigurationDto
    {
        public string? ApiPath { get; set; }
        public string? Body { get; set; }
        public string? Parameter { get; set; }
        public string? ApiType { get; set; }
        public string? ApiSource { get; set; }
        public string? ApiChineseName { get; set; }
        public string? RequestMethod { get; set; }
        public string? AuthMethod { get; set; }
        public string? CallMode { get; set; }
        public string? DataSourceType { get; set; }
        public int Timeout { get; set; }
        public int RequestLimit { get; set; }
        public int RateLimit { get; set; }
        public string? VersionNumber { get; set; }
        public string? Protocol { get; set; }
        public string? Tags { get; set; }
        public string? Description { get; set; }
        public bool IsEnabled { get; set; }
        public string? BusinessIdentifier { get; set; }
    }
}
