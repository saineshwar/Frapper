using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Common;
using Frapper.Repository;
using Frapper.Repository.Audit.Command;
using Frapper.ViewModel.Audit;
using Frapper.Web.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Frapper.Web.Filters
{
    public class AuditFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuditCommand _auditCommand;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditFilterAttribute(IAuditCommand auditCommand, IHttpContextAccessor httpContextAccessor)
        {
            _auditCommand = auditCommand;
            _httpContextAccessor = httpContextAccessor;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            AuditTbModel objaudit = new AuditTbModel();

            var controllerName = ((ControllerBase)context.Controller)
                .ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)context.Controller)
                .ControllerContext.ActionDescriptor.ActionName;
            var area = ((ControllerBase)context.Controller)
                .ControllerContext.ActionDescriptor.RouteValues["area"];

            var request = context.HttpContext.Request;

            if (!string.IsNullOrEmpty(Convert.ToString(context.HttpContext.Session.GetString(AllSessionKeys.UserId))))
            {
                var userValue = Convert.ToInt32(context.HttpContext.Session.GetString(AllSessionKeys.UserId));
                objaudit.UserId = Convert.ToString(userValue);

            }
            else
            {
                objaudit.UserId = string.Empty;
            }

            objaudit.SessionId = context.HttpContext.Session.Id;
            objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            objaudit.PageAccessed = Convert.ToString(context.HttpContext.Request.Path); // URL User Requested
            objaudit.LoggedInAt = DateTime.Now;


            if (actionName == "LogOff")
            {
                objaudit.LoggedOutAt = DateTime.Now; // Time User Logged OUT 
            }

            objaudit.LoginStatus = "A";
            objaudit.ControllerName = controllerName; // ControllerName 
            objaudit.ActionName = actionName;

            RequestHeaders header = request.GetTypedHeaders();
            Uri uriReferer = header.Referer;

            if (uriReferer != null)
            {
                objaudit.UrlReferrer = header.Referer.AbsoluteUri;
            }

            _auditCommand.InsertAuditData(objaudit);
        }
    }
}
