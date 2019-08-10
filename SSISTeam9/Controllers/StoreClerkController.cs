using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class StoreClerkController : Controller
    {
        //Store Clerk landing page
        public ActionResult Home(string userName, string sessionId)
        {
            if (userName == null || sessionId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View();
            }
        }
    }
}