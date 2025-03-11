using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.AdminService.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.AdminService.Commands;

public class RuleCommand: IRuleCommand
{
    private readonly IRoleMenuRepository _roleMenuRepository;
    private readonly ISysUserInfoRepository _sysUserInfoRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IDataDictionaryRepository _dataDictionaryRepository;
    private readonly IMapper _mapper;

    public RuleCommand(
        IRoleMenuRepository roleMenuRepository,
        ISysUserInfoRepository sysUserInfoRepository,
        IDataDictionaryRepository dataDictionaryRepository,
        IRoleRepository roleRepository,
        IMapper mapper)
    {
        _roleMenuRepository = roleMenuRepository;
        _sysUserInfoRepository = sysUserInfoRepository;
        _roleRepository = roleRepository;
        _dataDictionaryRepository = dataDictionaryRepository;
        _mapper = mapper;
    }


    public async Task<int> InsertRoleMenusAsync(List<A_SysRoleMenuDto> roleMenuDtos)
    {
        var roleMenus = _mapper.Map<List<SysRoleMenu>>(roleMenuDtos);
        int row = await _roleMenuRepository.InsertRoleMenusAsync(roleMenus);
        
        return row;
    }

    public async Task<int> DeleteRoleMenusAsync(List<A_SysRoleMenuDto> roleMenuDtos)
    {
        var roleMenus = _mapper.Map<List<SysRoleMenu>>(roleMenuDtos);
        int row = await _roleMenuRepository.DeleteRoleMenusAsync(roleMenus);
        
        return row;
    }
    
    /// <summary>
    /// 插入新用户
    /// </summary>
    public async Task<int> InsertUserAsync(A_SysUserInfoDto userDto)
    {
        var user = _mapper.Map<SysUserInfo>(userDto);
        int row = await _sysUserInfoRepository.InserAsync(user);
        return row;
    }
    
    /// <summary>
    /// 删除用户
    /// </summary>
    public async Task<int> DeleteUserAsync(A_SysUserInfoDto userDto)
    {
        var user = _mapper.Map<SysUserInfo>(userDto);
        int row = await _sysUserInfoRepository.DeleteAsync(user);
        return row;
    }
    /// <summary>
    /// 更新角色信息
    /// </summary>
    public async Task<int> UpdateRoleAsync(A_SysRoleDto roleDto)
    {
        var role = _mapper.Map<SysRole>(roleDto);
        int row = await _roleRepository.UpdateRoleAsync(role);
        return row;
    }
    
    /// <summary>
    /// 插入新角色
    /// </summary>
    public async Task<int> InsertRoleAsync(A_SysRoleDto roleDto)
    {
        var role = _mapper.Map<SysRole>(roleDto);
        int row = await _roleRepository.UpdateRoleAsync(role);
        return row;
    }
    /// <summary>
    /// 删除角色
    /// </summary>
    public async Task<int> DeleteRoleAsync(A_SysRoleDto roleDto)
    {
        var role = _mapper.Map<SysRole>(roleDto);
        int row = await _roleRepository.DeleteRoleAsync(role);
        return row;
    }
    
    /// <summary>
    /// 插入新数据字典
    /// </summary>
    public async Task<int> InsertDataDictionaryAsync(A_SysDataDictionaryDto dataDictionaryDto)
    {
        var dataDictionary = _mapper.Map<SysDataDictionary>(dataDictionaryDto);
        int row = await _dataDictionaryRepository.InsertDataDictionaryAsync(dataDictionary);
        return row;
    }

    /// <summary>
    /// 更新数据字典
    /// </summary>
    public async Task<int> UpdateDataDictionaryAsync(A_SysDataDictionaryDto dataDictionaryDto)
    {
        var dataDictionary = _mapper.Map<SysDataDictionary>(dataDictionaryDto);
        int row = await _dataDictionaryRepository.UpdateDataDictionaryAsync(dataDictionary);
        return row;
    }
    
    
    /// <summary>
    /// 删除数据字典
    /// </summary>
    public async Task<int> DeleteDataDictionaryAsync(A_SysDataDictionaryDto dataDictionaryDto)
    {
        var dataDictionary = _mapper.Map<SysDataDictionary>(dataDictionaryDto);
        int row = await _dataDictionaryRepository.DeleteDataDictionaryAsync(dataDictionary);
        return row;
    }
}