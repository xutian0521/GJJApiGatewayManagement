using GJJApiGateway.Management.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;

namespace GJJApiGateway.Management.Infrastructure.Repositories.Interfaces
{
    public interface IDataDictionaryRepository
    {
        Task<DataPageResult<SysDataDictionaryDto>> GetPagedDataDictionariesAsync(int pageIndex, int pageSize);
        Task<List<SysDataDictionaryDto>> GetDataDictionaryTreeAsync(int rootPId);
        Task<List<SysDataDictionary>> GetEnumTypeListAsync();
        Task<SysDataDictionaryDto> GetDataDictionaryByIdAsync(int id);
        Task<int> InsertDataDictionaryAsync(SysDataDictionary dataDictionary);
        Task<int> UpdateDataDictionaryAsync(SysDataDictionary dataDictionary);

        Task<int> DeleteDataDictionaryAsync(SysDataDictionary dataDictionary);
    }
}
