﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.DTOs
{
    public class A_UserInfoDto
    {
        public string[]? Roles { get; set; }
        public string? Introduction { get; set; }
        public string? Avatar { get; set; }
        public string? Name { get; set; }
    }
}
