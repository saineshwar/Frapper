using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using Frapper.Common;
using Frapper.ViewModel.Menus;
using Microsoft.EntityFrameworkCore;
using Frapper.Entities.Menus;
using Frapper.ViewModel.Ordering;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace Frapper.Repository.Menus.Queries
{
    public class MenuMasterQueries : IMenuMasterQueries
    {
        private readonly FrapperDbContext _frapperDbContext;
        private readonly IMemoryCache _cache;
        public MenuMasterQueries(FrapperDbContext frapperDbContext, IMemoryCache cache)
        {
            _frapperDbContext = frapperDbContext;
            _cache = cache;
        }

        public bool CheckMenuNameExists(string menuName)
        {
            try
            {
                var result = (from menu in _frapperDbContext.MenuMasters
                              where menu.MenuName == menuName
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckMenuExists(string menuName, int? roleId, int? categoryId)
        {
            try
            {
                var result = (from menu in _frapperDbContext.MenuMasters.AsNoTracking()
                              where menu.MenuName == menuName && menu.RoleId == roleId && menu.MenuCategoryId == categoryId
                              select menu).Any();

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<MenuMasterGrid> ShowAllMenus(string sortColumn, string sortColumnDir, string search)
        {
            try
            {
                var queryableMenuMaster = (from menu in _frapperDbContext.MenuMasters
                                           join category in _frapperDbContext.MenuCategorys on menu.MenuCategoryId equals category.MenuCategoryId
                                           join roleMaster in _frapperDbContext.RoleMasters on menu.RoleId equals roleMaster.RoleId
                                           orderby menu.MenuId descending
                                           select new MenuMasterGrid()
                                           {
                                               Status = menu.Status == true ? "Active" : "InActive",
                                               ActionMethod = menu.ActionMethod,
                                               MenuName = menu.MenuName,
                                               ControllerName = menu.ControllerName,
                                               MenuId = menu.MenuId,
                                               RoleName = roleMaster.RoleName,
                                               MenuCategoryName = category.MenuCategoryName,
                                               Area = string.IsNullOrEmpty(menu.Area) ? "-" : menu.Area
                                           }
                    );

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    queryableMenuMaster = queryableMenuMaster.OrderBy(sortColumn + " " + sortColumnDir);
                }


                if (!string.IsNullOrEmpty(search))
                {
                    queryableMenuMaster = queryableMenuMaster.Where(m => m.MenuName.Contains(search) || m.MenuName.Contains(search));
                }

                return queryableMenuMaster;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public EditMenuMasterViewModel GetMenuByMenuId(int? menuId)
        {
            try
            {
                var editmenu = (from menu in _frapperDbContext.MenuMasters
                                where menu.MenuId == menuId
                                select new EditMenuMasterViewModel()
                                {
                                    Status = menu.Status,
                                    ActionMethod = menu.ActionMethod,
                                    MenuName = menu.MenuName,
                                    ControllerName = menu.ControllerName,
                                    MenuId = menu.MenuId,
                                    RoleId = menu.RoleId,
                                    MenuCategoryId = menu.MenuCategoryId,
                                    Area = menu.Area
                                }).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public MenuMaster GetMenuMasterByMenuId(int? menuId)
        {
            try
            {
                var editmenu = (from menu in _frapperDbContext.MenuMasters
                                where menu.MenuId == menuId
                                select menu).FirstOrDefault();

                return editmenu;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EditValidationCheck(int? menuId, EditMenuMasterViewModel editMenu)
        {
            var result = (from menu in _frapperDbContext.MenuMasters.AsNoTracking()
                          where menu.MenuId == menuId
                          select menu).SingleOrDefault();

            if (result != null && (editMenu.MenuCategoryId == result.MenuCategoryId && editMenu.RoleId == result.RoleId && editMenu.MenuName == result.MenuName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SelectListItem> ListofMenusbyRoleId(RequestMenus requestMenus)
        {
            var listofactiveMenus = (from tempmenu in _frapperDbContext.MenuMasters
                                     where tempmenu.Status == true && tempmenu.RoleId == requestMenus.RoleID && tempmenu.MenuCategoryId == requestMenus.CategoryID
                                     orderby tempmenu.MenuId ascending
                                     select new SelectListItem
                                     {
                                         Value = tempmenu.MenuId.ToString(),
                                         Text = tempmenu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select---"
            });
            return listofactiveMenus;
        }

        public List<MenuMaster> GetMenuByRoleId(int? roleId, int? menuCategoryId)
        {
            var key = $"{AllMemoryCacheKeys.MenuMasterKey}_{roleId}";
            List<MenuMaster> menuList;
            if (_cache.Get(key) == null)
            {
                var result = (from menu in _frapperDbContext.MenuMasters.AsNoTracking()
                              orderby menu.SortingOrder ascending
                              where menu.Status == true
                              select menu).ToList();

                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(7),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set<List<MenuMaster>>(key, result, cacheExpirationOptions);

                menuList = ((from menu in result
                             orderby menu.SortingOrder ascending
                             where menu.Status == true && menu.RoleId == roleId && menu.MenuCategoryId == menuCategoryId
                             select menu).ToList());
            }
            else
            {
                var storeMenuMasters = _cache.Get(key) as List<MenuMaster>;

                menuList = ((from menu in storeMenuMasters
                             orderby menu.SortingOrder ascending
                             where menu.Status == true && menu.RoleId == roleId && menu.MenuCategoryId == menuCategoryId
                             select menu).ToList());
            }

            return menuList;
        }

        public List<MenuMasterOrderingVm> GetListofMenu(int roleId, int menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _frapperDbContext.MenuMasters
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId && tempmenu.MenuCategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new MenuMasterOrderingVm
                                     {
                                         MenuId = tempmenu.MenuId,
                                         MenuName = tempmenu.MenuName
                                     }).ToList();

            return listofactiveMenus;
        }

        public List<SelectListItem> ListofMenubyRoleIdSelectListItem(int roleId, int menuCategoryId)
        {
            var listofactiveMenus = (from tempmenu in _frapperDbContext.MenuMasters
                                     where tempmenu.Status == true && tempmenu.RoleId == roleId && tempmenu.MenuCategoryId == menuCategoryId
                                     orderby tempmenu.SortingOrder ascending
                                     select new SelectListItem
                                     {
                                         Value = tempmenu.MenuId.ToString(),
                                         Text = tempmenu.MenuName
                                     }).ToList();

            listofactiveMenus.Insert(0, new SelectListItem()
            {
                Value = "",
                Text = "---Select Main Menu---"
            });

            return listofactiveMenus;
        }
    }
}