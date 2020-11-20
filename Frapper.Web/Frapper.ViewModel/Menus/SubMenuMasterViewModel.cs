using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.ViewModel.Menus
{
    public class CreateSubMenuMasterViewModel
    {
        [Display(Name = "Area")]
        public string Area { get; set; }

        [Display(Name = "ControllerName")]
        [Required(ErrorMessage = "Enter ControllerName")]
        public string ControllerName { get; set; }

        [Display(Name = "ActionMethod")]
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Display(Name = "SubMenuName")]
        [Required(ErrorMessage = "Enter SubMenuName")]
        public string SubMenuName { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }

        [Display(Name = "MenuName")]
        [Required(ErrorMessage = "Choose Menu")]
        public int MenuId { get; set; }
        public List<SelectListItem> ListofMenus { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Select Role")]
        public int? RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Display(Name = "MenuCategory")]
        [Required(ErrorMessage = "Select Menu Category")]
        public int MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class EditSubMenuMasterViewModel
    {
        [Display(Name = "Area")]
        public string Area { get; set; }
        public int SubMenuId { get; set; }

        [Display(Name = "ControllerName")]
        [Required(ErrorMessage = "Enter ControllerName")]
        public string ControllerName { get; set; }

        [Display(Name = "ActionMethod")]
        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Display(Name = "SubMenuName")]
        [Required(ErrorMessage = "Enter SubMenuName")]
        public string SubMenuName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        [Display(Name = "Status")]
        public bool Status { get; set; }
        public DateTime? CreateDate { get; set; }

        [Display(Name = "MenuName")]
        [Required(ErrorMessage = "Choose Menu")]
        public int MenuId { get; set; }
        public List<SelectListItem> ListofMenus { get; set; }


        [Display(Name = "Role")]
        [Required(ErrorMessage = "Select Role")]
        public int? RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Display(Name = "MenuCategory")]
        [Required(ErrorMessage = "Select Menu Category")]
        public int? MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class SubMenuMasterViewModel
    {
        public int SubMenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string Area { get; set; }
        public string SubMenuName { get; set; }
        public string MenuName { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? MenuId { get; set; }
        public int? RoleId { get; set; }
        public string MenuCategoryName { get; set; }
        public string RoleName { get; set; }
    }

    public class RequestDeleteSubMenu
    {
        public int? SubMenuId { get; set; }
    }

    public class RequestMenus
    {
        public int? RoleID { get; set; }
        public int? CategoryID { get; set; }
    }
}
