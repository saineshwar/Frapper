using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Frapper.Common;
using Frapper.Entities.Menus;
using Frapper.Repository;
using Frapper.Repository.Menus.Command;
using Frapper.Repository.Menus.Queries;
using Frapper.Repository.Rolemasters.Queries;
using Frapper.ViewModel.Menus;
using Frapper.Web.Extensions;
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class SubMenuMasterController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IMenuCategoryQueries _menuCategoryQueries;
        private readonly IMenuMasterQueries _iMenuMasterQueries;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ISubMenuMasterQueries _subMenuMasterQueries;

        public SubMenuMasterController(IRoleQueries roleQueries,
            IUnitOfWorkEntityFramework unitOfWorkEntityFramework,
            IMenuCategoryQueries menuCategoryQueries,
            IMenuMasterQueries iMenuMasterQueries,
            IMapper mapper, INotificationService notificationService, ISubMenuMasterQueries subMenuMasterQueries)
        {
            _roleQueries = roleQueries;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _menuCategoryQueries = menuCategoryQueries;
            _iMenuMasterQueries = iMenuMasterQueries;
            _mapper = mapper;
            _notificationService = notificationService;
            _subMenuMasterQueries = subMenuMasterQueries;
        }


        [HttpGet]
        public IActionResult Create()
        {

            try
            {
                var subMenu = new CreateSubMenuMasterViewModel();
                subMenu.ListofMenus = new List<SelectListItem>()
                    {new SelectListItem() {Value = "", Text = "-----Select-----"}};
                subMenu.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem() {Value = "", Text = "-----Select-----"}
                };
                subMenu.ListofRoles = _roleQueries.ListofRoles();

                return View(subMenu);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateSubMenuMasterViewModel subMenu)
        {

            subMenu.ListofMenus = new List<SelectListItem>()
                {new SelectListItem() {Value = "", Text = "-----Select-----"}};
            subMenu.ListofMenuCategory = new List<SelectListItem>()
            {
                new SelectListItem() {Value = "", Text = "-----Select-----"}
            };
            subMenu.ListofRoles = _roleQueries.ListofRoles();

            if (ModelState.IsValid)
            {
                if (_subMenuMasterQueries.CheckSubMenuNameExists(subMenu.SubMenuName, subMenu.MenuId, subMenu.RoleId,
                    subMenu.MenuCategoryId))
                {
                    ModelState.AddModelError("", "SubMenu Name Already Exists");
                    return View(subMenu);
                }

                var mappedobject = _mapper.Map<SubMenuMaster>(subMenu);
                mappedobject.CreatedOn = DateTime.Now;
                mappedobject.CreatedBy = Convert.ToInt64(HttpContext.Session.Get<string>(AllSessionKeys.UserId));
                _unitOfWorkEntityFramework.SubMenuMasterCommand.Add(mappedobject);
                var result = _unitOfWorkEntityFramework.Commit();

                if (result)
                {
                    _notificationService.SuccessNotification("Message", "SubMenu was added Successfully!");
                    return RedirectToAction("Index", "SubMenuMaster");
                }
            }

            return View(subMenu);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllSubMenu()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request
                    .Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var records = _subMenuMasterQueries.ShowAllSubMenus(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = records.Count();
                var data = records.Skip(skip).Take(pageSize).ToList();
                var jsonData = new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult GetMenus(RequestMenus requestMenus)
        {
            var listofMenus = _iMenuMasterQueries.ListofMenusbyRoleId(requestMenus);
            return Json(listofMenus);
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                var submenuMaster = _subMenuMasterQueries.GetSubMenuById(id);
                submenuMaster.ListofRoles = _roleQueries.ListofRoles();
                submenuMaster.ListofMenus = new List<SelectListItem>()
                    {new SelectListItem() {Value = "", Text = "-----Select-----"}};
                submenuMaster.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem() {Value = "", Text = "-----Select-----"}
                };

                return View(submenuMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditSubMenuMasterViewModel subMenuMasterVm)
        {
            try
            {
                subMenuMasterVm.ListofRoles = _roleQueries.ListofRoles();
                subMenuMasterVm.ListofMenuCategory = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };
                subMenuMasterVm.ListofMenus = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Value = "",
                        Text = "-----Select-----"
                    }
                };

                if (ModelState.IsValid)
                {
                    if (_subMenuMasterQueries.EditValidationCheck(subMenuMasterVm.SubMenuId, subMenuMasterVm))
                    {
                        SubMenuMaster subMenuMaster = new SubMenuMaster()
                        {
                            SubMenuId = subMenuMasterVm.SubMenuId,
                            RoleId = subMenuMasterVm.RoleId,
                            MenuCategoryId = subMenuMasterVm.MenuCategoryId,
                            MenuId = subMenuMasterVm.MenuId,
                            Status = subMenuMasterVm.Status,
                            ActionMethod = subMenuMasterVm.ActionMethod,
                            Area = subMenuMasterVm.Area,
                            ControllerName = subMenuMasterVm.ControllerName,
                            SubMenuName = subMenuMasterVm.SubMenuName,
                            CreatedOn = DateTime.Now,
                        };
                        subMenuMaster.CreatedBy = Convert.ToInt64(HttpContext.Session.Get<string>(AllSessionKeys.UserId));
                        _unitOfWorkEntityFramework.SubMenuMasterCommand.Update(subMenuMaster);
                        _unitOfWorkEntityFramework.Commit();

                        _notificationService.SuccessNotification("Message", "SubMenu was Updated Successfully!");
                        return RedirectToAction("Index", "SubMenuMaster");
                    }
                    else if (_subMenuMasterQueries.CheckSubMenuNameExists(subMenuMasterVm.SubMenuName, subMenuMasterVm.MenuId,
                        subMenuMasterVm.RoleId, subMenuMasterVm.MenuCategoryId))
                    {
                        ModelState.AddModelError("", "SubMenu Already Exists");
                        return View(subMenuMasterVm);
                    }
                    else
                    {
                        SubMenuMaster subMenuMaster = new SubMenuMaster()
                        {
                            SubMenuId = subMenuMasterVm.SubMenuId,
                            RoleId = subMenuMasterVm.RoleId,
                            MenuCategoryId = subMenuMasterVm.MenuCategoryId,
                            MenuId = subMenuMasterVm.MenuId,
                            Status = subMenuMasterVm.Status,
                            Area = subMenuMasterVm.Area,
                            ControllerName = subMenuMasterVm.ControllerName,
                            ActionMethod = subMenuMasterVm.ActionMethod,
                            SubMenuName = subMenuMasterVm.SubMenuName,
                            CreatedOn = DateTime.Now
                        };
                        subMenuMaster.CreatedBy = Convert.ToInt64(HttpContext.Session.Get<string>(AllSessionKeys.UserId));
                        _unitOfWorkEntityFramework.SubMenuMasterCommand.Update(subMenuMaster);
                        _unitOfWorkEntityFramework.Commit();

                        _notificationService.SuccessNotification("Message", "SubMenu was Updated Successfully!");
                    }

                    return RedirectToAction("Index", "SubMenuMaster");
                }

                return View(subMenuMasterVm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult DeleteSubMenu(RequestDeleteSubMenu request)
        {
            try
            {
                var data = _subMenuMasterQueries.GetSubMenuBySubMenuId(request.SubMenuId);
                _unitOfWorkEntityFramework.SubMenuMasterCommand.Delete(data);
                var result = _unitOfWorkEntityFramework.Commit();

                if (result)
                {
                    _notificationService.SuccessNotification("Message", "The SubMenu Deleted successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }
    }
}
