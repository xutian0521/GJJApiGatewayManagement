using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GJJApiGateway.Management.Infrastructure.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    public class DataDictionaryRepository : IDataDictionaryRepository
    {
        private readonly ManagementDbContext _context;

        public DataDictionaryRepository(ManagementDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 分页查询父级数据字典（父节点：PId == null 或 PId == 0）
        /// </summary>
        public async Task<DataPageResult<SysDataDictionary>> GetPagedParentsAsync(int pageIndex, int pageSize)
        {
            // 定义父节点的查询条件
            var query = _context.SysDataDictionarys
                .Where(d => d.PId == null || d.PId == 0);

            // 计算总记录数
            int totalCount = await query.CountAsync();

            // 分页查询父节点（可按 SortId 和 DataKey 进行排序）
            var parentRecords = await query
                .OrderBy(d => d.SortId)
                .ThenBy(d => d.DataKey)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new DataPageResult<SysDataDictionary>
            {
                List = parentRecords,
                Total = totalCount
            };
        }
        public async Task<List<SysDataDictionary>> GetChildrenByParentIdsAsync(List<int> parentIds)
        {
            if (parentIds == null || !parentIds.Any())
            {
                return new List<SysDataDictionary>();
            }

            return await _context.SysDataDictionarys
                .Where(d => parentIds.Contains(d.PId))
                .OrderBy(d => d.SortId)
                .ThenBy(d => d.DataKey)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<List<SysDataDictionary>> GetAllDataDictionaryAsync()
        {
            return await _context.SysDataDictionarys
                .AsNoTracking()
                .ToListAsync();
        }
        
        /// <summary>
        /// 获取枚举字典类型列表（`PId = 0`）
        /// </summary>
        public async Task<List<SysDataDictionary>> GetEnumTypeListAsync()
        {
            var list = await _context.SysDataDictionarys
                .Where(d => d.PId == 0) // 只获取根级字典项
                .OrderBy(d => d.SortId)
                .AsNoTracking()
                .ToListAsync();

            return list;
        }
        /// <summary>
        /// 根据 ID 获取枚举字典信息，并获取其父级名称（如果有）
        /// </summary>
        public async Task<SysDataDictionary> GetDataDictionaryByIdAsync(int id)
        {
            var dictionary = await _context.SysDataDictionarys
                .Where(d => d.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return dictionary;
        }
        
        
        /// <summary>
        /// 插入新数据字典
        /// </summary>
        public async Task<int> InsertDataDictionaryAsync(SysDataDictionary dataDictionary)
        {
            _context.SysDataDictionarys.Add(dataDictionary);
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 更新数据字典
        /// </summary>
        public async Task<int> UpdateDataDictionaryAsync(SysDataDictionary dataDictionary)
        {
            _context.SysDataDictionarys.Attach(dataDictionary);
            _context.Entry(dataDictionary).State = EntityState.Modified; // 标记为已修改

            return await _context.SaveChangesAsync();
        }
        
        
        /// <summary>
        /// 删除数据字典
        /// </summary>
        public async Task<int> DeleteDataDictionaryAsync(SysDataDictionary dataDictionary)
        {
            _context.SysDataDictionarys.Remove(dataDictionary);
            return await _context.SaveChangesAsync();
        }
    }
}
