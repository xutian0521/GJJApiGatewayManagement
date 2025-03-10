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
        /// 获取字典枚举的分页数据（父子结构）
        /// </summary>
        public async Task<DataPageResult<SysDataDictionaryDto>> GetPagedDataDictionariesAsync(int pageIndex, int pageSize)
        {
            // 修正点1：明确父级过滤条件（假设父级PId为null）
            var parentQuery = _context.SysDataDictionarys
                .Where(d => d.PId == null)
                .GroupJoin(
                    _context.SysDataDictionarys,
                    parent => parent.Id,
                    child => child.PId,
                    (parent, children) => new SysDataDictionaryDto
                    {
                        // 修正点2：完整映射父级字段
                        Id = parent.Id,
                        PId = parent.PId, 
                        DataKey = parent.DataKey,
                        DataKeyAlias = parent.DataKeyAlias,
                        DataValue = parent.DataValue,
                        DataDescription = parent.DataDescription,
                        SortId = parent.SortId,
                        CreateTime = parent.CreateTime,
                        UpdateTime = parent.UpdateTime,
                        // 修正点3：正确映射子级集合
                        Children = children.Select(c => new SysDataDictionaryDto
                        {
                            Id = c.Id,
                            PId = c.PId,
                            DataKey = c.DataKey,
                            DataKeyAlias = c.DataKeyAlias,
                            DataValue = c.DataValue,
                            DataDescription = c.DataDescription,
                            SortId = c.SortId,
                            CreateTime = c.CreateTime,
                            UpdateTime = c.UpdateTime
                        }).ToList()
                    });

            // 修正点4：正确的排序逻辑
            var orderedQuery = parentQuery
                .OrderBy(x => x.SortId)  // 使用DTO的SortId字段排序
                .ThenBy(x => x.DataKey);

            // 计算总数（基于父级数量）
            int totalCount = await orderedQuery.CountAsync();

            // 分页查询
            var pagedList = await orderedQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            // 修正点5：返回类型修正
            return new DataPageResult<SysDataDictionaryDto>
            {
                List = pagedList,
                Total = totalCount
            };
        }
        
        /// <summary>
        /// 使用GroupJoin优化的递归树查询（单次查询+内存构建）
        /// </summary>
        public async Task<List<SysDataDictionaryDto>> GetDataDictionaryTreeAsync(int rootPId)
        {
            // 一次性加载所有相关节点（根据业务需求调整筛选条件）
            var allNodes = await _context.SysDataDictionarys

                .ToListAsync();

            // 构建父级到子级的映射字典
            var childrenLookup = allNodes
                .GroupBy(d => d.PId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 递归构建树形结构
            return BuildTreeNodes(rootPId, childrenLookup);
        }

        /// <summary>
        /// 递归构建树节点（内存操作）
        /// </summary>
        private List<SysDataDictionaryDto> BuildTreeNodes(
            int currentPId,
            IReadOnlyDictionary<int, List<SysDataDictionary>> childrenLookup)
        {
            if (!childrenLookup.TryGetValue(currentPId, out var currentNodes))
            {
                return new List<SysDataDictionaryDto>();
            }

            return currentNodes.Select(node => new SysDataDictionaryDto
            {
                Id = node.Id,
                PId = node.PId,
                DataKey = node.DataKey,
                DataKeyAlias = node.DataKeyAlias,
                DataValue = node.DataValue,
                DataDescription = node.DataDescription,
                SortId = node.SortId,
                CreateTime = node.CreateTime,
                UpdateTime = node.UpdateTime,
                Children = BuildTreeNodes(node.Id, childrenLookup) // 内存递归
            }).ToList();
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
        public async Task<SysDataDictionaryDto> GetDataDictionaryByIdAsync(int id)
        {
            var dictionary = await _context.SysDataDictionarys
                .Where(d => d.Id == id)
                .Select(d => new SysDataDictionaryDto
                {
                    Id = d.Id,
                    PId = d.PId,
                    DataKey = d.DataKey,
                    DataKeyAlias = d.DataKeyAlias,
                    DataValue = d.DataValue,
                    DataDescription = d.DataDescription,
                    SortId = d.SortId,
                    CreateTime = d.CreateTime,
                    UpdateTime = d.UpdateTime,
                    ParentName = _context.SysDataDictionarys
                        .Where(p => p.Id == d.PId)
                        .Select(p => p.DataKey)
                        .FirstOrDefault() // 获取父级 DataKey
                })
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
