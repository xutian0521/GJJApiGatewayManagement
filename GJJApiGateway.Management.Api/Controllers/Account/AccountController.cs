using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Account.DTOs;
using GJJApiGateway.Management.Api.Controllers.Account.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.AccountService.Interfaces;
using GJJApiGateway.Management.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.Account
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
            _logger.LogInformation($"用户开始登录【1】：用户名 {request.userName} started.");
            
            var result = await _accountService.UserLoginAsync(request.userName,request.password,request.code, request.codeKey);
            var viewModel = _mapper.Map<LoginResponseVM>(result.Data);
            _logger.LogInformation($"用户开始登录-结束【2】：REAL_NAME {viewModel.real_name} end.");
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
        /// 获取验证码
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