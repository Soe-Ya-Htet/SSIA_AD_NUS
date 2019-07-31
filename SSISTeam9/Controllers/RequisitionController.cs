using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewRequisition()
        {
            List<Inventory> itemList = RequisitionService.GetAllInventory();
            ViewData["itemList"] = itemList;
            
            return View();
        }
    }
}