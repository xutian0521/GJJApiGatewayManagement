using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_ApiAuthorizationCheckRequestDto
    {
        public string? JwtToken { get; set; }
        public string? ApiPath { get; set; }
    }
}
