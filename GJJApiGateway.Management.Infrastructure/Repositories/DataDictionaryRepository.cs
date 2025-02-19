using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GJJApiGateway.Management.Infrastructure.Repositories
{
    //public class DataDictionaryRepository : IDataDictionaryRepository
    //{
    //    private readonly ManagementDbContext _context;

    //    public DataDictionaryRepository(ManagementDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<List<D_SYS_DATADICTIONARY>> GetDataDictionaryListByType(string type, string val)
    //    {
    //        var query = _context.D_SYS_DATADICTIONARY.AsQueryable();
    //        query = query.Where(d => d.TYPEKEY == type && d.QDSHOW == 1);

    //        if (!string.IsNullOrEmpty(val))
    //            query = query.Where(d => d.VALUE.Contains(val.Trim()));

    //        return await query.OrderBy(d => d.SORTID).ToListAsync();
    //    }

    //    public async Task<D_SYS_DATADICTIONARY> GetDataDictionaryByType(string type)
    //    {
    //        return await _context.D_SYS_DATADICTIONARY
    //                             .Where(d => d.TYPEKEY == type && d.QDSHOW == 1)
    //                             .FirstOrDefaultAsync();
    //    }

    //    public async Task<List<D_SYS_DATADICTIONARY>> GetDataDictionaryTreeList(int pId)
    //    {
    //        return await _context.D_SYS_DATADICTIONARY
    //                             .Where(d => d.PARENTID == pId)
    //                             .OrderBy(d => d.SORTID)
    //                             .ToListAsync();
    //    }

    //    public async Task<List<D_SYS_DATADICTIONARY>> GetDataDictionaryListByParent()
    //    {
    //        return await _context.D_SYS_DATADICTIONARY
    //                             .Where(d => d.PARENTID == 0)
    //                             .OrderBy(d => d.SORTID)
    //                             .ToListAsync();
    //    }

    //    public async Task<(int code, string message)> AddOrModifyDictionary(P_AddOrModifyDictionary p)
    //    {
    //        var dictionary = await _context.D_SYS_DATADICTIONARY.FindAsync(p.id);
    //        if (dictionary != null)
    //        {
    //            dictionary.LABEL = p.LABEL;
    //            dictionary.DESCRIPTION = p.DESCRIPTION;
    //            dictionary.VAL = p.VAL;
    //            dictionary.SORTID = p.SORTID;

    //            await _context.SaveChangesAsync();
    //            return (ApiResultCodeConst.SUCCESS, "字典修改成功");
    //        }
    //        else
    //        {
    //            var newDictionary = new D_SYS_DATADICTIONARY
    //            {
    //                LABEL = p.LABEL,
    //                DESCRIPTION = p.DESCRIPTION,
    //                VAL = p.VAL,
    //                TYPEKEY = p.TYPEKEY,
    //                SORTID = p.SORTID
    //            };

    //            await _context.D_SYS_DATADICTIONARY.AddAsync(newDictionary);
    //            await _context.SaveChangesAsync();
    //            return (ApiResultCodeConst.SUCCESS, "字典新增成功");
    //        }
    //    }

    //    public async Task<(int code, string message)> DeleteEnum(int id)
    //    {
    //        var dictionary = await _context.D_SYS_DATADICTIONARY.FindAsync(id);
    //        if (dictionary == null)
    //            return (ApiResultCodeConst.ERROR, "字典不存在");

    //        _context.D_SYS_DATADICTIONARY.Remove(dictionary);
    //        await _context.SaveChangesAsync();

    //        return (ApiResultCodeConst.SUCCESS, "字典删除成功");
    //    }
    //}
}
