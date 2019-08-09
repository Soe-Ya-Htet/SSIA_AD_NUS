using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;

namespace SSISTeam9.Controllers
{
    public class AdjVoucherController : Controller
    {
        // GET: AdjVoucher
        public ActionResult Index()
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            return View(adjVouchers);
        }

        public ActionResult Generate(List<AdjVoucher> adjVouchers)
        {
            return View("Index");
        }

    }
}