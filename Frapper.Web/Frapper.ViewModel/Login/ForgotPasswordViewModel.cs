using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Frapper.ViewModel.Login
{
    public class ForgotPasswordViewModel
    {
        [StringLength(30, ErrorMessage = "Not valid Username")]
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }
    }
}
