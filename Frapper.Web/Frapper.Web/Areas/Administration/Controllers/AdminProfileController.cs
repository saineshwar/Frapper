using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Common;
using Frapper.Repository.ProfileImage.Queries;
using Frapper.Web.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Frapper.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [AuthorizeSuperAdmin]
    public class AdminProfileController : Controller
    {
        private readonly IProfileImageQueries _profileImageQueries;
        public AdminProfileController(IProfileImageQueries profileImageQueries)
        {
            _profileImageQueries = profileImageQueries;
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
