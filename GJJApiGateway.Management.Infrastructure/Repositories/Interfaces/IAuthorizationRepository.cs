using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<IEnumerable<Authorization>> GetAllAsync();
        Task<Authorization> GetByIdAsync(int id);
        Task AddAsync(Authorization authorization);
        Task UpdateAsync(Authorization authorization);
        Task DeleteAsync(int id);
    }
}
