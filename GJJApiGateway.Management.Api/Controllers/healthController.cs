using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class healthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
