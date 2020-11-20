using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using Frapper.Common;
using Frapper.Entities.Usermaster;
using Frapper.Repository;
using Frapper.Repository.Usermaster.Queries;
using Frapper.Services.Messages;
using Frapper.ViewModel.Messages;
using Frapper.ViewModel.Registration;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        public RegistrationController(IUnitOfWorkEntityFramework iUnitOfWork, IUserMasterQueries userMasterQueries, IMapper mapper, IEmailSender emailSender)
        {
            _unitOfWorkEntityFramework = iUnitOfWork;
            _userMasterQueries = userMasterQueries;
            _mapper = mapper;
            _emailSender = emailSender;
        }


        [HttpGet]
        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter valid security code", CaptchaGeneratorLanguage = Language.English, CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult Form(RegistrationViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_userMasterQueries.CheckUserExists(registerViewModel.UserName))
                {
                    ModelState.AddModelError("", "Entered Username Already Exists");
                    return View(registerViewModel);
                }

                if (_userMasterQueries.CheckUserExists(registerViewModel.EmailId))
                {
                    ModelState.AddModelError("", "Entered EmailId Already Exists");
                    return View(registerViewModel);
                }

                if (_userMasterQueries.CheckUserExists(registerViewModel.MobileNo))
                {
                    ModelState.AddModelError("", "Entered Phoneno Already Exists");
                    return View(registerViewModel);
                }

                if (!string.Equals(registerViewModel.Password, registerViewModel.ConfirmPassword,
                    StringComparison.Ordinal))
                {
                    TempData["Registered_Error_Message"] = "Password Does not Match";
                    return View(registerViewModel);
                }

                var salt = GenerateRandomNumbers.GenerateRandomDigitCode(20);
                var saltedpassword = HashHelper.CreateHashSHA512(registerViewModel.Password, salt);


                var userMappedobject = _mapper.Map<UserMaster>(registerViewModel);
                userMappedobject.Status = true;
                userMappedobject.CreatedOn = DateTime.Now;
                userMappedobject.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));
                userMappedobject.PasswordHash = saltedpassword;

                AssignedRoles assignedRoles = new AssignedRoles()
                {
                    AssignedRoleId = 0,
                    CreateDate = DateTime.Now,
                    Status = true,
                    RoleId = Convert.ToInt32(RolesHelper.Roles.User),
                    UserMaster = userMappedobject
                };

                UserTokens userTokens = new UserTokens()
                {
                    UserMaster = userMappedobject,
                    CreatedOn = DateTime.Now,
                    HashId = 0,
                    PasswordSalt = salt
                };

                _unitOfWorkEntityFramework.UserMasterCommand.Add(userMappedobject);
                _unitOfWorkEntityFramework.AssignedRolesCommand.Add(assignedRoles);
                _unitOfWorkEntityFramework.UserTokensCommand.Add(userTokens);
                var result = _unitOfWorkEntityFramework.Commit();

                if (result)
                {
                    var userdetails = _userMasterQueries.GetUserbyUserName(registerViewModel.UserName);
                    var token = HashHelper.CreateHashSHA256((GenerateRandomNumbers.GenerateRandomDigitCode(6)));
                    var body = _emailSender.CreateRegistrationVerificationEmail(userdetails, token);

                    _unitOfWorkEntityFramework.VerificationCommand.SendRegistrationVerificationToken(userdetails.UserId, token);
                    _unitOfWorkEntityFramework.Commit();
                    MessageTemplate messageTemplate = new MessageTemplate()
                    {
                        ToAddress = userdetails.EmailId,
                        Subject = "Welcome to Frapper",
                        Body = body
                    };

                    _emailSender.SendMailusingSmtp(messageTemplate);

                    TempData["Registered_Success_Message"] = "User Registered Successfully";
                }
                else
                {
                    TempData["Registered_Error_Message"] = "Error While Registrating User Successfully";
                }
            }

            return RedirectToAction("Form", "Registration");
        }
    }
}
