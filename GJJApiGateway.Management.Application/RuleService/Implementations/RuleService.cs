using AutoMapper;
using GJJApiGateway.Management.Application.RuleService.DTOs;
using GJJApiGateway.Management.Application.RuleService.Interfaces;
using GJJApiGateway.Management.Application.Shared.DTOs;
using GJJApiGateway.Management.Infrastructure.Repositories.Interfaces;
using GJJApiGateway.Management.Model.Entities;

namespace GJJApiGateway.Management.Application.RuleService.Implementations
{
    public class RuleService : IRuleService
    {
        private readonly IMenuRepository _menuRepository;


        private readonly IMapper _mapper;

        public RuleService(IMenuRepository menuRepository,IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<A_SysMenuDto>> GetMenuTreeListAsync(int pId, bool isFilterDisabledMenu)
        {
            var menus = await _menuRepository.GetMenusAsync(pId, isFilterDisabledMenu);
            var menuList = _mapper.Map<List<A_SysMenuDto>>(menus);

            if (menus.Count <= 0)
            {
                return null;
            }
            else
            {
                foreach (var item in menus)
                {
                    var menu = new A_SysMenuDto()
                    {
                        ID = item.ID,
                        NAME = item.NAME,
                        ICON = item.ICON,
                        ISENABLE = item.ISENABLE,
                        PID = item.PID,
                        PATH = item.PATH,
                        SORTID = item.SORTID,
                        ALIAS = item.ALIAS,
                        REMARK = item.REMARK,
                        Subs = await GetMenuTreeListAsync(item.ID, isFilterDisabledMenu)
                    };
                    menuList.Add(menu);
                }
                return menuList;
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
                    ID = item.ID,
                    NAME = item.NAME,
                    ICON = item.ICON,
                    ISENABLE = item.ISENABLE,
                    PID = item.PID,
                    PATH = item.PATH,
                    SORTID = item.SORTID,
                    ALIAS = item.ALIAS,
                    REMARK = item.REMARK,
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

            if (menuDto.ID > 0) // 修改
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
                    return ServiceResult<string>.Fail("删除子菜单失败");
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
    }
}
