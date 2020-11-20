using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Frapper.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Exceptional;

namespace Frapper.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            ViewBag.ErrorId = "404";
            CookieOptions option = new CookieOptions();

            if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
            }

            HttpContext.Session.Remove(AllSessionKeys.RoleId);
            HttpContext.Session.Clear();
            return View();
        }


        [Route("Error/{StatusCode}")]
        public IActionResult Error(int statusCode)
        {
            bool isAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { responseText = "Ajax Error" });
            }

            if (statusCode == 404)
            {
                ViewBag.ErrorId = "404";
            }

            if (statusCode == 500)
            {
                ViewBag.ErrorId = "500";
            }

            CookieOptions option = new CookieOptions();

            if (Request.Cookies[AllSessionKeys.AuthenticationToken] != null)
            {
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append(AllSessionKeys.AuthenticationToken, "", option);
            }

            HttpContext.Session.Remove(AllSessionKeys.RoleId);
            HttpContext.Session.Clear();


            return View();
        }


    }
}
