using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class ApiApplicationMappingRepository: IApiApplicationMappingRepository
    {
        private readonly ManagementDbContext _context;

        public ApiApplicationMappingRepository(ManagementDbContext context)
        {
            _context = context;
        }

        // 插入API授权关系
        public async Task<int> InsertApiApplicationMappingsAsync(IEnumerable<ApiApplicationMapping> mappings)
        {
            await _context.ApiApplicationMappings.AddRangeAsync(mappings);
            return await _context.SaveChangesAsync();
        }

        // 删除旧的授权映射
        public async Task<int> DeleteOldMappingsAsync(List<ApiApplicationMapping> mappings)
        {
            _context.ApiApplicationMappings.RemoveRange(mappings);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApiApplicationMapping>> GetExistingMappingsByApplicationIdsAsync(IEnumerable<int> applicationIds)
        {
            return await _context.ApiApplicationMappings
                .Where(m => applicationIds.Contains(m.ApplicationId))
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
