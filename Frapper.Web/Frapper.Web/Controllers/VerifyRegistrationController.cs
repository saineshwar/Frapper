using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Repository;
using Frapper.Repository.EmailVerification.Queries;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Controllers
{
    public class VerifyRegistrationController : Controller
    {
        private readonly IVerificationQueries _verificationQueries;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;

        public VerifyRegistrationController(IVerificationQueries verificationQueries, IUnitOfWorkEntityFramework unitOfWorkEntityFramework)
        {
            _verificationQueries = verificationQueries;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
        }

        [HttpGet]
        public IActionResult Verify(string key, string hashtoken)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(hashtoken))
                {
                    var arrayVakue = SecurityTokenHelper.SplitToken(key);
                    if (arrayVakue != null)
                    {
                        // arrayVakue[1] "UserId"
                        var userId = Convert.ToInt64(arrayVakue[1]);
                        var rvModel = _verificationQueries.GetRegistrationGeneratedToken(userId);
                        if (rvModel != null)
                        {
                            var result = SecurityTokenHelper.IsTokenValid(arrayVakue, hashtoken, rvModel.GeneratedToken);

                            if (result == 1)
                            {
                                TempData["TokenErrorMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (result == 2)
                            {
                                TempData["TokenErrorMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (result == 0)
                            {
                                if (_verificationQueries.CheckIsAlreadyVerifiedRegistration(Convert.ToInt64(arrayVakue[1])))
                                {
                                    TempData["TokenErrorMessage"] = "Sorry Link Expired";
                                    return RedirectToAction("Login", "Portal");
                                }

                                HttpContext.Session.SetString("VerificationUserId", arrayVakue[1]);

                                var resetPasswordVerificationobj = _verificationQueries.GetRegistrationGeneratedToken(userId);
                                _unitOfWorkEntityFramework.VerificationCommand.UpdateRegisterVerification(resetPasswordVerificationobj);
                                var updateresult = _unitOfWorkEntityFramework.Commit();

                                if (updateresult)
                                {
                                    TempData["Verify"] = "Done";
                                    return RedirectToAction("Completed", "VerifyRegistration");
                                }
                                else
                                {
                                    TempData["TokenErrorMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                                    return RedirectToAction("Login", "Portal");
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                return RedirectToAction("Login", "Portal");
            }

            TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
            return RedirectToAction("Login", "Portal");
        }


        [HttpGet]
        public IActionResult Completed()
        {
            if (Convert.ToString(TempData["Verify"]) == "Done")
            {
                TempData["RegistrationCompleted"] = "Registration Process Completed. Now you can Login and Access Account.";
                return View();
            }
            else
            {
                TempData["TokenMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                return RedirectToAction("Login", "Portal");
            }

        }
    }
}
