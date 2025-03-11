using AutoMapper;
using GJJApiGateway.Management.Application.APIAuthService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.APIAuthService.Queries;

public class AuthQuery: IAuthQuery
{
    private readonly IApiInfoRepository _apiInfoRepository;
    private readonly IApiApplicationRepository _apiApplicationRepository;
    private readonly IApiApplicationMappingRepository  _apiApplicationMappingRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数，注入仓储接口和 AutoMapper 映射器
    /// </summary>
    public AuthQuery(
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
    public async Task<List<A_ApiInfoDto>> GetApiInfoListAsync(string apiChineseName,
        string description, string businessIdentifier, string apiSource, string apiPath, int pageIndex, int pageSize)
    {
        var apiInfos = await _apiInfoRepository.GetApiInfoListAsync(apiChineseName,
            description, businessIdentifier, apiSource, apiPath, pageIndex, pageSize);
        var apiInfoDtos = _mapper.Map<List<A_ApiInfoDto>>(apiInfos);
        return apiInfoDtos;
    }
    
    public async Task<A_ApiInfoDto> GetApiInfoByIdAsync(int apiId)
    {
        var apiInfo = await _apiInfoRepository.GetApiInfoByIdAsync(apiId);
        var apiInfoDto = _mapper.Map<A_ApiInfoDto>(apiInfo);
        return apiInfoDto;
    }
    
    public async Task<List<A_ApiInfoDto>> GetApiInfoListByIdsAsync(IEnumerable<int> apiIds)
    {
        var apiInfos = await _apiInfoRepository.GetApiInfoListByIdsAsync(apiIds);
        var apiInfoDtos = _mapper.Map<List<A_ApiInfoDto>>(apiInfos);
        return apiInfoDtos;
    }
    
    public async Task<List<A_ApiApplicationDto>> GetApplicationsByIdsAsync(IEnumerable<int> applicationIds)
    {
        var apps = await _apiApplicationRepository.GetApplicationsByIdsAsync(applicationIds);
        var appDtos = _mapper.Map<List<A_ApiApplicationDto>>(apps);
        return appDtos;
    }
    
    public async Task<IEnumerable<A_ApiApplicationMappingDto>>
        GetExistingMappingsByApplicationIdsAsync(IEnumerable<int> applicationIds)
    {
        var appMappings = 
            await _apiApplicationMappingRepository.GetExistingMappingsByApplicationIdsAsync(applicationIds);
        var appMappingDtos = _mapper.Map<List<A_ApiApplicationMappingDto>>(appMappings);
        return appMappingDtos;
    }
    
    public async Task<List<A_ApiApplicationDto>> GetApplicationsByApiIdAsync(int apiId)
    {
        var apis = await _apiInfoRepository.GetApplicationsByApiIdAsync(apiId);
        var apiDtos = _mapper.Map<List<A_ApiApplicationDto>>(apis);
        return apiDtos;
    }
    
    public async Task<List<A_ApiInfoDto>> GetApisByApplicationIdAsync(int applicationId)
    {
        var apiInfos = await _apiInfoRepository.GetApisByApplicationIdAsync(applicationId);
        var apiInfoDtos = _mapper.Map<List<A_ApiInfoDto>>(apiInfos);
        return apiInfoDtos;
    }
    
    public async Task<List<A_ApiInfoDto>> GetAllApiInfosAsync()
    {
        var apiInfos = await _apiInfoRepository.GetAllApiInfosAsync();
        var apiInfoDtos = _mapper.Map<List<A_ApiInfoDto>>(apiInfos);
        return apiInfoDtos;
    }
    
    public async Task<int> GetApiInfoCountAsync(string apiChineseName,
        string description, string businessIdentifier, string apiSource, string apiPath)
    {
        var row = await _apiInfoRepository.GetApiInfoCountAsync(apiChineseName,
            description, businessIdentifier, apiSource, apiPath);
        return row;
    }
    
}