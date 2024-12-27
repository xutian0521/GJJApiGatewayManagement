using AutoMapper;
using GJJApiGateway.Management.Api.DTOs;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using GJJApiGateway.Management.Model.ViewModels;

namespace GJJApiGateway.Management.Application.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;

        public AuthorizationService(IAuthorizationRepository authorizationRepository, IMapper mapper)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorizationViewModel>> GetAllAsync()
        {
            var authorizations = await _authorizationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorizationViewModel>>(authorizations);
        }

        public async Task<AuthorizationViewModel> GetByIdAsync(int id)
        {
            var authorization = await _authorizationRepository.GetByIdAsync(id);
            return _mapper.Map<AuthorizationViewModel>(authorization);
        }

        public async Task<AuthorizationViewModel> CreateAsync(CreateAuthorizationDto dto)
        {
            var authorization = _mapper.Map<Authorization>(dto);
            authorization.CreatedAt = DateTime.UtcNow;
            authorization.UpdatedAt = DateTime.UtcNow;

            await _authorizationRepository.AddAsync(authorization);
            return _mapper.Map<AuthorizationViewModel>(authorization);
        }

        public async Task<AuthorizationViewModel> UpdateAsync(UpdateAuthorizationDto dto)
        {
            var authorization = await _authorizationRepository.GetByIdAsync(dto.Id);
            if (authorization == null)
            {
                return null;
            }

            _mapper.Map(dto, authorization);
            authorization.UpdatedAt = DateTime.UtcNow;

            await _authorizationRepository.UpdateAsync(authorization);
            return _mapper.Map<AuthorizationViewModel>(authorization);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var authorization = await _authorizationRepository.GetByIdAsync(id);
            if (authorization == null)
            {
                return false;
            }

            await _authorizationRepository.DeleteAsync(id);
            return true;
        }
    }
}
