using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Repository;
using Frapper.Repository.Menus.Queries;
using Frapper.Repository.Rolemasters.Queries;
using Frapper.ViewModel.Ordering;
using Frapper.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class OrderingController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IMenuCategoryQueries _iMenuCategoryQueries;
        private readonly IUnitOfWorkDapper _unitOfWorkDapper;
        private readonly IMenuMasterQueries _menuMasterQueries;
        private readonly ISubMenuMasterQueries _subMenuMasterQueries;
        public OrderingController(IRoleQueries roleQueries, IMenuCategoryQueries menuCategoryQueries, IUnitOfWorkDapper unitOfWorkDapper, IMenuMasterQueries menuMasterQueries, ISubMenuMasterQueries subMenuMasterQueries)
        {
            _roleQueries = roleQueries;
            _iMenuCategoryQueries = menuCategoryQueries;
            _unitOfWorkDapper = unitOfWorkDapper;
            _menuMasterQueries = menuMasterQueries;
            _subMenuMasterQueries = subMenuMasterQueries;
        }


        #region MenuCategory
        [HttpGet]
        public IActionResult MenuCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MenuCategory(RequestMenuCategoryOrderVM request)
        {
            int preference = 1;
            var listofStoringOrders = new List<MenuCategoryStoringOrder>();
            if (request.SelectedOrder != null)
            {
                foreach (int menuId in request.SelectedOrder)
                {
                    listofStoringOrders.Add(new MenuCategoryStoringOrder()
                    {
                        RoleId = request.RoleId,
                        SortingOrder = preference,
                        MenuCategoryId = menuId
                    });
                    preference += 1;
                }
            }

            _unitOfWorkDapper.OrderingCommand.UpdateMenuCategoryOrder(listofStoringOrders);
            _unitOfWorkDapper.Commit();

            return View();
        }

        public JsonResult GetAllRoles()
        {
            return Json(_roleQueries.ListofRoles());
        }

        public JsonResult GetAllMenuCategorybyRoleId(int roleId)
        {
            return Json(_iMenuCategoryQueries.ListofMenubyRoleCategoryId(roleId));
        }
        #endregion

        #region MainMenu

        [HttpGet]
        public IActionResult MainMenu()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MainMenu(RequestMenuMasterOrderVM request)
        {
            int preference = 1;
            var listofStoringOrders = new List<MenuStoringOrder>();
            if (request.SelectedOrder != null)
            {
                foreach (int menuId in request.SelectedOrder)
                {
                    listofStoringOrders.Add(new MenuStoringOrder()
                    {
                        RoleId = request.RoleId,
                        MenuId = menuId,
                        SortingOrder = preference
                    });
                    preference += 1;
                }
            }

            _unitOfWorkDapper.OrderingCommand.UpdateMenuOrder(listofStoringOrders);
            _unitOfWorkDapper.Commit();
            return View();
        }

        public JsonResult GetCategorybyRoleId(int roleId)
        {
            return Json(_iMenuCategoryQueries.GetCategorybyRoleId(roleId));
        }

        public JsonResult GetAllMenubyRoleId(RequestMenu requestMenu)
        {
            return Json(_menuMasterQueries.GetListofMenu(requestMenu.RoleId, requestMenu.MenuCategoryId));
        }

        public JsonResult GetAllMenubyRoleIdSelectListItem(int roleId, int menuCategoryId)
        {
            return Json(_menuMasterQueries.ListofMenubyRoleIdSelectListItem(roleId, menuCategoryId));
        }

        public JsonResult GetAllSubMenubyRoleId(RequestSubMenu requestSubMenu)
        {
            return Json(_subMenuMasterQueries.ListofSubMenubyRoleId(requestSubMenu.RoleId, requestSubMenu.MenuId));
        }
        #endregion

        [HttpGet]
        public IActionResult SubMenu()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SubMenu(RequestSubMenuMasterOrderVM request)
        {
            int preference = 1;
            var listofStoringOrders = new List<SubMenuStoringOrder>();
            if (request.SelectedOrder != null)
            {
                foreach (int subMenuId in request.SelectedOrder)
                {
                    listofStoringOrders.Add(new SubMenuStoringOrder()
                    {
                        RoleId = request.RoleId,
                        MenuId = request.MenuId,
                        SortingOrder = preference,
                        SubMenuId = subMenuId
                    });
                    preference += 1;
                }
            }

            _unitOfWorkDapper.OrderingCommand.UpdateSubMenuOrder(listofStoringOrders);

            return View();
        }

    }
}
