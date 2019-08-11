using SSISTeam9.DAO;
using SSISTeam9.Models;
using SSISTeam9.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Filters
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public bool aExecuting = true;
        public bool rExecuted = true;
        const string authHeaderKey = "Authorization";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if(aExecuting)
            {
                var authHeader = filterContext.HttpContext.Request.Headers[authHeaderKey];
                if (authHeader != null)
                {
                    var basicTokens = authHeader.Split(' ');
                    if(!"Basic".Equals(basicTokens[0]))
                    {
                        HandleUnauthorized(filterContext);
                        return;
                    }
                    var decodedAuthToken = Base64Decode(basicTokens[1]);
                    var vals = decodedAuthToken.Split(':');
                    var username = vals[0];
                    var password = vals[1];
                    Employee emp = EmployeeDAO.GetUserPassword(username);
                    if (emp != null && password.Equals(emp.Password))
                    {
                        AuthUtil.CreatePrincipal(emp);
                        base.OnActionExecuting(filterContext);
                        return;
                    }

                }

                HandleUnauthorized(filterContext);
            }
        }

        private void HandleUnauthorized(ActionExecutingContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

        private string Base64Encode(String plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        private string Base64Decode(String encodedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if(rExecuted)
            {
                Employee emp = AuthUtil.GetCurrentLoggedUser();
                if(emp != null)
                    filterContext.HttpContext.Response.AddHeader(authHeaderKey, String.Format("Basic {0}", Base64Encode(emp.UserName + ":" + emp.Password)));
            }
        }

    }
}