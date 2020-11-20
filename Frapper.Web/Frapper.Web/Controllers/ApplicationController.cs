using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frapper.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Exceptional;

namespace Frapper.Web.Controllers
{
    [AuthorizeSuperAdmin]
    public class ApplicationController : Controller
    {
        public async Task Exceptions()
        {
            await ExceptionalMiddleware.HandleRequestAsync(HttpContext).ConfigureAwait(false);
        }
    }
}
