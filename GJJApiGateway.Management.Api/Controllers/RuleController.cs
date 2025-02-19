using Microsoft.AspNetCore.Mvc;
using GJJApiGateway.Management.Application.Services;
using GJJApiGateway.Management.Api.ViewModel;
using GJJApiGateway.Management.Api.DTOs;
using AutoMapper;
using GJJApiGateway.Management.Application.Interfaces;
using GJJApiGateway.Management.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using GJJApiGateway.Management.Application.DTOs;

namespace GJJApiGateway.Management.Api.Controllers
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

        //[HttpGet("LoadModifyRoleMenu")]
        //public async Task<s_ApiResult<List<MenuDTO>>> LoadModifyRoleMenu(int roleId)
        //{
        //    var list = await _ruleService.GetSysMenus(roleId, 0, false);
        //    var result = _mapper.Map<List<MenuDTO>>(list);
        //    return new s_ApiResult<List<MenuDTO>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        //}

        //[HttpPost("SettingRoleMenu")]
        //public async Task<s_ApiResult> SettingRoleMenu([FromBody] P_SettingRoleMenu model)
        //{
        //    var result = await _ruleService.SettingRoleMenuAsync(model.roleId, model.menuIds);
        //    return new s_ApiResult(result.code, result.message);
        //}

        //// User Methods
        //[HttpGet("UserList")]
        //public async Task<s_ApiResult<Pager<UserDTO>>> UserList(string userName, string roleId, int pageIndex = 1, int pageSize = 10)
        //{
        //    var list = await _ruleService.UserList(userName, roleId, pageIndex, pageSize);
        //    var result = _mapper.Map<Pager<UserDTO>>(list);
        //    return new s_ApiResult<Pager<UserDTO>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        //}

        //[HttpPost("AddOrModifyUser")]
        //public async Task<s_ApiResult> AddOrModifyUser(P_AddOrModifyUser p)
        //{
        //    var r = await _ruleService.AddOrModifyUser(p.id, p.userName, p.password, p.roleId, p.realName, p.remark);
        //    return new s_ApiResult(r.code, r.message);
        //}

        //[HttpGet("DelateUser")]
        //public async Task<s_ApiResult> DeleteUser(int id)
        //{
        //    var r = await _ruleService.DeleteUser(id);
        //    return new s_ApiResult(r.code, r.message);
        //}

        //[HttpGet("LoadModifyUserInfo")]
        //public async Task<s_ApiResult<UserDTO>> LoadModifyUserInfo(string id)
        //{
        //    var r = await _ruleService.LoadModifyUserInfoAsync(id);
        //    var result = _mapper.Map<UserDTO>(r.user);
        //    return new s_ApiResult<UserDTO>(r.code, r.message, result);
        //}

        //// Role Methods
        //[HttpGet("LoadModifyRoleInfo")]
        //public async Task<s_ApiResult<RoleDTO>> LoadModifyRoleInfo(int id)
        //{
        //    var r = await _ruleService.LoadModifyRoleInfo(id);
        //    var result = _mapper.Map<RoleDTO>(r.role);
        //    return new s_ApiResult<RoleDTO>(r.code, r.message, result);
        //}

        //[HttpGet("RoleList")]
        //public async Task<s_ApiResult<Pager<RoleDTO>>> RoleList(string roleName, int pageIndex = 1, int pageSize = 10)
        //{
        //    var r = await _ruleService.RoleListAsync(roleName, pageIndex, pageSize);
        //    var result = _mapper.Map<Pager<RoleDTO>>(r.list);
        //    return new s_ApiResult<Pager<RoleDTO>>(r.code, r.message, result);
        //}

        //[HttpPost("AddOrModifyRole")]
        //public async Task<s_ApiResult> AddOrModifyRole(P_AddOrModifyRole p)
        //{
        //    var r = await _ruleService.AddOrModifyRoleAsync(p.id, p.roleName, p.remark);
        //    return new s_ApiResult(r.code, r.message);
        //}

        //[HttpPost("DeleteRole")]
        //public async Task<s_ApiResult> DeleteRole([FromForm] int id)
        //{
        //    var r = await _ruleService.DeleteRoleAsync(id);
        //    return new s_ApiResult(r.code, r.message);
        //}

        //// Data Dictionary Methods
        //[HttpGet("GetDataDictionaryListByType")]
        //public async Task<s_ApiResult<List<DataDictionaryDTO>>> GetDataDictionaryListByType(string type, string val = "")
        //{
        //    var list = await _ruleService.GetDataDictionaryListByType(type, val);
        //    var result = _mapper.Map<List<DataDictionaryDTO>>(list);
        //    return new s_ApiResult<List<DataDictionaryDTO>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        //}

        //[HttpGet("GetDataDictionaryByType")]
        //public async Task<s_ApiResult<DataDictionaryDTO>> GetDataDictionaryByType(string type)
        //{
        //    var list = await _ruleService.GetDataDictionaryByType(type);
        //    var result = _mapper.Map<DataDictionaryDTO>(list);
        //    return new s_ApiResult<DataDictionaryDTO>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        //}

        //[HttpGet("GetDataDictionaryTreeList")]
        //public async Task<s_ApiResult<List<DataDictionaryDTO>>> GetDataDictionaryTreeList()
        //{
        //    var list = await _ruleService.DataDictionaryTreeList(0);
        //    var result = _mapper.Map<List<DataDictionaryDTO>>(list);
        //    return new s_ApiResult<List<DataDictionaryDTO>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        //}

        //[HttpGet("GetDataDictionaryListByParent")]
        //public async Task<s_ApiResult<List<DataDictionaryDTO>>> GetDataDictionaryListByParent()
        //{
        //    var list = await _ruleService.GetDataDictionaryListByParent();
        //    var result = _mapper.Map<List<DataDictionaryDTO>>(list);
        //    return new s_ApiResult<List<DataDictionaryDTO>>(ApiResultCodeConst.SUCCESS, ApiResultMessageConst.SUCCESS, result);
        //}

        //[HttpGet("LoadModifyEnumInfoById")]
        //public async Task<s_ApiResult<DataDictionaryDTO>> LoadModifyEnumInfoById(int id)
        //{
        //    var r = await _ruleService.LoadModifyEnumInfoById(id);
        //    var result = _mapper.Map<DataDictionaryDTO>(r.@enum);
        //    return new s_ApiResult<DataDictionaryDTO>(r.code, r.message, result);
        //}

        //[HttpPost("AddOrModifyDictionary")]
        //public async Task<s_ApiResult> AddOrModifyDictionary(P_AddOrModifyDictionary p)
        //{
        //    var result = await _ruleService.AddOrModifyDictionary(p);
        //    return new s_ApiResult(result.code, result.message);
        //}

        //[HttpGet("DeleteEnum")]
        //public async Task<s_ApiResult> DeleteEnum(int id)
        //{
        //    var result = await _ruleService.DeleteEnumAsync(id);
        //    return new s_ApiResult(result.code, result.message);
        //}
    }
}
