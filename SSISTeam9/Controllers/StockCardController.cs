﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class StockCardController : Controller
    {
        public ActionResult All()
        {
            List<Inventory> catalogues = CatalogueService.GetAllCatalogue();

            ViewData["catalogues"] = catalogues;
            return View();
        }
    }
}