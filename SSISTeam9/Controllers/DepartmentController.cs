﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Controllers
{
    public class DepartmentController : Controller
    {
        public ActionResult ViewCatalogue()
        {
            List<Inventory> catalogues = new List<Inventory>();
            ViewData["AllCatalogue"] = CatalogueDAO.DisplayAllCatalogue();

            return View();
        }

        public ActionResult DeleteCatalogue(int itemId)
        {
            CatalogueDAO.DeleteCatalogue(itemId);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult CreateCatalogue(string ItemCode, string Category, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
        {
            CatalogueDAO.CreateCatalogue(ItemCode, Category, Description, UnitOfMeasure, Supplier1Id, Supplier2Id, Supplier3Id);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult UpdateCatalogue()
        {
            return View();
        }

    }
}