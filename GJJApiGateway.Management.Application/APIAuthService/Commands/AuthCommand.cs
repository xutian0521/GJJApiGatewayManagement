using AutoMapper;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Application.APIAuthService.Commands;

public class AuthCommand: IAuthCommand
{
    private readonly IApiInfoRepository _apiInfoRepository;
    private readonly IApiApplicationRepository _apiApplicationRepository;
    private readonly IApiApplicationMappingRepository  _apiApplicationMappingRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数，注入仓储接口和 AutoMapper 映射器
    /// </summary>
    public AuthCommand(
        IApiInfoRepository apiInfoRepository,
        IApiApplicationRepository apiApplicationRepository,
        IApiApplicationMappingRepository apiApplicationMappingRepository,
        IMapper mapper)
    {
        _apiInfoRepository = apiInfoRepository;
        _apiApplicationRepository = apiApplicationRepository;
        _apiApplicationMappingRepository = apiApplicationMappingRepository;
        _mapper = mapper;
    }
    public async Task<int> UpdateApiInfoAsync(A_ApiInfoDto apiInfoDto)
    {
        var apiInfo= _mapper.Map<ApiInfo>(apiInfoDto);
        int row = await _apiInfoRepository.UpdateApiInfoAsync(apiInfo);
        return row;
    }
    
    public async Task<int> DeleteOldMappingsAsync(List<A_ApiApplicationMappingDto> mappingDtos)
    {
        var mappings= _mapper.Map<List<ApiApplicationMapping>>(mappingDtos);
        int row = await _apiApplicationMappingRepository.DeleteOldMappingsAsync(mappings);
        return row;
    }
    
    public async Task<int> InsertApiApplicationMappingsAsync(List<A_ApiApplicationMappingDto> mappingDtos)
    {
        var mappings= _mapper.Map<List<ApiApplicationMapping>>(mappingDtos);
        int row = await _apiApplicationMappingRepository.DeleteOldMappingsAsync(mappings);
        return row;
    }
    
    public async Task<int> UpdateApplicationsAsync(List<A_ApiApplicationDto> applicationDtos)
    {
        var apps= _mapper.Map<List<ApiApplication>>(applicationDtos);
        int row = await _apiApplicationRepository.UpdateApplicationsAsync(apps);
        return row;
    }
    
    public async Task<int> UpdateApiStatusAsync(int apiId, string newStatus)
    {
        int row = await _apiInfoRepository.UpdateApiStatusAsync(apiId, newStatus);
        return row;
    }
    
    public async Task<int> DeleteApiInfoAsync(int apiId)
    {
        int row = await _apiInfoRepository.DeleteApiInfoAsync(apiId);
        return row;
    }
    
    public async Task<int> BulkInsertApiInfosAsync(List<A_ApiInfoDto> apiInfoDtos)
    {
        var apiInfos= _mapper.Map<List<ApiInfo>>(apiInfoDtos);
        int row = await _apiInfoRepository.BulkInsertApiInfosAsync(apiInfos);
        return row;
    }
    
    public async Task<int> CreateApiInfoAsync(A_ApiInfoDto apiInfoDto)
    {
        var apiInfo= _mapper.Map<ApiInfo>(apiInfoDto);
        int row = await _apiInfoRepository.CreateApiInfoAsync(apiInfo);
        return row;
    }
}