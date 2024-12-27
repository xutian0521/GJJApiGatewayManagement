using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Model.ViewModels
{
    public class AuthorizationViewModel
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public List<string> AllowedEndpoints { get; set; }
    }
}
