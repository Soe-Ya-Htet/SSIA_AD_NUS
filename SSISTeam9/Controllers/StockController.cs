using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        public ActionResult All ()
        {
            List<Inventory> items = StockService.GetAllItems();

            ViewData["items"] = items;
            return View();
        }
    }
}