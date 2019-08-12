using SSISTeam9.DAO;
using SSISTeam9.Filters;
using SSISTeam9.Models;
using SSISTeam9.Services;
using SSISTeam9.Utility;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("rest")]
    public class RestLoginController : Controller
    {
        private readonly IRestService restService;

        public RestLoginController(IRestService restService)
        {
            this.restService = restService;
        }

        [BasicAuthenticationAttribute(aExecuting = false)]
        [Route("login")]
        [HttpPost]
        public ActionResult PostLogin(Employee emp)
        {
            //Dictionary<string, object> resDict = new Dictionary<string, object>();

            //Employee emp2 = EmployeeDAO.GetUserPassword(emp.UserName);
            //if(emp2 == null || string.IsNullOrEmpty(emp2.UserName))
            //{
            //    resDict.Add("login", false);
            //    resDict.Add("msg", "Username not found");
            //}
            //else if (!emp.Password.Equals(emp2.Password))
            //{
            //    resDict.Add("login", false);
            //    resDict.Add("msg", "Incorrect password");
            //} else
            //{
            //    resDict.Add("login", true);
            //    resDict.Add("msg", "Login success");
            //    resDict.Add("emp", emp2);
            //    AuthUtil.CreatePrincipal(emp2);
            //}

            return Json(restService.Login(emp), JsonRequestBehavior.AllowGet);
        }

        [BasicAuthenticationAttribute(rExecuted = false)]
        [HttpPost]
        [Route("logout")]
        public ActionResult PostLogout()
        {
            //AuthUtil.InvalidateUser();
            return Json(restService.Logout(), JsonRequestBehavior.AllowGet);
        }
    }
}