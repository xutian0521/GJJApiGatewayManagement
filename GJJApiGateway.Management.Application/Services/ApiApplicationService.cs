using AutoMapper;
using GJJApiGateway.Management.Application.DTOs;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Common.Utilities;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Application.Services
{
    public class ApiApplicationService : IApiApplicationService
    {
        private readonly IApiApplicationRepository _repository;
        private readonly IMapper _mapper;

        public ApiApplicationService(IApiApplicationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // 获取API应用程序分页列表
        public async Task<ServiceResult<PageResult<A_ApiApplicationDto>>> GetApiApplicationsAsync(string? applicationName, string? description, int page, int limit)
        {
            try
            {
                // 获取总记录数
                int total = await _repository.GetApiApplicationsCountAsync(applicationName, description);

                // 获取分页数据
                var applications = await _repository.GetApiApplicationsListAsync(applicationName, description, page, limit);

                // 映射为业务层 DTO
                var applicationDtos = _mapper.Map<List<A_ApiApplicationDto>>(applications);

                var pageResult = new PageResult<A_ApiApplicationDto>
                {
                    Items = applicationDtos,
                    Total = total
                };

                return ServiceResult<PageResult<A_ApiApplicationDto>>.Success(pageResult, "查询成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<PageResult<A_ApiApplicationDto>>.Fail($"获取API应用程序列表时发生异常：{ex.Message}");
            }
        }


        public async Task<ServiceResult<A_ApiApplicationDto>> GetAppByIdAsync(int appId)
        {
            var result = await _repository.GetAppByIdAsync(appId);
            if (result == null)
            {
                return ServiceResult<A_ApiApplicationDto>.Fail("Application not found.");
            }
            var dto = _mapper.Map<A_ApiApplicationDto>(result);
            return ServiceResult<A_ApiApplicationDto>.Success(dto, "查询成功");
        }

        // 创建新的API应用程序并返回新增记录的ID
        public async Task<ServiceResult<int>> CreateApiApplicationAsync(A_ApiApplicationDto applicationDto)
        {
            try
            {
                // 将DTO转换为数据库实体对象
                var application = _mapper.Map<ApiApplication>(applicationDto);

                // 设置创建时间、最后修改时间以及初始化TokenVersion
                application.CreateTime = DateTime.Now;
                application.LastModifiedTime = DateTime.Now;
                application.TokenVersion = 1;

                // 生成JWT Token
                var exp = (DateTime.UtcNow.AddDays(36500) - new DateTime(1970, 1, 1)).TotalSeconds;
                var jwtToken = JwtHelper.EncryptApi(application.Id, application.ApplicationName, "", "36500", "", application.AuthMethod, exp, application.TokenVersion);

                // 将生成的JWT Token赋值到实体中
                application.JwtToken = jwtToken;

                // 调用数据层进行创建操作
                var createdAppId = await _repository.CreateApiApplicationAsync(application);

                return ServiceResult<int>.Success(createdAppId, "创建成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Fail($"创建API应用程序时发生异常：{ex.Message}");
            }
        }

        public async Task<ServiceResult<string>> DeleteApiApplicationAsync(int appId)
        {
            var result = await _repository.DeleteApiApplicationAsync(appId);
            return result ? ServiceResult<string>.Success("删除成功") : ServiceResult<string>.Fail("删除失败");
        }

        // 更新API应用程序
        // 更新API应用程序
        public async Task<ServiceResult<string>> UpdateApiApplicationAsync(int appId, A_ApiApplicationDto applicationDto)
        {
            try
            {
                // 将DTO转换为实体对象
                var applicationEntity = _mapper.Map<ApiApplication>(applicationDto);

                // 调用数据层更新
                var result = await _repository.UpdateApiApplicationAsync(appId, applicationEntity);

                if (result)
                {
                    return ServiceResult<string>.Success("更新成功");
                }
                else
                {
                    return ServiceResult<string>.Fail("更新失败，找不到该应用程序");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail($"更新API应用程序时发生异常：{ex.Message}");
            }
        }

        public async Task<ServiceResult<string>> GetAuthorizedTokenAsync(int applicationId)
        {
            var token = await _repository.GetAuthorizedTokenAsync(applicationId);
            return ServiceResult<string>.Success(token, "查询成功");
        }

        public async Task<ServiceResult<IEnumerable<A_ApiInfoDto>>> GetAuthorizedApiListAsync(int applicationId)
        {
            try
            {
                // 从数据层获取授权API列表，经过映射后的API列表
                var apiList = await _repository.GetAuthorizedApiListAsync(applicationId);

                //if (apiList == null || !apiList.Any())
                //{
                //    return ServiceResult<IEnumerable<A_ApiInfoDto>>.Fail("未找到指定应用程序的授权API列表。");
                //}

                // 将获取到的实体对象映射为DTO
                var apiInfoDtos = _mapper.Map<IEnumerable<A_ApiInfoDto>>(apiList);

                return ServiceResult<IEnumerable<A_ApiInfoDto>>.Success(apiInfoDtos, "查询成功");
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<A_ApiInfoDto>>.Fail($"获取授权API列表时发生异常: {ex.Message}");
            }
        }
    }

}
