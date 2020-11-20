using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using Frapper.Common;
using Frapper.Entities.Usermaster;
using Frapper.Repository;
using Frapper.Repository.EmailVerification.Command;
using Frapper.Repository.Notices.Queries;
using Frapper.Repository.Usermaster.Queries;
using Frapper.Services.Messages;
using Frapper.ViewModel.Login;
using Frapper.ViewModel.Messages;
using Frapper.ViewModel.Usermaster;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Controllers
{
    [AllowAnonymous]
    public class PortalController : Controller
    {
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IUserTokensQueries _userTokensQueries;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private INoticeQueries _noticeQueries;
        public PortalController(IUserMasterQueries userMasterQueries, IUserTokensQueries userTokensQueries, IEmailSender emailSender, IUnitOfWorkEntityFramework unitOfWorkEntityFramework, INoticeQueries noticeQueries)
        {
            _userMasterQueries = userMasterQueries;
            _userTokensQueries = userTokensQueries;
            _emailSender = emailSender;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _noticeQueries = noticeQueries;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_noticeQueries.ShowNotice() != null)
            {
                var notice = _noticeQueries.ShowNotice();
                ViewBag.NoticeTitle = notice.NoticeTitle;
                ViewBag.Noticebody = notice.NoticeBody;
                ViewBag.NoticeCreatedOn = notice.CreatedOn;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter valid security code", CaptchaGeneratorLanguage = Language.English, CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_userMasterQueries.CheckUserExists(loginViewModel.Username))
                {
                    ModelState.AddModelError("", "Entered Username or Password is Invalid");
                }
                else
                {
                    var loggedInuserdetails = _userMasterQueries.GetCommonUserDetailsbyUserName(loginViewModel.Username);

                    if (loggedInuserdetails == null)
                    {
                        ModelState.AddModelError("", "Entered Username or Password is Invalid");
                        return View();
                    }

                    var usersalt = _userTokensQueries.GetUserSaltbyUserid(loggedInuserdetails.UserId);
                    if (usersalt == null)
                    {
                        ModelState.AddModelError("", "Entered Username or Password is Invalid");
                        return View();
                    }

                    if (loggedInuserdetails.RoleId == Convert.ToInt32(RolesHelper.Roles.User))
                    {
                        if (!_userMasterQueries.CheckIsAlreadyVerifiedRegistration(loggedInuserdetails.UserId))
                        {
                            ModelState.AddModelError("", "Email Verification Pending");
                            return View();
                        }
                    }

                    if (loggedInuserdetails.Status == false)
                    {
                        ModelState.AddModelError("", "Your Account is InActive Contact Administrator");
                        return View();
                    }

                    var generatedhash = HashHelper.CreateHashSHA512(loginViewModel.Password, usersalt.PasswordSalt);

                    if (string.Equals(loggedInuserdetails.PasswordHash, generatedhash, StringComparison.Ordinal))
                    {
                        SetAuthenticationCookie();
                        SetApplicationSession(loggedInuserdetails);

                        switch (loggedInuserdetails.RoleId)
                        {
                            case 1:
                                return RedirectToAction("Dashboard", "Dashboard", new { Area = "Administration" });
                            case 2:
                                return RedirectToAction("Dashboard", "UserDashboard");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Entered Username or Password is Invalid");
                    }

                    return View();
                }
            }

            return View();
        }


        private void SetAuthenticationCookie()
        {
            string strAuthToken = Guid.NewGuid().ToString();
            HttpContext.Session.SetString(AllSessionKeys.AuthenticationToken, strAuthToken);
            Response.Cookies.Append(AllSessionKeys.AuthenticationToken, strAuthToken);
        }

        private void SetApplicationSession(CommonUserDetailsViewModel commonUser)
        {
            HttpContext.Session.SetInt32(AllSessionKeys.RoleId, commonUser.RoleId);
            HttpContext.Session.SetString(AllSessionKeys.UserId, Convert.ToString(commonUser.UserId));
            HttpContext.Session.SetString(AllSessionKeys.UserName, Convert.ToString(commonUser.UserName));
            HttpContext.Session.SetString(AllSessionKeys.RoleName, Convert.ToString(commonUser.RoleName));
            if (commonUser.FirstName != null)
                HttpContext.Session.SetString(AllSessionKeys.FirstName, Convert.ToString(commonUser.FirstName));

            HttpContext.Session.SetString(AllSessionKeys.EmailId, Convert.ToString(commonUser.EmailId));
            HttpContext.Session.SetString(AllSessionKeys.MobileNo, Convert.ToString(commonUser.MobileNo));
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter valid security code", CaptchaGeneratorLanguage = Language.English, CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!_userMasterQueries.CheckUserExists(forgotPasswordViewModel.UserName))
            {
                ModelState.AddModelError("", "Entered Username or Password is Invalid");
            }
            else
            {
                var userdetails = _userMasterQueries.GetUserbyUserName(forgotPasswordViewModel.UserName);
                var token = HashHelper.CreateHashSHA256((GenerateRandomNumbers.GenerateRandomDigitCode(6)));
                var body = _emailSender.CreateVerificationEmail(userdetails, token);
                _unitOfWorkEntityFramework.VerificationCommand.SendResetVerificationToken(userdetails.UserId, token);
                _unitOfWorkEntityFramework.Commit();
                MessageTemplate messageTemplate = new MessageTemplate()
                {
                    ToAddress = userdetails.EmailId,
                    Subject = "Welcome to Frapper",
                    Body = body
                };

                _emailSender.SendMailusingSmtp(messageTemplate);

                TempData["ForgotPasswordMessage"] = "An email has been sent to the address you have registered." +
                                                    "Please follow the link in the email to complete your password reset request";

                return RedirectToAction("ForgotPassword", "Portal");
            }

            return View();
        }


        [HttpPost]
        public IActionResult Logout()
        {
            try
            {
                CookieOptions option = new CookieOptions();

                if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
                {
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
                }

                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Portal");
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
