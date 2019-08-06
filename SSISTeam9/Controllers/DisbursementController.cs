using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
namespace SSISTeam9.Controllers
{
    public class DisbursementController : Controller
    {
        // GET: Disbursement
        public ActionResult ViewAllDisbursements()
        {
            ViewData["disbursements"] = DisbursementListService.ViewAllDisbursments();

            return View();
        }
    }
}