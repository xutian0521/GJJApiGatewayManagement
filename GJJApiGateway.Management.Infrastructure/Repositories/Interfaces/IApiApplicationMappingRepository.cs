using GJJApiGateway.Management.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IApiApplicationMappingRepository
    {
        Task<int> InsertApiApplicationMappingsAsync(IEnumerable<ApiApplicationMapping> mappings);
        Task<int> DeleteOldMappingsAsync(List<ApiApplicationMapping> mappings);
        Task<IEnumerable<ApiApplicationMapping>> GetExistingMappingsByApplicationIdsAsync(IEnumerable<int> applicationIds);
    }
}
