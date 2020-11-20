using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Frapper.Common;
using Frapper.Entities.Usermaster;
using Frapper.Repository;
using Frapper.Repository.ProfileImage.Queries;
using Frapper.Repository.Rolemasters.Queries;
using Frapper.Repository.Usermaster.Queries;
using Frapper.ViewModel.Rolemasters;
using Frapper.ViewModel.Usermaster;
using Frapper.Web.Extensions;
using Frapper.Web.Filters;
using Frapper.Web.Helpers;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class UserController : Controller
    {
        private readonly IRoleQueries _roleQueries;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IUserMasterQueries _userMasterQueries;
        private readonly INotificationService _notificationService;
        private readonly IAssignedRolesQueries _assignedRolesQueries;
       

        public UserController(
            IRoleQueries roleQueries,
            IMapper mapper,
            IUnitOfWorkEntityFramework iUnitOfWorkEntityFramework,
            IUserMasterQueries userMasterQueries,
            INotificationService notificationService,
            IAssignedRolesQueries assignedRolesQueries, IProfileImageQueries profileImageQueries)
        {
            _roleQueries = roleQueries;
            _mapper = mapper;
            _unitOfWorkEntityFramework = iUnitOfWorkEntityFramework;
            _userMasterQueries = userMasterQueries;
            _notificationService = notificationService;
            _assignedRolesQueries = assignedRolesQueries;
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateUserViewModel createUserView = new CreateUserViewModel()
            {
                ListRole = _roleQueries.ListofRoles()
            };
            return View(createUserView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserViewModel createModel)
        {

            createModel.ListRole = _roleQueries.ListofRoles();

            if (!ModelState.IsValid)
            {
                return View(createModel);
            }

            if (_userMasterQueries.CheckUserExists(createModel.UserName))
            {
                ModelState.AddModelError("", "Entered Username Already Exists");
                return View(createModel);
            }

            if (_userMasterQueries.CheckEmailExists(createModel.EmailId))
            {
                ModelState.AddModelError("", "Entered EmailId Already Exists");
                return View(createModel);
            }

            if (_userMasterQueries.CheckMobileNoExists(createModel.MobileNo))
            {
                ModelState.AddModelError("", "Entered MobileNo Already Exists");
                return View(createModel);
            }

            if (!string.Equals(createModel.Password, createModel.ConfirmPassword,
                StringComparison.Ordinal))
            {
                TempData["Registered_Error_Message"] = "Password Does not Match";
                return View(createModel);
            }

            var salt = GenerateRandomNumbers.GenerateRandomDigitCode(20);
            var saltedpassword = HashHelper.CreateHashSHA512(createModel.Password, salt);

            var userMappedobject = _mapper.Map<UserMaster>(createModel);
            userMappedobject.Status = true;
            userMappedobject.CreatedOn = DateTime.Now;
            userMappedobject.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));
            userMappedobject.PasswordHash = saltedpassword;

            AssignedRoles assignedRoles = new AssignedRoles()
            {
                AssignedRoleId = 0,
                CreateDate = DateTime.Now,
                Status = true,
                RoleId = createModel.RoleId,
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
                _notificationService.SuccessNotification("Message", "User Registered Successfully !");
                return RedirectToAction("Index");
            }
            else
            {
                _notificationService.DangerNotification("Message", "Something went wrong Please Try Once Again !");
                return View(createModel);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GridAllUser()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                var usersdata = _userMasterQueries.ShowAllUsers(sortColumn, sortColumnDirection, searchValue);
                recordsTotal = usersdata.Count();
                var data = usersdata.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "User");
                }
                var userMaster = _userMasterQueries.GetUserForEditByUserId(id);
                userMaster.ListRole = _roleQueries.ListofRoles();
                return View(userMaster);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditUserViewModel editUser)
        {
            editUser.ListRole = _roleQueries.ListofRoles();

            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            var userMaster = _userMasterQueries.GetUserDetailsbyUserId(editUser.UserId);

            if (editUser.MobileNo != userMaster.MobileNo)
            {
                ModelState.AddModelError("", "Entered MobileNo Already Exists");
                return View(editUser);
            }

            if (editUser.EmailId != userMaster.EmailId)
            {
                ModelState.AddModelError("", "Entered EmailId Already Exists");
                return View(editUser);
            }

            userMaster.FirstName = editUser.FirstName;
            userMaster.LastName = editUser.LastName;
            userMaster.MobileNo = editUser.MobileNo;
            userMaster.EmailId = editUser.EmailId;
            userMaster.Gender = editUser.Gender;
            userMaster.ModifiedOn = DateTime.Now;
            userMaster.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));

            var assignedroles = _assignedRolesQueries.GetAssignedRolesDetailsbyUserId(userMaster.UserId);
            assignedroles.RoleId = editUser.RoleId;

            _unitOfWorkEntityFramework.UserMasterCommand.Update(userMaster);
            _unitOfWorkEntityFramework.AssignedRolesCommand.Update(assignedroles);
            var result = _unitOfWorkEntityFramework.Commit();

            if (result)
            {
                _notificationService.SuccessNotification("Message", "User Details Updated Successfully !");
                return RedirectToAction("Index");
            }
            else
            {
                _notificationService.DangerNotification("Message", "Something went wrong Please Try Once Again !");
                return View(editUser);
            }
        }

        public JsonResult ChangeUserStatus(RequestStatus requestStatus)
        {
            try
            {
                var userMaster = _userMasterQueries.GetUserDetailsbyUserId(requestStatus.UserId);
                userMaster.Status = requestStatus.Status;
                _unitOfWorkEntityFramework.UserMasterCommand.ChangeUserStatus(userMaster);
                var result = _unitOfWorkEntityFramework.Commit();

                if (result)
                {
                    _notificationService.SuccessNotification("Message", "Changed User Status successfully!");
                    return Json(new { Result = "success" });
                }
                else
                {
                    return Json(new { Result = "failed", Message = "Cannot Delete" });
                }

            }
            catch (Exception)
            {
                return Json(new { Result = "failed", Message = "Cannot Delete" });
            }
        }


     
    }
}
