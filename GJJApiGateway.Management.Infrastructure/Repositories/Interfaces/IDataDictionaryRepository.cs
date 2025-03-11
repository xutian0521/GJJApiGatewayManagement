using GJJApiGateway.Management.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IDataDictionaryRepository
    {
        Task<DataPageResult<SysDataDictionary>> GetPagedParentsAsync(int pageIndex, int pageSize);
        Task<List<SysDataDictionary>> GetChildrenByParentIdsAsync(List<int> parentIds);
        Task<List<SysDataDictionary>> GetAllDataDictionaryAsync();
        Task<List<SysDataDictionary>> GetEnumTypeListAsync();
        Task<SysDataDictionary> GetDataDictionaryByIdAsync(int id);
        Task<int> InsertDataDictionaryAsync(SysDataDictionary dataDictionary);
        Task<int> UpdateDataDictionaryAsync(SysDataDictionary dataDictionary);

        Task<int> DeleteDataDictionaryAsync(SysDataDictionary dataDictionary);
    }
}
