using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class AdjVoucherController : Controller
    {
        // GET: AdjVoucher
        public ActionResult Index(List<AdjVoucher> adjVouchers)
        {
            AdjVoucherService.InsertReason(adjVouchers);
            return View();
        }
    }
}