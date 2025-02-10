using Microsoft.AspNetCore.Mvc;
using GJJApiGateway.Management.Application.Services;
using GJJApiGateway.Management.Api.ViewModel;
using GJJApiGateway.Management.Api.DTOs;
using AutoMapper;
using GJJApiGateway.Management.Application.Interfaces;

namespace GJJApiGateway.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("UserLogin")]
        public s_ApiResult<LoginResponseVM> UserLogin([FromBody] C_LoginRequestDto request)
        {
            var result = _accountService.UserLogin(request.Username);
            var viewModel = _mapper.Map<LoginResponseVM>(result.Data);
            return new s_ApiResult<LoginResponseVM>(result.Code, result.Message, viewModel);
        }

        [HttpGet("GetUserInfo")]
        public s_ApiResult<UserInfoVM> GetUserInfo(string token)
        {
            var result = _accountService.GetUserInfo(token);
            var viewModel = _mapper.Map<UserInfoVM>(result.Data);
            return new s_ApiResult<UserInfoVM>(result.Code, result.Message, viewModel);
        }
    }


}