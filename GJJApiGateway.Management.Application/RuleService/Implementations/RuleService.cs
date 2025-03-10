using AutoMapper;
using GJJApiGateway.Management.Application.AccountService.Commands;
using GJJApiGateway.Management.Application.AccountService.DTOs;
using GJJApiGateway.Management.Application.RuleService.Commands;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.RuleService.Interfaces;
using GJJApiGateway.Management.Application.RuleService.Queries;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Application.Shared.Queries;
using GJJApiGateway.Management.Common.Utilities;
using GJJApiGateway.Management.Infrastructure.Configuration;
using GJJApiGateway.Management.Infrastructure.Repositories;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.RuleService.Implementations
{
    public class RuleService : IRuleService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRuleQuery _ruleQuery;
        private readonly IRuleCommand _ruleCommand;
        private readonly IUserInfoCommand _userInfoCommand;
        private readonly IUserInfoQuery _userInfoQuery;
        private readonly PasswordSettings _passwordSettings;
        private readonly IMapper _mapper;

        public RuleService(
            IMenuRepository menuRepository,
            IRuleQuery ruleQuery,
            IRuleCommand ruleCommand,
            IUserInfoCommand userInfoCommand,
            IUserInfoQuery userInfoQuery,
            PasswordSettings passwordSettings,
            IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _ruleQuery = ruleQuery;
            _ruleCommand = ruleCommand;
            _userInfoCommand = userInfoCommand;
            _userInfoQuery = userInfoQuery;
            _passwordSettings = passwordSettings;
        }
        //---------------------------------------------------菜单-----------------------------------------------------

        public async Task<List<A_SysMenuDto>> GetMenuTreeListAsync(int pId, bool isFilterDisabledMenu)
        {
            var menuDtos = await _ruleQuery.GetMenusAsync(pId, isFilterDisabledMenu);

            if (menuDtos.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (var item in menuDtos)
                {
                    item.Subs = await GetMenuTreeListAsync(item.Id, isFilterDisabledMenu);
                }
                return menuDtos;
            }
        }

        public async Task<List<A_SysMenuDto>> GetSysMenusAsync(int roleId, int pId, bool isFilterDisabledMenu)
        {
            var menus = await _menuRepository.GetSysMenusAsync(roleId, pId, isFilterDisabledMenu);

            var menuList = new List<A_SysMenuDto>();

            foreach (var item in menus)
            {
                var menu = new A_SysMenuDto()
                {
                    Id = item.ID,
                    Name = item.NAME,
                    Icon = item.ICON,
                    IsEnable = item.ISENABLE,
                    PId = item.PID,
                    Path = item.PATH,
                    SortId = item.SORTID,
                    Alias = item.ALIAS,
                    Remark = item.REMARK,
                    Subs = await GetSysMenusAsync(roleId, item.ID, isFilterDisabledMenu) // 递归获取子菜单
                };
                menuList.Add(menu);
            }

            // 这里假设您已经有一个映射逻辑来将 `v_SysMenu` 转换为 `MenuDTO`
            return _mapper.Map<List<A_SysMenuDto>>(menuList);
        }

        public async Task<ServiceResult<string>> AddOrModifyMenuAsync(A_SysMenuDto menuDto)
        {
            // 将 DTO 转换为数据层模型
            var model = _mapper.Map<SysMenu>(menuDto);

            if (menuDto.Id > 0) // 修改
            {
                var updateResult = await _menuRepository.UpdateMenuAsync(model);
                if (updateResult > 0)
                {
                    return ServiceResult<string>.Success("修改成功！");
                }
                else
                {
                    return ServiceResult<string>.Fail("修改失败！");
                }
            }
            else // 添加
            {
                var insertResult = await _menuRepository.AddMenuAsync(model);
                if (insertResult > 0)
                {
                    return ServiceResult<string>.Success("添加成功！");
                }
                else
                {
                    return ServiceResult<string>.Fail("添加失败！");
                }
            }
        }

        /// <summary>
        /// 获取父级菜单枚举
        /// </summary>
        public async Task<List<A_SysMenuDto>> GetParentMenuEnumsAsync()
        {
            var parentMenus = await _menuRepository.GetParentMenuEnumsAsync();
            return _mapper.Map<List<A_SysMenuDto>>(parentMenus);

        }

        /// <summary>
        /// 载入修改菜单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<A_SysMenuDto> LoadModifyMenuAsync(int id)
        {
            var menu = await _menuRepository.LoadModifyMenuAsync(id);
            return _mapper.Map<A_SysMenuDto>(menu);
        }

        /// <summary>
        /// 删除指定菜单及其所有子菜单
        /// </summary>
        public async Task<ServiceResult<string>> DeleteMenuAsync(int id)
        {
            var existingMenu = await _menuRepository.GetMenuByIdAsync(id);

            if (existingMenu == null)
            {
                return ServiceResult<string>.Fail("菜单不存在");
            }

            // 如果是父级菜单，先删除所有子菜单
            if (existingMenu.PID == 0)
            {
                var childDeleteResult = await _menuRepository.DeleteChildMenusAsync(id);
                if (childDeleteResult <= 0)
                {
                    //return ServiceResult<string>.Fail("删除子菜单失败");
                }
            }

            // 删除当前菜单
            var deleteResult = await _menuRepository.DeleteMenuAsync(id);
            if (deleteResult > 0)
            {
                return ServiceResult<string>.Success("删除成功");
            }
            else
            {
                return ServiceResult<string>.Fail("删除失败");
            }
        }
        
        /// <summary>
        /// 设置角色菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public async Task<ServiceResult<string>> SettingRoleMenuAsync(int roleId, List<int> menuIds)
        {
            int insertedRow = 0;
            int deletedRow = 0;

            List<A_SysRoleMenuDto> addList = new List<A_SysRoleMenuDto>();
            List<A_SysRoleMenuDto> deleteList = new List<A_SysRoleMenuDto>();

            var existingRoleMenuDtos = await _ruleQuery.GetRoleMenusByRoleIdAsync(roleId);

            // 找出需要新增的菜单
            foreach (var item in menuIds)
            {
                if (!existingRoleMenuDtos.Any(x => x.MenuId == item))
                {
                    addList.Add(new A_SysRoleMenuDto
                    {
                        RoleId = roleId,
                        MenuId = item,
                        CanAdd = 1,
                        CanAudit = 1,
                        CanEdit = 1,
                        CanDelete = 1
                    });
                }
            }

            // 找出需要删除的菜单
            foreach (var item in existingRoleMenuDtos)
            {
                if (!menuIds.Contains(item.MenuId))
                {
                    deleteList.Add(item);
                }
            }

            if (addList.Any())
            {
                insertedRow = await _ruleCommand.InsertRoleMenusAsync(addList);
            }

            if (deleteList.Any())
            {
                deletedRow = await _ruleCommand.DeleteRoleMenusAsync(deleteList);
            }
                
            return (insertedRow > 0 || deletedRow > 0) 
                ? ServiceResult<string>.Success("设置成功") 
                : ServiceResult<string>.Fail("设置失败");
        }
        
        //---------------------------------------------------用户-----------------------------------------------------
        
        public async Task<ServiceResult<PageResult<A_SysUserInfoDto>>> UserListAsync(string userName, string roleId, int pageIndex = 1, int pageSize = 10)
        {
            // 调用仓储层方法获取用户分页列表
            var pager = await _ruleQuery.GetPagedUsersAsync(userName, roleId, pageIndex, pageSize);

            return pager.Total > 0
                ? ServiceResult<PageResult<A_SysUserInfoDto>>.Success(pager, "获取用户列表成功")
                : ServiceResult<PageResult<A_SysUserInfoDto>>.Fail("未找到用户");
        }

        public async Task<ServiceResult<string>> AddOrModifyUserAsync(
            int id, string userName, string password, string roleId, string realName, string remark)
        {

            string salt = _passwordSettings.Salt;

            if (id == 0)
            {
                // 检查用户是否已存在
                var existingUser = await _userInfoQuery.GetUserByNameAsync(0, userName);
                if (existingUser != null)
                {
                    return ServiceResult<string>.Fail("用户已存在");
                }

                var newUser = new A_SysUserInfoDto()
                {
                    Name = userName,
                    Password = EncryptionHelper.MD5Encoding(password, salt),//Common.MD5Encoding(password, salt), // MD5 加密
                    Salt = salt,
                    RoleId = roleId,
                    RealName = realName,
                    Remark = remark,
                    CreateTime = DateTime.Now
                };

                int insertResult = await _ruleCommand.InsertUserAsync(newUser);
                return insertResult > 0
                    ? ServiceResult<string>.Success("新增成功")
                    : ServiceResult<string>.Fail("新增失败");
            }
            else
            {
                // 修改操作
                var existingUser = await _userInfoQuery.GetUserByNameAsync(id, "");
                if (existingUser == null)
                {
                    return ServiceResult<string>.Fail("用户不存在");
                }

                existingUser.Name = userName;
                // existingUser.PASSWORD = Common.MD5Encoding(password, salt);
                existingUser.RoleId = roleId;
                existingUser.RealName = realName;
                existingUser.Remark = remark;

                int updateResult = await _userInfoCommand.UpdateUserInfoAsync(existingUser);
                return updateResult > 0
                    ? ServiceResult<string>.Success("修改成功")
                    : ServiceResult<string>.Fail("修改失败");
            }
        }

        public async Task<ServiceResult<string>> DeleteUserAsync(int id)
        {
            // 先检查用户是否存在
            var existingUser = await _userInfoQuery.GetUserByNameAsync(id, "");
            if (existingUser == null)
            {
                return ServiceResult<string>.Fail("用户不存在");
            }

            // 删除用户
            int deleteResult = await _ruleCommand.DeleteUserAsync(existingUser);
            return deleteResult > 0
                ? ServiceResult<string>.Success("删除成功") 
                : ServiceResult<string>.Fail("删除失败");
        }
        
        public async Task<ServiceResult<A_SysUserInfoDto>> LoadModifyUserInfoAsync(string id)
        {
            if (!int.TryParse(id, out int userId))
            {
                return ServiceResult<A_SysUserInfoDto>.Fail("无效的用户ID");
            }

            // 调用仓储层获取用户信息
            var userInfoDto = await _userInfoQuery.GetUserByNameAsync(userId, "");
            if (userInfoDto == null)
            {
                return ServiceResult<A_SysUserInfoDto>.Fail("用户信息不存在");
            }


            return ServiceResult<A_SysUserInfoDto>.Success(userInfoDto, "用户信息存在");
        }
        
        //---------------------------------------------------角色-----------------------------------------------------
        
        public async Task<ServiceResult<A_SysRoleDto>> LoadModifyRoleInfoAsync(int id)
        {
            // 调用仓储层获取角色信息
            var roleDto = await _ruleQuery.GetRoleByIdAsync(id);
            if (roleDto == null)
            {
                return ServiceResult<A_SysRoleDto>.Fail("角色不存在");
            }
                
            return ServiceResult<A_SysRoleDto>.Success(roleDto, "角色信息存在");
        }

        public async Task<ServiceResult<PageResult<A_SysRoleDto>>> RoleListAsync(string roleName, int pageIndex = 1, int pageSize = 10)
        {
            // 调用仓储层获取角色分页列表
            var pager = await _ruleQuery.GetPagedRolesAsync(roleName, pageIndex, pageSize);

            return pager.Total > 0
                ? ServiceResult<PageResult<A_SysRoleDto>>.Success(pager, "获取角色列表成功")
                : ServiceResult<PageResult<A_SysRoleDto>>.Fail("未找到角色");
        }

        public async Task<ServiceResult<string>> AddOrModifyRoleAsync(int id, string roleName, string remark)
        {
            var existingRole = await _ruleQuery.GetRoleByIdAsync(id);

            if (existingRole != null)
            {
                // 角色存在，进行修改操作
                existingRole.RoleName = roleName;
                existingRole.Remark = remark;

                // 更新角色信息
                int updateResult = await _ruleCommand.UpdateRoleAsync(existingRole);
                return updateResult > 0
                    ? ServiceResult<string>.Success("角色修改成功")
                    : ServiceResult<string>.Fail("角色修改失败");
            }
            else
            {
                // 角色不存在，进行新增操作
                var newRole = new A_SysRoleDto
                {
                    RoleName = roleName,
                    Remark = remark
                };

                // 插入角色信息
                int insertResult = await _ruleCommand.InsertRoleAsync(newRole);
                return insertResult > 0
                    ? ServiceResult<string>.Success("角色新增成功")
                    : ServiceResult<string>.Fail("角色新增失败");
            }
        }

        public async Task<ServiceResult<string>> DeleteRoleAsync(int id)
        {
            // 先检查角色是否存在
            var role = await _ruleQuery.GetRoleByIdAsync(id);
            if (role == null)
            {
                return ServiceResult<string>.Fail("角色不存在");
            }

            // 删除角色
            int deleteResult = await _ruleCommand.DeleteRoleAsync(role);
            return deleteResult > 0
                ? ServiceResult<string>.Success("角色删除成功") 
                : ServiceResult<string>.Fail("角色删除失败");
        }

        //---------------------------------------------------字典-----------------------------------------------------

        public async Task<ServiceResult<PageResult<A_SysDataDictionaryDto>>> DictListAsync(int pageIndex = 1, int pageSize = 10)
        {
            // 调用仓储层获取分页数据
            var pager = await _ruleQuery.GetPagedDataDictionariesAsync(pageIndex, pageSize);

            return pager.Total > 0
                ? ServiceResult<PageResult<A_SysDataDictionaryDto>>.Success(pager, "获取字典枚举列表成功")
                : ServiceResult<PageResult<A_SysDataDictionaryDto>>.Fail("未找到字典枚举数据");
        }

        public async Task<ServiceResult<List<A_SysDataDictionaryDto>>> DictTreeListAsync(int pId)
        {
            // 调用仓储层获取数据字典树
            var dictionaryTree = await _ruleQuery.GetDataDictionaryTreeAsync(pId);

            return dictionaryTree.Any()
                ? ServiceResult<List<A_SysDataDictionaryDto>>.Success(dictionaryTree, "获取字典列表成功")
                : ServiceResult<List<A_SysDataDictionaryDto>>.Fail("未找到字典数据");
        }
        
        public async Task<ServiceResult<List<A_SysDataDictionaryDto>>> EnumTypeListAsync()
        {
            // 调用仓储层获取字典类型列表
            var dictionaryTypes = await _ruleQuery.GetEnumTypeListAsync();

            return dictionaryTypes.Any()
                ? ServiceResult<List<A_SysDataDictionaryDto>>.Success(dictionaryTypes, "获取枚举字典类型成功")
                : ServiceResult<List<A_SysDataDictionaryDto>>.Fail("未找到枚举字典类型");
        }

        public async Task<ServiceResult<A_SysDataDictionaryDto>> LoadModifyEnumInfoAsync(int id)
        {
            // 调用仓储层获取枚举字典信息
            var dictionary = await _ruleQuery.GetDataDictionaryByIdAsync(id);

            if (dictionary == null)
            {
                return ServiceResult<A_SysDataDictionaryDto>.Fail("枚举字典不存在");
            }

            return ServiceResult<A_SysDataDictionaryDto>.Success(dictionary, "获取枚举字典成功");
        }
        
        public async Task<ServiceResult<string>> AddOrModifyEnumAsync(int id, string dataKey,
            string dataKeyAlias, int pId, string dataValue, string dataDescription, int sortId)
        {
// 调用仓储层获取数据字典信息
            var dictionary = await _ruleQuery.GetDataDictionaryByIdAsync(id);
            int isUpdate = 0;

            if (dictionary == null) // 新增
            {
                var newDictionary = new A_SysDataDictionaryDto
                {
                    DataKey = dataKey,
                    DataKeyAlias = dataKeyAlias,
                    DataValue = dataValue,
                    PId = pId,
                    DataDescription = dataDescription,
                    SortId = sortId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                int insertResult = await _ruleCommand.InsertDataDictionaryAsync(newDictionary);
                return insertResult > 0
                    ? ServiceResult<string>.Success("新增成功")
                    : ServiceResult<string>.Fail("新增失败");
            }
            else // 修改
            {
                dictionary.DataKey = dataKey;
                dictionary.DataKeyAlias = dataKeyAlias;
                dictionary.DataValue = dataValue;
                dictionary.PId = pId;
                dictionary.DataDescription = dataDescription;
                dictionary.SortId = sortId;
                dictionary.UpdateTime = DateTime.UtcNow;

                isUpdate = await _ruleCommand.UpdateDataDictionaryAsync(dictionary);
            }

            return isUpdate > 0
                ? ServiceResult<string>.Success("修改成功")
                : ServiceResult<string>.Fail("修改失败");
        }

        public async Task<ServiceResult<string>> DeleteEnumAsync(int id)
        {
            // 调用仓储层获取数据字典信息
            var dictionary = await _ruleQuery.GetDataDictionaryByIdAsync(id);
            if (dictionary == null)
            {
                return ServiceResult<string>.Fail("枚举字典不存在");
            }

            // 删除字典
            int deleteResult = await _ruleCommand.DeleteDataDictionaryAsync(dictionary);
            return deleteResult > 0
                ? ServiceResult<string>.Success("删除成功！")
                : ServiceResult<string>.Fail("删除失败!");
        }
    }
}
