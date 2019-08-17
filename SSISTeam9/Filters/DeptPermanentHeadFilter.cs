using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System.Web.Routing;

namespace SSISTeam9.Filters
{
    public class DeptPermanentHeadFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];
            Employee e = EmployeeService.GetUserBySessionId(sessionId);
            string userRole = e.EmpRole;
            string displayRole = e.EmpDisplayRole;

            if (!EmployeeService.IsActiveSessionId(sessionId))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Login" }
                    }
                );
            }
            else if (!(userRole == "HEAD" && displayRole == "HEAD"))
            {
                context.Result = new RedirectToRouteResult(
                       new RouteValueDictionary
                       {
                        { "controller", "Home" },
                        { "action", "NotAuthorised" }
                       }
                   );
            }
        }
    }
}