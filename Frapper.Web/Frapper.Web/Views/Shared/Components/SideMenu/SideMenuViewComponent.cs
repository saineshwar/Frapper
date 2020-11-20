using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Repository.Menus.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Frapper.Web.Views.Shared.Components.SideMenu
{
    public class SideMenuViewComponent : ViewComponent
    {
        private readonly IMenuCategoryQueries _menuCategoryQueries;

        public SideMenuViewComponent(IMenuCategoryQueries menuCategoryQueries)
        {
            _menuCategoryQueries = menuCategoryQueries;
        }

        public IViewComponentResult Invoke()
        {
            var menucategoryList = _menuCategoryQueries.GetCategoryByRoleId(HttpContext.Session.GetInt32("Portal.RoleId"));

            return View(menucategoryList);
        }
    }

}
