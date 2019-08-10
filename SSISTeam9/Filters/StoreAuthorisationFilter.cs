using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;

namespace SSISTeam9.Filters
{
    // An Authorization Filter for Store Views
    public class StoreAuthorisationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];

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
            else
            {
                string userRole = EmployeeService.GetUserBySessionId(sessionId).EmpRole;
                if (userRole == "EMPLOYEE" || userRole == "HEAD")
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
}