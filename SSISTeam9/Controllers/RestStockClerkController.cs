using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("Rest/StockClerk")]
    public class RestStockClerkController : Controller
    {
        [Route("Index")]
        public ActionResult Index()
        {
            return Json("No Memory", JsonRequestBehavior.AllowGet);
        }
    }
}