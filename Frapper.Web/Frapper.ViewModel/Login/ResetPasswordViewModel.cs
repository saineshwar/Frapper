using System.ComponentModel.DataAnnotations;

namespace Frapper.ViewModel.Login
{
    public class ResetPasswordViewModel
    {
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("Password", ErrorMessage = "Enter Valid Password")]
        public string ConfirmPassword { get; set; }
    }
}