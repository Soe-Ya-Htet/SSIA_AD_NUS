using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class StoreMSController : Controller
    {
        //Store Manager and Supervisor landing page
        //Will have an additional tab to view adjustment voucher for approval
        public ActionResult Index()
        {
            return View();
        }
    }
}