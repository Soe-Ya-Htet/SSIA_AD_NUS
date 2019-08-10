using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("rest/representative")]
    public class RestRepresentativeController : Controller
    {
        [Route("items/received")]
        public ActionResult Index()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}