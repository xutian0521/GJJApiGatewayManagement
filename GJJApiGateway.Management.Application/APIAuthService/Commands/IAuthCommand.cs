using GJJApiGateway.Management.Application.APIAuthService.DTOs;

namespace GJJApiGateway.Management.Application.APIAuthService.Commands;

public interface IAuthCommand
{
    Task<int> UpdateApiInfoAsync(A_ApiInfoDto apiInfoDto);
    Task<int> DeleteOldMappingsAsync(List<A_ApiApplicationMappingDto> mappingDtos);
    Task<int> InsertApiApplicationMappingsAsync(List<A_ApiApplicationMappingDto> mappingDtos);
    Task<int> UpdateApplicationsAsync(List<A_ApiApplicationDto> applicationDtos);
    Task<int> UpdateApiStatusAsync(int apiId, string newStatus);
    Task<int> DeleteApiInfoAsync(int apiId);
    Task<int> BulkInsertApiInfosAsync(List<A_ApiInfoDto> apiInfoDtos);
    Task<int> CreateApiInfoAsync(A_ApiInfoDto apiInfoDto);
}