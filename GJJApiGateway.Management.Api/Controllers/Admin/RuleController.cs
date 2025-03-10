using AutoMapper;
using GJJApiGateway.Management.Api.Controllers.Admin.DTOs;
using GJJApiGateway.Management.Api.Controllers.Admin.ViewModels;
using GJJApiGateway.Management.Api.Controllers.Shared.ViewModels;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.RuleService.Interfaces;
using GJJApiGateway.Management.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace GJJApiGateway.Management.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        private readonly IRuleService _ruleService;
        private readonly IMapper _mapper;

        public RuleController(IRuleService ruleService, IMapper mapper)
        {
            _ruleService = ruleService;
            _mapper = mapper;
        }
        //---------------------------------------------------菜单-----------------------------------------------------

        // Menu Methods
        [HttpGet("MenuTreeList")]
        public async Task<s_ApiResult<List<SysMenuVM>>> MenuTreeList(bool isFilterDisabledMenu = false)
        {
            var list = await _ruleService.GetMenuTreeListAsync(0, isFilterDisabledMenu);
            var result = _mapper.Map<List<SysMenuVM>>(list);
            return new s_ApiResult<List<SysMenuVM>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        [HttpGet("GetSysMenus")]
        public async Task<s_ApiResult<List<SysMenuVM>>> GetSysMenus(bool isFilterDisabledMenu = false)
        {
            var user = this.HttpContext.Items["User"] as UserJwtPayload;

            var list = await _ruleService.GetSysMenusAsync(int.Parse(user.roleId), 0, isFilterDisabledMenu);
            var result = _mapper.Map<List<SysMenuVM>>(list);
            return new s_ApiResult<List<SysMenuVM>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        [HttpPost("AddOrModifyMenu")]
        public async Task<s_ApiResult<string>> AddOrModifyMenu(C_SysMenuDto p)
        {
            if (string.IsNullOrEmpty(p.title))
            {
                return new s_ApiResult<string>(ApiResultCodeConst.ERROR, "菜单名称不能为空", "");
            }


            var dto = _mapper.Map<A_SysMenuDto>(p);

            var result = await _ruleService.AddOrModifyMenuAsync(dto);
            return new s_ApiResult<string>(result.Code, result.Message, "");
        }

        [HttpGet("ParentMenuEnums")]
        public async Task<s_ApiResult<List<SysMenuVM>>> ParentMenuEnums()
        {
            var list = await _ruleService.GetParentMenuEnumsAsync();
            var result = _mapper.Map<List<SysMenuVM>>(list);
            return new s_ApiResult<List<SysMenuVM>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        [HttpGet("LoadModifyMenu")]
        public async Task<s_ApiResult<SysMenuVM>> LoadModifyMenu(int id)
        {
            var one = await _ruleService.LoadModifyMenuAsync(id);
            var result = _mapper.Map<SysMenuVM>(one);
            return new s_ApiResult<SysMenuVM>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        [HttpGet("DeleteMenu")]
        public async Task<s_ApiResult<string>> DeleteMenu(int id)
        {
            var result = await _ruleService.DeleteMenuAsync(id);
            return new s_ApiResult<string>(result.Code, result.Message, "");
        }

        [HttpGet("LoadModifyRoleMenu")]
        public async Task<s_ApiResult<List<SysMenuVM>>> LoadModifyRoleMenu(int roleId)
        {
            var list = await _ruleService.GetSysMenusAsync(roleId, 0, false);
            var result = _mapper.Map<List<SysMenuVM>>(list);
            return new s_ApiResult<List<SysMenuVM>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        [HttpPost("SettingRoleMenu")]
        public async Task<s_ApiResult<string>> SettingRoleMenu([FromBody] C_SettingRoleMenuDto model)
        {
            var result = await _ruleService.SettingRoleMenuAsync(model.roleId, model.menuIds);
            return new s_ApiResult<string>(result.Code, result.Message, "");
        }


        //---------------------------------------------------用户-----------------------------------------------------
        
        // User Methods
        [HttpGet("UserList")]
        public async Task<s_ApiResult<Pager<C_SysUserInfoDto>>> UserList(string userName = "", string roleId = "", int pageIndex = 1, int pageSize = 10)
        {
            var data = await _ruleService.UserListAsync(userName, roleId, pageIndex, pageSize);
            var result = _mapper.Map<Pager<C_SysUserInfoDto>>(data.Data);
            return new s_ApiResult<Pager<C_SysUserInfoDto>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        }

        [HttpPost("AddOrModifyUser")]
        public async Task<s_ApiResult<string>> AddOrModifyUser([FromBody] C_AddOrModifyUserDto p)
        {
            var r = await _ruleService.AddOrModifyUserAsync(p.id, p.userName, p.password, p.roleId, p.realName, p.remark);
            return new s_ApiResult<string>(r.Code, r.Message, "");
        }

        [HttpGet("DelateUser")]
        public async Task<s_ApiResult<string>> DeleteUser(int id)
        {
            var r = await _ruleService.DeleteUserAsync(id);
            return new s_ApiResult<string>(r.Code, r.Message, "");
        }

        [HttpGet("LoadModifyUserInfo")]
        public async Task<s_ApiResult<C_SysUserInfoDto>> LoadModifyUserInfo(string id)
        {
            var r = await _ruleService.LoadModifyUserInfoAsync(id);
            var result = _mapper.Map<C_SysUserInfoDto>(r.Data);
            return new s_ApiResult<C_SysUserInfoDto>(r.Code, r.Message, result);
        }

        //---------------------------------------------------角色-----------------------------------------------------
        
        // Role Methods
        [HttpGet("LoadModifyRoleInfo")]
        public async Task<s_ApiResult<C_SysRoleDto>> LoadModifyRoleInfo(int id)
        {
            var r = await _ruleService.LoadModifyRoleInfoAsync(id);
            var result = _mapper.Map<C_SysRoleDto>(r.Data);
            return new s_ApiResult<C_SysRoleDto>(r.Code, r.Message, result);
        }

        [HttpGet("RoleList")]
        public async Task<s_ApiResult<Pager<C_SysRoleDto>>> RoleList(string roleName= "", int pageIndex = 1, int pageSize = 10)
        {
            var r = await _ruleService.RoleListAsync(roleName, pageIndex, pageSize);
            var result = _mapper.Map<Pager<C_SysRoleDto>>(r.Data);
            return new s_ApiResult<Pager<C_SysRoleDto>>(r.Code, r.Message, result);
        }

        [HttpPost("AddOrModifyRole")]
        public async Task<s_ApiResult<string>> AddOrModifyRole([FromBody] C_AddOrModifyRoleDto p)
        {
            var r = await _ruleService.AddOrModifyRoleAsync(p.id, p.roleName, p.remark);
            return new s_ApiResult<string>(r.Code, r.Message, "");
        }

        [HttpPost("DeleteRole")]
        public async Task<s_ApiResult<string>> DeleteRole([FromForm] int id)
        {
            var r = await _ruleService.DeleteRoleAsync(id);
            return new s_ApiResult<string>(r.Code, r.Message, "");
        }
        
        //---------------------------------------------------字典-----------------------------------------------------

// 数据字典分页列表
    [HttpGet("DictEnumList")]
    public async Task<s_ApiResult<Pager<C_SysDataDictionaryDto>>> DictEnumList(int pageIndex = 1, int pageSize = 10)
    {
        var list = await _ruleService.DictListAsync(pageIndex, pageSize);
        var result = _mapper.Map<Pager<C_SysDataDictionaryDto>>(list);
        return new s_ApiResult<Pager<C_SysDataDictionaryDto>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
    }

    // 递归获取字典树列表
    [HttpGet("DictTreeList")]
    public async Task<s_ApiResult<List<C_SysDataDictionaryDto>>> DictTreeList()
    {
        var list = await _ruleService.DictTreeListAsync(0);
        var result = _mapper.Map<List<C_SysDataDictionaryDto>>(list);
        return new s_ApiResult<List<C_SysDataDictionaryDto>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
    }

    // 获取数据字典类型（PId=0）
    [HttpGet("EnumTypeList")]
    public async Task<s_ApiResult<List<C_SysDataDictionaryDto>>> EnumTypeList()
    {
        var list = await _ruleService.EnumTypeListAsync();
        var result = _mapper.Map<List<C_SysDataDictionaryDto>>(list);
        return new s_ApiResult<List<C_SysDataDictionaryDto>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
    }

    // 载入修改枚举字典
    [HttpGet("LoadModifyEnumInfo")]
    public async Task<s_ApiResult<C_SysDataDictionaryDto>> LoadModifyEnumInfo(int id)
    {
        var entity = await _ruleService.LoadModifyEnumInfoAsync(id);
        var result = _mapper.Map<C_SysDataDictionaryDto>(entity.Data);
        return new s_ApiResult<C_SysDataDictionaryDto>(entity.Code, entity.Message, result);
    }

    // 新增或修改枚举字典
    [HttpPost("AddOrModifyEnum")]
    public async Task<s_ApiResult<string>> AddOrModifyEnum([FromBody] C_AddOrModifyDataDictionaryDto p)
    {
        var result = await _ruleService.AddOrModifyEnumAsync(
            p.id, p.dataKey, p.dataKeyAlias, p.pId, p.dataValue, p.dataDescription, p.sortId);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }

    // 删除枚举字典
    [HttpPost("DeleteEnum")]
    public async Task<s_ApiResult<string>> DeleteEnum([FromBody] int id)
    {
        var result = await _ruleService.DeleteEnumAsync(id);
        return new s_ApiResult<string>(result.Code, result.Message, "");
    }
    }
}
