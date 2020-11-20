using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Frapper.Web.Filters
{
    public class AuthorizeResetPasswordAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetString("ActiveVerification"))))
            {
                string activeVerificationvalue = (string)context.HttpContext.Session.GetString("ActiveVerification");

                if ("1" != activeVerificationvalue)
                {
                    if (context.Controller is Controller controller)
                    {
                        controller.TempData["ErrorMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                    }

                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {{ "Controller", "Error" },
                            { "Action", "Error" } });
                }
            }
            else
            {
                ViewResult result = new ViewResult();
                result.ViewName = "Error";

                if (context.Controller is Controller controller)
                {
                    controller.TempData["ErrorMessage"] = "Sorry Verification Failed Please request a new Verification link!";
                }

                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {{ "Controller", "Error" },
                        { "Action", "Error" } });

            }
        }
    }
}