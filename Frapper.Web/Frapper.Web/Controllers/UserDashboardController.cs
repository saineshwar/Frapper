using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Common;
using Frapper.Repository;
using Frapper.Repository.EmailVerification.Queries;
using Frapper.Repository.Usermaster.Queries;
using Frapper.Services.PdfService;
using Frapper.ViewModel.Login;
using Frapper.Web.Filters;
using Frapper.Web.Helpers;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Controllers
{
 
    [AuthorizeUser]
    public class UserDashboardController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IUserTokensQueries _userTokensQueries;
        private readonly INotificationService _notificationService;
        private readonly IVerificationQueries _verificationQueries;
        private readonly IPdfGenerator _pdfGenerator;
        public UserDashboardController(IUserMasterQueries userMasterQueries,
            IUserTokensQueries userTokensQueries,
            IUnitOfWorkEntityFramework unitOfWorkEntityFramework,
            INotificationService notificationService,
            IVerificationQueries verificationQueries,
            IPdfGenerator pdfGenerator)
        {
            _userMasterQueries = userMasterQueries;
            _userTokensQueries = userTokensQueries;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _notificationService = notificationService;
            _verificationQueries = verificationQueries;
            _pdfGenerator = pdfGenerator;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


        // GET: User
        public IActionResult Changepassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Changepassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userid = Convert.ToString(HttpContext.Session.GetString(AllSessionKeys.UserId));
                var getuserdetails = _userMasterQueries.GetUserDetailsbyUserId(Convert.ToInt64(userid));
                var usersalt = _userTokensQueries.GetUserSaltbyUserid(getuserdetails.UserId);
                var generatehash = HashHelper.CreateHashSHA512(changePasswordViewModel.CurrentPassword, usersalt.PasswordSalt);

                if (changePasswordViewModel.CurrentPassword == changePasswordViewModel.Password)
                {
                    ModelState.AddModelError("", @"New Password Cannot be same as Old Password");
                    return View(changePasswordViewModel);
                }

                if (!string.Equals(getuserdetails.PasswordHash, generatehash, StringComparison.Ordinal))
                {
                    ModelState.AddModelError("", "Current Password Entered is InValid");
                    return View(changePasswordViewModel);
                }

                if (!string.Equals(changePasswordViewModel.Password, changePasswordViewModel.ConfirmPassword, StringComparison.Ordinal))
                {
                    _notificationService.DangerNotification("Message", "Password Does not Match!");
                    return View(changePasswordViewModel);
                }
                else
                {
                    var salt = GenerateRandomNumbers.GenerateRandomDigitCode(20);
                    var saltedpassword = HashHelper.CreateHashSHA512(changePasswordViewModel.Password, salt);
                    _unitOfWorkEntityFramework.UserMasterCommand.UpdatePasswordandHistory(getuserdetails.UserId, saltedpassword, salt, "C");
                    var result = _unitOfWorkEntityFramework.Commit();

                    if (result)
                    {
                        _notificationService.SuccessNotification("Message", "Your Password Changed Successfully!");
                        var registerVerificationobj = _verificationQueries.GetRegistrationGeneratedToken(getuserdetails.UserId);
                        _unitOfWorkEntityFramework.VerificationCommand.UpdateRegisterVerification(registerVerificationobj);
                        return RedirectToAction("Changepassword", "UserDashboard");
                    }
                    else
                    {
                        _notificationService.DangerNotification("Message", "Something Went Wrong Please try again!");
                        return View(changePasswordViewModel);
                    }
                }
            }

            return View(changePasswordViewModel);
        }

        [HttpGet]
        public IActionResult Invoice()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DownloadInvoice()
        {
            return _pdfGenerator.DownloadInvoicepdf();
        }
    }
}
