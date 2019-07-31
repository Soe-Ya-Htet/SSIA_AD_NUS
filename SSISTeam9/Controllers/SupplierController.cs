using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models.Supplier;
using SSISTeam9.DAO;

namespace SSISTeam9.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        //Display all suppliers
        public ActionResult All()
        {
            return View();
        }

        public ActionResult CreateNew()
        {
            return View();
        }

        public ActionResult UpdateDetails()
        {
            return View();
        }

    }
}