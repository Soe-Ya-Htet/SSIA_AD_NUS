using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("rest")]
    public class RestLoginController : Controller
    {
        [Route("login")]
        public ActionResult Login()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}