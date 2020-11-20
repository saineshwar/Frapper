using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Repository;
using Frapper.Repository.EmailVerification.Queries;
using Frapper.Repository.Usermaster.Queries;
using Frapper.ViewModel.Login;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IVerificationQueries _verificationQueries;
        public ResetPasswordController(IUserMasterQueries userMasterQueries, IUnitOfWorkEntityFramework unitOfWorkEntityFramework, IVerificationQueries verificationQueries)
        {
            _userMasterQueries = userMasterQueries;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _verificationQueries = verificationQueries;
        }

        [HttpGet]
        public IActionResult Reset()
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reset(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userid = Convert.ToString(HttpContext.Session.GetString("VerificationUserId"));
                var getuserdetails = _userMasterQueries.GetUserDetailsbyUserId(Convert.ToInt64(userid));

                if (!string.Equals(resetPasswordViewModel.Password, resetPasswordViewModel.ConfirmPassword, StringComparison.Ordinal))
                {
                    TempData["Reset_Error_Message"] = "Password Does not Match";
                    return View(resetPasswordViewModel);
                }
                else
                {
                    var salt = GenerateRandomNumbers.GenerateRandomDigitCode(20);
                    var saltedpassword = HashHelper.CreateHashSHA512(resetPasswordViewModel.Password, salt);
                    _unitOfWorkEntityFramework.UserMasterCommand.UpdatePasswordandHistory(getuserdetails.UserId, saltedpassword, salt, "R");
                    var result = _unitOfWorkEntityFramework.Commit();
                    if (result)
                    {
                        var resetPasswordVerificationobj = _verificationQueries.GetResetGeneratedToken(getuserdetails.UserId);
                        _unitOfWorkEntityFramework.VerificationCommand.UpdateResetVerification(resetPasswordVerificationobj);
                        var updateresult = _unitOfWorkEntityFramework.Commit();

                        if (updateresult)
                        {
                            TempData["Reset_Success_Message"] = "Password Reset Successfully!";
                        }

                        return RedirectToAction("Login", "Portal");
                    }
                    else
                    {
                        TempData["Reset_Error_Message"] = "Something Went Wrong Please try again!";
                        return View(resetPasswordViewModel);
                    }

                }
            }

            return View(resetPasswordViewModel);
        }

        private void CheckIsPasswordAlreadyExists(ResetPasswordViewModel resetPasswordViewModel)
        {
            var salt = GenerateRandomNumbers.GenerateRandomDigitCode(20);
            var saltedpassword = HashHelper.CreateHashSHA512(resetPasswordViewModel.Password, salt);
        }
    }
}
