using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using SSISTeam9.Services;

namespace SSISTeam9.Filters
{
    // An Authorization Filter for Department Views
    public class DepartmentFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];
            string userRole = EmployeeService.GetUserBySessionId(sessionId).EmpRole;

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
            else if (userRole == "STORE_CLERK" || userRole == "STORE_SUPERVISOR" || userRole == "STORE_MANAGER")
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