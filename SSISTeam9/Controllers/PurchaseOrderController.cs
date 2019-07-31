﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;

namespace SSISTeam9.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All()
        {
            List<PurchaseOrder> orders = PurchaseOrderService.GetAllOrders();

            ViewData["orders"] = orders;
            return View();
        }
    }
}