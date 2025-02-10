using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_ApiApplicationDto
    {
        public int Id { get; set; }
        public string? ApplicationName { get; set; }
        public int TokenVersion { get; set; }
        public string? JwtToken { get; set; }
    }
}
