using System.Linq.Dynamic.Core;
using System.Linq;
using System;
using System.Collections.Generic;
using Frapper.Common;
using Frapper.Entities.Menus;
using Frapper.ViewModel.Menus;
using Frapper.ViewModel.Ordering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Frapper.Repository.Menus.Queries
{
    public class SubMenuMasterQueries : ISubMenuMasterQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        private readonly IMemoryCache _cache;
        public SubMenuMasterQueries(FrapperDbContext frapperDbContext, IMemoryCache cache)
        {
            _frapperDbContext = frapperDbContext;
            _cache = cache;
        }

        public bool CheckSubMenuNameExists(string subMenuName, int menuId)
        {
            try
            {
                var result = (from submenu in _frapperDbContext.SubMenuMasters.AsNoTracking()
                              where submenu.SubMenuName == subMenuName && submenu.MenuId == menuId
                              select submenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditSubMenuMasterViewModel GetSubMenuById(int? subMenuId)
        {
            try
            {
                var result = (from submenu in _frapperDbContext.SubMenuMasters
                              where submenu.SubMenuId == subMenuId
                              select new EditSubMenuMasterViewModel()
                              {
                                  RoleId = submenu.RoleId,
                                  Status = submenu.Status,
                                  MenuCategoryId = submenu.MenuCategoryId,
                                  SubMenuName = submenu.SubMenuName,
                                  ControllerName = submenu.ControllerName,
                                  ActionMethod = submenu.ActionMethod,
                                  MenuId = submenu.MenuId,
                                  SubMenuId = submenu.SubMenuId,
                                  Area = submenu.Area

                              }).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckSubMenuNameExists(string subMenuName, int? menuId, int? roleId, int? categoryId)
        {
            try
            {
                var result = (from subMenu in _frapperDbContext.SubMenuMasters.AsNoTracking()
                              where subMenu.SubMenuName == subMenuName
                                    && subMenu.MenuId == menuId
                                    && subMenu.RoleId == roleId
                                    && subMenu.MenuCategoryId == categoryId
                              select subMenu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<SubMenuMasterViewModel> ShowAllSubMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryablesSubMenuMasters = (from submenu in _frapperDbContext.SubMenuMasters
                                                join category in _frapperDbContext.MenuCategorys on submenu.MenuCategoryId equals category.MenuCategoryId
                                                join roleMaster in _frapperDbContext.RoleMasters on submenu.RoleId equals roleMaster.RoleId
                                                join menuMaster in _frapperDbContext.MenuMasters on submenu.MenuId equals menuMaster.MenuId

                                                select new SubMenuMasterViewModel
                                                {
                                                    SubMenuName = submenu.SubMenuName,
                                                    MenuName = menuMaster.MenuName,
                                                    ActionMethod = submenu.ActionMethod,
                                                    ControllerName = submenu.ControllerName,
                                                    Status = submenu.Status == true ? "Active" : "InActive",
                                                    SubMenuId = submenu.SubMenuId,
                                                    RoleName = roleMaster.RoleName,
                                                    MenuCategoryName = category.MenuCategoryName,
                                                    Area = string.IsNullOrEmpty(submenu.Area) ? "---" : submenu.Area
                                                });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(search))
                {
                    queryablesSubMenuMasters = queryablesSubMenuMasters.Where(m => m.SubMenuName.Contains(search) || m.SubMenuName.Contains(search));
                }

                return queryablesSubMenuMasters;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool EditValidationCheck(int? subMenuId, EditSubMenuMasterViewModel editsubMenu)
        {
            var result = (from submenu in _frapperDbContext.SubMenuMasters.AsNoTracking()
                          where submenu.SubMenuId == subMenuId
                          select submenu).SingleOrDefault();

            if (result != null && (editsubMenu.MenuId == result.MenuId
                                   && editsubMenu.MenuCategoryId == result.MenuCategoryId
                                   && editsubMenu.RoleId == result.RoleId
                                   && editsubMenu.SubMenuName == result.SubMenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public SubMenuMaster GetSubMenuBySubMenuId(int? subMenuId)
        {
            try
            {
                var result = (from submenu in _frapperDbContext.SubMenuMasters
                              where submenu.SubMenuId == subMenuId
                              select submenu).SingleOrDefault();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SubMenuMaster> GetSubMenuByRoleId(int? roleId, int? menuCategoryId, int menuid)
        {
            List<SubMenuMaster> subMenuList;
            if (_cache.Get(AllMemoryCacheKeys.MenuSubMenuKey) == null)
            {
                var result = (from subMenu in _frapperDbContext.SubMenuMasters.AsNoTracking()
                              where subMenu.Status == true
                              select subMenu).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set<List<SubMenuMaster>>(AllMemoryCacheKeys.MenuSubMenuKey, result, cacheExpirationOptions);

                subMenuList = (from subMenu in result
                               where subMenu.RoleId == roleId && subMenu.Status == true && subMenu.MenuCategoryId == menuCategoryId && subMenu.MenuId == menuid
                               select subMenu).ToList();

            }
            else
            {
                var storeSubMenuMasters = _cache.Get(AllMemoryCacheKeys.MenuSubMenuKey) as List<SubMenuMaster>;

                subMenuList = (from subMenu in storeSubMenuMasters
                               where subMenu.RoleId == roleId && subMenu.Status == true && subMenu.MenuCategoryId == menuCategoryId && subMenu.MenuId == menuid
                               select subMenu).ToList();
            }

            return subMenuList;
        }

        public List<SubMenuMasterOrderingVm> ListofSubMenubyRoleId(int roleId, int menuid)
        {
            var listofactivesubMenus = (from tempsubmenu in _frapperDbContext.SubMenuMasters
                                     where tempsubmenu.Status == true && tempsubmenu.RoleId == roleId && tempsubmenu.MenuId == menuid
                                     orderby tempsubmenu.SortingOrder ascending
                                     select new SubMenuMasterOrderingVm
                                     {
                                         SubMenuId = tempsubmenu.SubMenuId,
                                         SubMenuName = tempsubmenu.SubMenuName
                                     }).ToList();

            return listofactivesubMenus;
        }
    }
}