using System.ComponentModel.DataAnnotations;

namespace Frapper.ViewModel.Rolemasters
{
    public class RoleMasterViewModel
    {
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }
    }

    public class EditRoleMasterViewModel
    {
        [Required(ErrorMessage = "Enter RoleName")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Choose Status")]
        public bool Status { get; set; }

        public int RoleId { get; set; }
    }

    public class RequestDeleteRole
    {
        public int RoleId { get; set; }
    }

    public class RoleMasterGrid
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Status { get; set; }
    }

}