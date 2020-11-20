using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Frapper.ViewModel.Usermaster
{
    public class CreateUserViewModel
    {
        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter FirstName")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter LastName")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "EmailId")]
        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string EmailId { get; set; }

        [Display(Name = "MobileNo")]
        [Required(ErrorMessage = "Mobile-no Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        public string MobileNo { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Display(Name = "Status")]
        public bool? Status { get; set; }

        [Display(Name = "Password")]
        [MinLength(7, ErrorMessage = "Minimum Password must be 7 in characters")]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Choose Role")]
        public short RoleId { get; set; }
        public List<SelectListItem> ListRole { get; set; }
    }

    public class EditUserViewModel
    {
        public long UserId { get; set; }

        [Display(Name = "UserName")]
        [MinLength(6, ErrorMessage = "Minimum Username must be 6 in characters")]
        [Required(ErrorMessage = "Username Required")]
        public string UserName { get; set; }

        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }

        [Display(Name = "EmailId")]
        [Required(ErrorMessage = "EmailID Required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address")]
        public string EmailId { get; set; }

        [Display(Name = "MobileNo")]
        [Required(ErrorMessage = "Mobile-no Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong Mobile-no")]
        public string MobileNo { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender Required")]
        public string Gender { get; set; }

        [Display(Name = "Status")]
        public bool? Status { get; set; }

        [Display(Name = "Role")]
        [Required(ErrorMessage = "Choose Role")]
        public int RoleId { get; set; }
        public List<SelectListItem> ListRole { get; set; }
    }

    public class CommonUserDetailsViewModel
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public string PasswordHash { get; set; }
    }
}