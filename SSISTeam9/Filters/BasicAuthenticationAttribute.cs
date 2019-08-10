using System;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Security.Principal;
using System.Threading;
using System.Net.Http;
using System.Net;
using SSISTeam9.Models;

namespace SSISTeam9.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Uri uri = actionContext.Request.RequestUri;
            if(uri.AbsoluteUri.Substring(uri.AbsoluteUri.IndexOf("/rest/"), uri.AbsoluteUri.Length).StartsWith("login"))
            {
                return;
            }

            var authHeader = actionContext.Request.Headers.Authorization;
            if(authHeader != null)
            {
                var authToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                var vals = decodedAuthToken.Split(':');
                var username = vals[0];
                var password = vals[1];
                var isValid = false;
                if(isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(username), null);
                    Thread.CurrentPrincipal = principal;
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK, "Success");
                    actionContext.Response.Headers.Add("Authorization", String.Format("Basic {0}", Base64Encode(username+":"+password)));
                    return;
                }
            }

            HandleUnauthorized(actionContext);
        }

        private void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private string Base64Encode(String plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        private string Base64Decode(String encodedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
        }

    }
}
