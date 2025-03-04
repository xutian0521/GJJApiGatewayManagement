using GJJApiGateway.Management.Api.Controllers.APIAuth.DTOs;
using GJJApiGateway.Management.Application.APIAuthService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.APIAuth
{
    /// <summary>
    /// 授权控制器，负责处理授权规则的 CRUD 操作。
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        /// <summary>
        /// 授权服务接口，用于处理授权规则的业务逻辑。
        /// </summary>
        private readonly IAuthorizationService _apiAuthorizationService;

        /// <summary>
        /// 构造函数，注入授权服务。
        /// </summary>
        /// <param name="authorizationService">授权服务接口实例。</param>
        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _apiAuthorizationService = authorizationService;
        }


        [HttpPost("validate")]
        public async Task<IActionResult> ValidateApiAuthorization([FromBody] C_ApiAuthorizationCheckRequestDto request)
        {
            // 校验API是否注册
            var r = await _apiAuthorizationService.ValidateApiAuthorizationAsync(request.JwtToken, request.ApiPath);
            return r == null ? NotFound() : Ok(r);
        }
    }

    public class ApiAuthorizationRequest
    {
        public string JwtToken { get; set; }
        public string ApiPath { get; set; }
    }

    public class ApiAuthorizationResult
    {

    }
}

