using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.Entities
{
    public class Authorization
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string AllowedEndpoints { get; set; } // JSON 或其他格式
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
