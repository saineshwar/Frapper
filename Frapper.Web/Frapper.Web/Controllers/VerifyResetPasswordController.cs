using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Frapper.Repository.EmailVerification.Queries;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Controllers
{
    public class VerifyResetPasswordController : Controller
    {
        private readonly IVerificationQueries _verificationQueries;
        public VerifyResetPasswordController(IVerificationQueries verificationQueries)
        {
            _verificationQueries = verificationQueries;
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
                        var rvModel = _verificationQueries.GetResetGeneratedToken(userId);
                        if (rvModel != null)
                        {
                            var result = SecurityTokenHelper.IsTokenValid(arrayVakue, hashtoken, rvModel.GeneratedToken);

                            if (result == 1)
                            {
                                TempData["TokenMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (result == 2)
                            {
                                TempData["TokenMessage"] = "Sorry Verification Link Expired Please request a new Verification link!";
                                return RedirectToAction("Login", "Portal");
                            }

                            if (result == 0)
                            {
                                HttpContext.Session.SetString("VerificationUserId", arrayVakue[1]);
                                HttpContext.Session.SetString("ActiveVerification", "1");
                                return RedirectToAction("Reset", "ResetPassword");
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
    }
}
