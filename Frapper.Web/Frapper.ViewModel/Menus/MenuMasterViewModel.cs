using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frapper.ViewModel.Menus
{
    public class CreateMenuMasterViewModel
    {
        [Display(Name = "Area")]
        public string Area { get; set; }

        [Display(Name = "ControllerName")]
        [Required(ErrorMessage = "Enter Controller Name")]
        public string ControllerName { get; set; }


        [Display(Name = "ActionMethod")]
        [Required(ErrorMessage = "Enter Action Method")]
        public string ActionMethod { get; set; }

        [Display(Name = "MenuName")]
        [Required(ErrorMessage = "Enter Menu Name")]
        public string MenuName { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Required Role")]
        public int RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Display(Name = "MenuCategory")]
        [Required(ErrorMessage = "Required Menu Category")]
        public int MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class EditMenuMasterViewModel
    {
      
        public string Area { get; set; }
        public int MenuId { get; set; }

        [Required(ErrorMessage = "Enter Controller Name")]
        public string ControllerName { get; set; }

        [Required(ErrorMessage = "Enter ActionMethod")]
        public string ActionMethod { get; set; }

        [Required(ErrorMessage = "Enter Menu Name")]
        public string MenuName { get; set; }
        public bool Status { get; set; }

        [Required(ErrorMessage = "Required Role")]
        public int? RoleId { get; set; }
        public List<SelectListItem> ListofRoles { get; set; }

        [Required(ErrorMessage = "Required Menu Category")]
        public int? MenuCategoryId { get; set; }
        public List<SelectListItem> ListofMenuCategory { get; set; }
    }

    public class MenuMasterGrid
    {
        public int MenuId { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string MenuName { get; set; }
        public string Status { get; set; }
        public string MenuCategoryName { get; set; }
        public string RoleName { get; set; }
        public string Area { get; set; }
    }

    public class RequestDeleteMenuMaster
    {
        public int MenuId { get; set; }
    }
}
