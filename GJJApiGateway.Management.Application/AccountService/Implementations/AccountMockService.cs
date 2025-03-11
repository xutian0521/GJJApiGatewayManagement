using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.Commands;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.AccountService.Interfaces;
using GJJApiGateway.Management.Application.AccountService.Module.Validation;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Application.Shared.Queries;
using GJJApiGateway.Management.Common.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace GJJApiGateway.Management.Application.AccountService.Implementations
{
    public class AccountMockService: IAccountService
    {
        private readonly IUserInfoCommand _userInfoCommand;
        private readonly IUserInfoQuery _userInfoQuery;
        private readonly IUserNameCheckModule _userNameCheckModule;
        private readonly IMapper _mapper;
        static IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        public AccountMockService(
            IUserInfoCommand userInfoCommand, 
            IUserInfoQuery userInfoQuery,
            IUserNameCheckModule userNameCheckModule,
            IMapper mapper)
        {
            _userInfoCommand= userInfoCommand;
            _userInfoQuery = userInfoQuery;
            _userNameCheckModule = userNameCheckModule;
            _mapper = mapper;
        }
        public async Task<ServiceResult<A_LoginResponseDto>> UserLoginAsync(string userName,
                string password, string code, string codeKey)
        {

            //string _code = _memoryCache.Get<string>("ValidateCode_" + codeKey);

            //if (!_code.Equals(code))
            //{
            //    return ServiceResult<A_LoginResponseDto>.Fail("验证码不正确");
            //}
            
            // 1. 调用用户名校验模块进行校验
            UserName_ValidationResult validation = await _userNameCheckModule.ValidateUserAsync(userName);
            if (!validation.IsValid)
            {
                // 若校验失败，返回包含错误信息的结果，不继续后续流程
                return ServiceResult<A_LoginResponseDto>.Fail(validation.ErrorMessage);
            }
            var userInfoDto =  await _userInfoQuery.GetUserByNameAsync(0, userName);
            
            if (userInfoDto == null)
            {
                return ServiceResult<A_LoginResponseDto>.Fail("用户名不存在");
            }

            string _pwd = EncryptionHelper.MD5Encoding(password, userInfoDto.Salt);
            if (!userInfoDto.Password.Equals(_pwd))
            {
                return ServiceResult<A_LoginResponseDto>.Fail("密码不正确");
            }
            userInfoDto.LastLoginTime = DateTime.Now;
            int updateRow = await _userInfoCommand.UpdateUserInfoAsync(userInfoDto);
            string userJson = System.Text.Json.JsonSerializer.Serialize(userInfoDto);

            const int expMin = 60 * 24 * 30;
            //const int expMin = 1;
            var exp = (DateTime.UtcNow.AddMinutes(expMin) - new DateTime(1970, 1, 1)).TotalSeconds;
            var srtJson = JwtHelper.Encrypt(userInfoDto.Id.ToString(), userInfoDto.Name, userInfoDto.RoleId, exp);


            A_LoginResponseDto dto = new A_LoginResponseDto()
            {
                access_token = srtJson,
                expires_in = expMin,
                user_id = userInfoDto.Id.ToString(),
                user_name = userInfoDto.Name,
                role_id = userInfoDto.RoleId,
                real_name = userInfoDto.RealName,

            };
            return ServiceResult<A_LoginResponseDto>.Success(dto);



        }

        public ServiceResult<A_SysUserInfoDto> GetUserInfo(string token)
        {
            // 临时假数据
            return token switch
            {
                "admin-token" => new ServiceResult<A_SysUserInfoDto>
                {
                    Code = 1,
                    Data = new A_SysUserInfoDto
                    {
                        //Roles = new[] { "admin" },
                        //Introduction = "I am a super administrator",
                        //Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                        //Name = "Super Admin"
                    }
                },
                "editor-token" => new ServiceResult<A_SysUserInfoDto>
                {
                    Code = 1,
                    Data = new A_SysUserInfoDto
                    {
                        //Roles = new[] { "editor" },
                        //Introduction = "I am an editor",
                        //Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                        //Name = "Normal Editor"
                    }
                },
                _ => new ServiceResult<A_SysUserInfoDto>
                {
                    Code = 50008,
                    Message = "Login failed, unable to get user details."
                }
            };
        }

        /// <summary>
        /// 获取图像验证码
        /// </summary>
        /// <returns></returns>
        public dynamic GetValidateCode()
        {
            var num = RandomHelper.CreateRandomNumber();

            string codeKey = Guid.NewGuid().ToString();
            _memoryCache.Set("ValidateCode_" + codeKey, num, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30)));

            byte[] bytes = CaptchaHelper.CreateValidateGraphic(num);
            var base64 = Convert.ToBase64String(bytes);
            Console.WriteLine("成功！");
            return new { codeKey = codeKey, imageData = "data:image/jpeg;base64," + base64 };
        }
    }
}