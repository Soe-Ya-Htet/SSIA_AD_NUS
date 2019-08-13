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
            return Json(restService.Login(emp), JsonRequestBehavior.AllowGet);
        }

        [BasicAuthenticationAttribute(rExecuted = false)]
        [HttpPost]
        [Route("logout")]
        public ActionResult PostLogout()
        {
            return Json(restService.Logout(), JsonRequestBehavior.AllowGet);
        }
    }
}