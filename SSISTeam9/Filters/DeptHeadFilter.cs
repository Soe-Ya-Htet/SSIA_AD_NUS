using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using SSISTeam9.Models;

namespace SSISTeam9.Filters
{
    public class DeptHeadFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];
            Employee e = EmployeeService.GetUserBySessionId(sessionId);
            string userRole = e.EmpRole;
            string displayRole = e.EmpDisplayRole;
            bool between = DelegateService.CheckDate(e.DeptId);
            bool after = DelegateService.AfterDate(e.DeptId);
            bool delegated = false;
            if(between && !after)
            {
                delegated = true;
            }

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

            else if((!(userRole == "HEAD" && displayRole == "HEAD" && !delegated)) && (!(userRole=="HEAD" && displayRole=="EMPLOYEE" && delegated)))
            {
                context.Result = new RedirectToRouteResult(
                       new RouteValueDictionary
                       {
                        { "controller", "Home" },
                        { "action", "NotAuthorised" }
                       }
                   );
            }

            else if (userRole != "HEAD")
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