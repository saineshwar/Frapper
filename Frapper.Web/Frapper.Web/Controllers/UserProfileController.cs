using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Common;
using Frapper.Entities.ProfileImage;
using Frapper.Repository;
using Frapper.Repository.ProfileImage.Queries;
using Frapper.Repository.Usermaster.Queries;
using Frapper.ViewModel.Usermaster;
using Frapper.Web.Filters;
using Frapper.Web.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Frapper.Web.Controllers
{
    [AuthorizeUser]
    public class UserProfileController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWorkEntityFramework _unitOfWorkEntityFramework;
        private readonly IProfileImageQueries _profileImageQueries;
        private readonly IUserMasterQueries _userMasterQueries;
        public UserProfileController(INotificationService notificationService,
            IUnitOfWorkEntityFramework unitOfWorkEntityFramework, IProfileImageQueries profileImageQueries, IUserMasterQueries userMasterQueries)
        {
            _notificationService = notificationService;
            _unitOfWorkEntityFramework = unitOfWorkEntityFramework;
            _profileImageQueries = profileImageQueries;
            _userMasterQueries = userMasterQueries;
        }

        [HttpGet]
        public IActionResult MyProfile()
        {
            try
            {
                var result = _profileImageQueries.CheckProfileImageExists(Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));
                if (result)
                {
                    var profileImage = _profileImageQueries.GetProfileImageByProfileImageId(Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));

                    ViewBag.UserProfileimagepath = profileImage.Medium;
                }
                else
                {
                    ViewBag.UserProfileimagepath = "/img/user.png";
                }

                var userprofile =
                    _userMasterQueries.GetProfileForEditByUserId(
                        Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));
                return View(userprofile);

            }
            catch (Exception)
            {
                throw;
            }


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MyProfile(EditUserProfileViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                var userprofile =
                    _userMasterQueries.GetProfileForEditByUserId(
                        Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));

                if (!string.Equals(userprofile.EmailId, editUser.EmailId, StringComparison.OrdinalIgnoreCase))
                {
                    if (_userMasterQueries.CheckEmailExists(editUser.EmailId))
                    {
                        ModelState.AddModelError("", "Entered EmailId Already Exists");
                        return View(editUser);
                    }
                }

                if (!string.Equals(userprofile.MobileNo, editUser.MobileNo, StringComparison.OrdinalIgnoreCase))
                {
                    if (_userMasterQueries.CheckMobileNoExists(editUser.MobileNo))
                    {
                        ModelState.AddModelError("", "Entered MobileNo Already Exists");
                        return View(editUser);
                    }
                }

                var userMaster = _userMasterQueries.GetUserDetailsbyUserId(editUser.UserId);
                userMaster.ModifiedOn = DateTime.Now;
                userMaster.ModifiedBy = Convert.ToInt32(HttpContext.Session.GetString(AllSessionKeys.UserId));
                userMaster.LastName = editUser.LastName;
                userMaster.FirstName = editUser.FirstName;
                userMaster.MobileNo = editUser.MobileNo;
                userMaster.EmailId = editUser.EmailId;
                userMaster.Gender = editUser.Gender;

                _unitOfWorkEntityFramework.UserMasterCommand.Update(userMaster);
                var result = _unitOfWorkEntityFramework.Commit();

                if (result)
                {
                    _notificationService.SuccessNotification("Message", "User Details Updated Successfully !");
                    return RedirectToAction("MyProfile");
                }
                else
                {
                    _notificationService.DangerNotification("Message", "Something went wrong Please Try Once Again !");
                    return View(editUser);
                }

            }

            return View(editUser);
        }


        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(string filename, IFormFile blob)
        {
            try
            {
                string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    var newfileName200 = GenerateFileName("Photo_200_200_", systemFileExtenstion);
                    var filepath200 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileImages")).Root + $@"\{newfileName200}";
                    image.Mutate(x => x.Resize(200, 200));
                    image.Save(filepath200);

                    image.Mutate(x => x.Resize(180, 180));
                    var newfileName180 = GenerateFileName("Photo_180_180_", systemFileExtenstion);
                    var filepath180 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileImages")).Root + $@"\{newfileName180}";
                    image.Save(filepath180);

                    var newfileName32 = GenerateFileName("Photo_32_32_", systemFileExtenstion);
                    var filepath32 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileImages")).Root + $@"\{newfileName32}";
                    image.Mutate(x => x.Resize(32, 32));
                    image.Save(filepath32);

                    if (_profileImageQueries.CheckProfileImageExists(Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId))))
                    {
                        var getprofileImage =
                            _profileImageQueries.GetProfileImageByProfileImageId(
                                Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));

                        if (getprofileImage != null)
                        {
                            getprofileImage.ImageName = "ProfileImage";
                            getprofileImage.ImageType = systemFileExtenstion;
                            getprofileImage.ModifiedOn = DateTime.Now;
                            getprofileImage.Tiny = $"/ProfileImages/{newfileName32}";
                            getprofileImage.Small = $"/ProfileImages/{newfileName180}";
                            getprofileImage.Medium = $"/ProfileImages/{newfileName200}";
                            getprofileImage.Status = true;
                            getprofileImage.ModifiedBy =
                                Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId));


                            _unitOfWorkEntityFramework.ProfileImageCommand.Update(getprofileImage);
                            _unitOfWorkEntityFramework.Commit();
                        }
                    }
                    else
                    {
                        ProfileImageProperty profileImageProperty = new ProfileImageProperty()
                        {
                            ProfileImageId = 0,
                            ImageName = "ProfileImage",
                            ImageType = systemFileExtenstion,
                            CreatedOn = DateTime.Now,
                            Tiny = $"/ProfileImages/{newfileName32}",
                            Small = $"/ProfileImages/{newfileName180}",
                            Medium = $"/ProfileImages/{newfileName200}",
                            Status = true,
                            CreatedBy = Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId))
                        };

                        _unitOfWorkEntityFramework.ProfileImageCommand.Add(profileImageProperty);
                        _unitOfWorkEntityFramework.Commit();
                    }

                }

                _notificationService.SuccessNotification("Message", "Your Profile Photo Update Successfully!");

                return Json(new { Message = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Message = "ERROR" });
            }
        }
        private string GenerateFileName(string fileTypeName, string fileextenstion)
        {
            if (fileTypeName == null) throw new ArgumentNullException(nameof(fileTypeName));
            if (fileextenstion == null) throw new ArgumentNullException(nameof(fileextenstion));
            return $"{fileTypeName}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{fileextenstion}";
        }

        [HttpGet]
        public IActionResult Capture()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Capture(string filename, IFormFile blob)
        {
            try
            {
                if (blob == null)
                {
                    return Json(new { Result = "ERROR", Message = "Error in Capture Image Try Again" });
                }

                string extension = System.IO.Path.GetExtension(filename);
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

                    var newfileName200 = GenerateFileName("Photo_200_200_", systemFileExtenstion);
                    var filepath200 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProfileImages")).Root + $@"\{newfileName200}";
                    image.Mutate(x => x.Resize(200, 200));
                    image.Save(filepath200);
                }
                _notificationService.SuccessNotification("Message", "Your Photo Capture Image Successfully!");
                return Json(new { Result = "OK", Message = "Capture Image Successfully" });
            }
            catch (Exception)
            {
                return Json(new { Result = "ERROR", Message = "Error in Capture Image Try Again" });
            }
        }

        [HttpPost]
        public IActionResult CheckIsProfileImageExists()
        {
            try
            {
                var result = _profileImageQueries.CheckProfileImageExists(Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));
                if (result)
                {
                    var profileImage = _profileImageQueries.GetProfileImageByProfileImageId(Convert.ToInt64(HttpContext.Session.GetString(AllSessionKeys.UserId)));

                    return Json(new { result = true, imagepath = profileImage.Medium });
                }
                else
                {
                    return Json(new { result = false, imagepath = "/img/user.png" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
