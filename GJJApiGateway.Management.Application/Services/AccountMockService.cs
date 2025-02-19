using System;
using AutoMapper;
using Consul;
using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Common.Constants;
using GJJApiGateway.Management.Common.Utilities;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GJJApiGateway.Management.Application.Services
{
    public class AccountMockService: IAccountService
    {
        private readonly ISysUserInfoRepository _sysUserInfoRepository;
        private readonly IMapper _mapper;
        static IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        public AccountMockService(ISysUserInfoRepository sysUserInfoRepository,
            IMapper mapper)
        {
            _sysUserInfoRepository= sysUserInfoRepository;
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
            var user = await this.GetUserByNameAsync(0, userName);

            if (user == null)
            {
                return ServiceResult<A_LoginResponseDto>.Fail("用户名不存在");
            }

            string _pwd = EncryptionHelper.MD5Encoding(password, user.Salt);
            if (!user.Password.Equals(_pwd))
            {
                return ServiceResult<A_LoginResponseDto>.Fail("密码不正确");
            }
            user.LastLoginTime = DateTime.Now;
            var UserDb= _mapper.Map<SysUserInfo>(user);
            int updateRow = await _sysUserInfoRepository.UpdateAsync(UserDb);
            string userJson = System.Text.Json.JsonSerializer.Serialize(user);

            const int expMin = 60 * 24 * 30;
            //const int expMin = 1;
            var exp = (DateTime.UtcNow.AddMinutes(expMin) - new DateTime(1970, 1, 1)).TotalSeconds;
            var srtJson = JwtHelper.Encrypt(user.Id.ToString(), user.Name, user.RoleId, exp);


            A_LoginResponseDto dto = new A_LoginResponseDto()
            {
                access_token = srtJson,
                expires_in = expMin,
                user_id = user.Id.ToString(),
                user_name = user.Name,
                role_id = user.RoleId,
                real_name = user.RealName,

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

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public async Task<A_SysUserInfoDto> GetUserByNameAsync(int id, string userName)
        {
            var userDb = await _sysUserInfoRepository.GetUserByNameAsync(id, userName);
            var user =_mapper.Map<A_SysUserInfoDto>(userDb);
            return user;

        }
    }
}