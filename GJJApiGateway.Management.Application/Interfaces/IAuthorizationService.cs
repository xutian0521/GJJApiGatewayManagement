using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.Interfaces
{
    public interface IAuthorizationService
    {
        Task<IEnumerable<AuthorizationViewModel>> GetAllAsync();
        Task<AuthorizationViewModel> GetByIdAsync(int id);
        Task<AuthorizationViewModel> CreateAsync(CreateAuthorizationDto dto);
        Task<AuthorizationViewModel> UpdateAsync(UpdateAuthorizationDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
