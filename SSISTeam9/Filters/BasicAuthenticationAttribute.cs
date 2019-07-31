using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace SSISTeam9
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public string BasicRealm { get; set; }
        protected string Username { get; set; }
        protected string Password { get; set; }

        public BasicAuthenticationAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var auth = request.Headers["Authorization"];
            if(String.IsNullOrEmpty(auth))
            {
                //filterContext.HttpContext.Response.addHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Basic"));
                filterContext.HttpContext.Response.addHeader("Authorization", String.Format("Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("un:pw"), BasicRealm ?? "Basic")));
                filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true; filterContext.HttpContext.Response.addHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Basic"));

                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(":");
            var user = new { Name = cred[0], Pass = cred[1] };

        }
    }
}