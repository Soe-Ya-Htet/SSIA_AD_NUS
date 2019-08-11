using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;
using SSISTeam9.Services;

namespace SSISTeam9.Filters
{
    public class PendingDeliveryOrderFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            string sessionId = HttpContext.Current.Request["sessionId"];
            string orderNumber = HttpContext.Current.Request["orderNumber"];
            long empId = EmployeeService.GetUserBySessionId(sessionId).EmpId;
            long empIdOfOrder = PurchaseOrderService.GetOrderDetails(orderNumber).EmployeeId;

            if (empId != empIdOfOrder)
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