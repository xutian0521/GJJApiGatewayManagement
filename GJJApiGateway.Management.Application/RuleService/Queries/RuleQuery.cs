using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.RuleService.Queries;

public class RuleQuery: IRuleQuery
{
    private readonly IRoleMenuRepository _roleMenuRepository;
    private readonly ISysUserInfoRepository  _sysUserInfoRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDataDictionaryRepository _dataDictionaryRepository;

    
    
    private readonly IMapper _mapper;
    
    public RuleQuery(
        IRoleMenuRepository roleMenuRepository,
        ISysUserInfoRepository sysUserInfoRepository,
        IRoleRepository roleRepository,
        IDataDictionaryRepository dataDictionaryRepository,
        IMapper mapper)
    {
        _roleMenuRepository = roleMenuRepository;
        _sysUserInfoRepository = sysUserInfoRepository;
        _roleRepository = roleRepository;
        _dataDictionaryRepository = dataDictionaryRepository;
        _mapper = mapper;
    }
    public async Task<List<A_SysRoleMenuDto>> GetRoleMenusByRoleIdAsync(int roleId)
    {
        var roleMenus = await _roleMenuRepository.GetRoleMenusByRoleIdAsync(roleId);
        var roleMenuDtos = _mapper.Map<List<A_SysRoleMenuDto>>(roleMenus);
        return roleMenuDtos;
    }

    public async Task<PageResult<A_SysUserInfoDto>> GetPagedUsersAsync(string userName, string roleId, int pageIndex, int pageSize)
    {
        var dataPaper = await _sysUserInfoRepository.GetPagedUsersAsync(userName,roleId, pageIndex, pageSize);
        var paperDto = _mapper.Map<PageResult<A_SysUserInfoDto>>(dataPaper);
        return paperDto;
    }
    
    /// <summary>
    /// 根据角色 ID 获取角色信息
    /// </summary>
    public async Task<A_SysRoleDto?> GetRoleByIdAsync(int id)
    {
        var sysRole = await _roleRepository.GetRoleById(id);
        var sysRoleDto = _mapper.Map<A_SysRoleDto>(sysRole);
        return sysRoleDto;
    }
    
    /// <summary>
    /// 获取分页角色列表
    /// </summary>
    public async Task<PageResult<A_SysRoleDto>> GetPagedRolesAsync(string roleName, int pageIndex, int pageSize)
    {
        var dataPaper = await _roleRepository.GetPagedRolesAsync(roleName, pageIndex, pageSize);
        var paperDto = _mapper.Map<PageResult<A_SysRoleDto>>(dataPaper);
        return paperDto;
    }

    public async Task<PageResult<A_SysDataDictionaryDto>> GetPagedDataDictionariesAsync(int pageIndex, int pageSize)
    {
        var dataPaper = await _dataDictionaryRepository.GetPagedDataDictionariesAsync(pageIndex, pageSize);
        var paperDto = _mapper.Map<PageResult<A_SysDataDictionaryDto>>(dataPaper);
        return paperDto;
    }

    public async Task<List<A_SysDataDictionaryDto>> GetDataDictionaryTreeAsync(int rootPId)
    {
        var datas = await _dataDictionaryRepository.GetDataDictionaryTreeAsync(rootPId);
        var dataDtos = _mapper.Map<List<A_SysDataDictionaryDto>>(datas);
        return dataDtos;
    }
    
    public async Task<List<A_SysDataDictionaryDto>> GetEnumTypeListAsync()
    {
        var datas = await _dataDictionaryRepository.GetEnumTypeListAsync();
        var dataDtos = _mapper.Map<List<A_SysDataDictionaryDto>>(datas);
        return dataDtos;
    }
    
    /// <summary>
    /// 根据 ID 获取枚举字典信息，并获取其父级名称（如果有）
    /// </summary>
    public async Task<A_SysDataDictionaryDto> GetDataDictionaryByIdAsync(int id)
    {
        var dictionary = await _dataDictionaryRepository.GetDataDictionaryByIdAsync(id);
        var dictionaryDto = _mapper.Map<A_SysDataDictionaryDto>(dictionary);
        return dictionaryDto;
    }
}