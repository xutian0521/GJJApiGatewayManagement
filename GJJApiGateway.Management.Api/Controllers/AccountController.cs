using Microsoft.AspNetCore.Mvc;
using GJJApiGateway.Management.Application.Services;
using GJJApiGateway.Management.Api.ViewModel;
using GJJApiGateway.Management.Api.DTOs;
using AutoMapper;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Common.Constants;
using Microsoft.AspNetCore.Authorization;

namespace GJJApiGateway.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService accountService,
            IMapper mapper, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<s_ApiResult<LoginResponseVM>> UserLogin([FromBody] C_LoginRequestDto request)
        {
            _logger.LogInformation($"用户开始登录【1】：用户名 {request.Username} started.");

            var result = await _accountService.UserLoginAsync(request.Username,request.Password,request.code, request.codeKey);
            var viewModel = _mapper.Map<LoginResponseVM>(result.Data);
            _logger.LogInformation($"用户开始登录-结束【2】：REAL_NAME {viewModel.REAL_NAME} end.");
            return new s_ApiResult<LoginResponseVM>(result.Code, result.Message, viewModel);
        }

        [HttpGet("GetUserInfo")]
        public s_ApiResult<SysUserInfoVM> GetUserInfo(string token)
        {
            var result = _accountService.GetUserInfo(token);
            var viewModel = _mapper.Map<SysUserInfoVM>(result.Data);
            return new s_ApiResult<SysUserInfoVM>(result.Code, result.Message, viewModel);
        }

        /// <summary>
        /// 获取图形验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetValidateCode")]
        public v_ApiResult GetValidateCode()
        {
            var result = _accountService.GetValidateCode();
            return new v_ApiResult(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }
    }


}