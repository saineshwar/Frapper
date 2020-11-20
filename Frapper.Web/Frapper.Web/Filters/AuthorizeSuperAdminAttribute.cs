using System;
using Frapper.Common;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Frapper.Web.Filters
{
    public class AuthorizeSuperAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId))))
            {
                var roleValue = Convert.ToInt32(context.HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
                if (roleValue != Convert.ToInt32(RolesHelper.Roles.SuperAdmin))
                {
                   

                    if (context.Controller is Controller controller)
                    {
                        controller.ViewData["ErrorMessage"] = "Invalid User";
                        controller.HttpContext.Session.Clear();
                    }

                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {{ "Controller", "Error" },
                            { "Action", "Error" } });
                }

            }
            else
            {
               

                if (context.Controller is Controller controller)
                {
                    controller.ViewData["ErrorMessage"] = "You Session has been Expired";
                    controller.HttpContext.Session.Clear();
                }

                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {{ "Controller", "Error" },
                        { "Action", "Error" } });

            }
        }
    }
}