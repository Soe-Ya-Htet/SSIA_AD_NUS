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
    // An Authorization Filter for Department Views
    public class DepartmentFilter : ActionFilterAttribute, IAuthorizationFilter
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
            else if (!(userRole == "EMPLOYEE" && displayRole == "EMPLOYEE") && !(userRole == "REPRESENTATIVE" && displayRole == "REPRESENTATIVE"))
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