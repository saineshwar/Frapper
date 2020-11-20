using System.ComponentModel.DataAnnotations;

namespace Frapper.ViewModel.Customers
{
    public class CustomersViewModel
    {
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "MobileNo")]
        public string MobileNo { get; set; }

        [Display(Name = "LandlineNo")]
        public string LandlineNo { get; set; }

        [Display(Name = "EmailId")]
        public string EmailId { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Pincode")]
        public string Pincode { get; set; }
    }
}