using System;
using System.Collections.Generic;
using System.Linq;
using Frapper.Entities.Menus;
using Frapper.ViewModel.Menus;
using Frapper.ViewModel.Ordering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.Repository.Menus.Queries
{
    public interface IMenuCategoryQueries
    {
        MenuCategory GetCategoryByMenuCategoryId(int? menuCategoryId);
        EditMenuCategoriesViewModel GetCategoryByMenuCategoryIdForEdit(int? menuCategoryId);
        List<SelectListItem> GetCategorybyRoleId(int? roleId);
        int GetCategoryCount(string menuCategoryName);
        IQueryable<MenuCategoryGridViewModel> ShowAllMenusCategory(string sortColumn, string sortColumnDir,
            string search);
        bool CheckCategoryNameExists(string menuCategoryName, int roleId);
        List<MenuCategory> GetCategoryByRoleId(int? roleId);
        List<MenuCategoryOrderingVm> ListofMenubyRoleCategoryId(int roleId);
    }
}