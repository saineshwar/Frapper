using FluentValidation;
using Frapper.ViewModel.Customers;

namespace Frapper.Web.Validators
{
    public class CustomerValidator : AbstractValidator<CustomersViewModel>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("Please Enter 'FirstName'")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Enter Valid First Name")
                .MinimumLength(3).WithMessage("minimum length of 3 characters are allowed")
                .MaximumLength(50).WithMessage("maximum length of 50 charaters is allowed");

            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Please Enter 'LastName'")
                .Matches(@"^[a-zA-Z ]*$").WithMessage("Enter Valid Last Name")
                .MinimumLength(3).WithMessage("minimum length of 3 characters are allowed")
                .MaximumLength(50).WithMessage("maximum length of 50 charaters is allowed");

            RuleFor(x => x.MobileNo).NotEmpty()
                .WithMessage("Please Enter 'MobileNo'")
                .Matches("^[7-9][0-9]{9}$").WithMessage("Mobile No. code should start with 7,8,9");

            RuleFor(x => x.LandlineNo).NotEmpty()
                .WithMessage("Please Enter 'LandlineNo'")
                .Matches(@"^(\d{10})$").WithMessage("InValid LandlineNo");

            RuleFor(x => x.EmailId)
                .NotEmpty().WithMessage("Please Enter 'EmailId'")
                .EmailAddress();

            RuleFor(reg => reg.Street).NotEmpty()
                .WithMessage("Please Enter 'Street'");

            RuleFor(reg => reg.City).NotEmpty()
                .WithMessage("Please Enter 'City'");

            RuleFor(reg => reg.State).NotEmpty()
                .WithMessage("Please Enter 'State'");

            RuleFor(x => x.Pincode).NotEmpty()
                .MinimumLength(6).WithMessage("minimum length of 6 characters are allowed")
                .MaximumLength(6).WithMessage("maximum length of 6 charaters is allowed")
                .WithMessage("Please Enter 'Pincode'");

        }
    }
}
