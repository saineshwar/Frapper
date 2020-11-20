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
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class MenuCategoryController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IMapper _mapper;
        private readonly IMenuCategoryQueries _menuCategoryQueries;

        public MenuCategoryController(IRoleQueries roleQueries,
            INotificationService notificationService,
            IUnitOfWorkEntityFramework iUnitOfWork,
            IMapper mapper,
            IMenuCategoryQueries menuCategoryQueries)
        {
            _roleQueries = roleQueries;
            _notificationService = notificationService;
            _unitOfWorkEntityFramework = iUnitOfWork;
            _mapper = mapper;
            _menuCategoryQueries = menuCategoryQueries;
        }

        public IActionResult Create()
        {
            CreateMenuCategoryViewModel addCategoriesVm = new CreateMenuCategoryViewModel()
            {
                ListofRoles = _roleQueries.ListofRoles(),
                Status = true
            };
            return View(addCategoriesVm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateMenuCategoryViewModel menuCategory)
        {
            menuCategory.ListofRoles = _roleQueries.ListofRoles();

            if (ModelState.IsValid)
            {
                if (_menuCategoryQueries.CheckCategoryNameExists(menuCategory.MenuCategoryName, menuCategory.RoleId))
                {
                    ModelState.AddModelError("", "Menu Category Already Exists!");
                    return View(menuCategory);
                }
                else
                {
                    var mappedobject = _mapper.Map<MenuCategory>(menuCategory);
                    mappedobject.CreatedOn = DateTime.Now;
                    mappedobject.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));
                    _unitOfWorkEntityFramework.MenuCategoryCommand.Add(mappedobject);
                    var result = _unitOfWorkEntityFramework.Commit();

                    if (result)
                    {
                        _notificationService.SuccessNotification("Message",
                            "The Menu Category was added Successfully!");
                        return RedirectToAction("Index", "MenuCategory");
                    }
                }
            }

            return View(menuCategory);
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GridAllMenuCategory()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var rolesdata = _menuCategoryQueries.ShowAllMenusCategory(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = rolesdata.Count();
                var data = rolesdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var editdata = _menuCategoryQueries.GetCategoryByMenuCategoryIdForEdit(id);
                editdata.ListofRoles = _roleQueries.ListofRoles();
                return View(editdata);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditMenuCategoriesViewModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var editdata = _menuCategoryQueries.GetCategoryByMenuCategoryId(category.MenuCategoryId);

                    if (editdata.RoleId == category.RoleId && editdata.MenuCategoryName == category.MenuCategoryName)
                    {
                        MenuCategory categories = new MenuCategory()
                        {
                            RoleId = category.RoleId,
                            MenuCategoryName = category.MenuCategoryName,
                            Status = category.Status,
                            MenuCategoryId = category.MenuCategoryId,
                            ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId)),
                            ModifiedOn = DateTime.Now,
                        };

                        _unitOfWorkEntityFramework.MenuCategoryCommand.Update(categories);
                        _unitOfWorkEntityFramework.Commit();

                        _notificationService.SuccessNotification("Message", "The Menu Category was Updated Successfully!");
                        return RedirectToAction("Index", "MenuCategory");
                    }
                    else if (_menuCategoryQueries.CheckCategoryNameExists(category.MenuCategoryName, category.RoleId))
                    {
                        ModelState.AddModelError("", "Menu Category Already Exists!");
                        category.ListofRoles = _roleQueries.ListofRoles();
                        return View(category);
                    }
                    else
                    {
                        MenuCategory categories = new MenuCategory()
                        {
                            RoleId = category.RoleId,
                            MenuCategoryName = category.MenuCategoryName,
                            Status = category.Status,
                            MenuCategoryId = category.MenuCategoryId,
                            ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId)),
                            ModifiedOn = DateTime.Now,
                        };

                        _unitOfWorkEntityFramework.MenuCategoryCommand.Update(categories);
                        _unitOfWorkEntityFramework.Commit();

                        _notificationService.SuccessNotification("Message", "The Menu Category was Updated Successfully!");
                        return RedirectToAction("Index", "MenuCategory");
                    }
                }

                category.ListofRoles = _roleQueries.ListofRoles();
                return View(category);
            }
            catch
            {
                throw;
            }
        }

        public JsonResult DeleteMenuCategory(RequestDeleteCategory requestCategory)
        {
            try
            {
                var data = _menuCategoryQueries.GetCategoryByMenuCategoryId(requestCategory.MenuCategoryId);
                _unitOfWorkEntityFramework.MenuCategoryCommand.Delete(data);
                var result = _unitOfWorkEntityFramework.Commit();

                return Json(new { Result = "success", Message = "Cannot Delete" });
            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }

     


    }
}
