﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_ApiAuthorizationCheckResultDto
    {
        public bool IsAuthorized { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }

    }
}
