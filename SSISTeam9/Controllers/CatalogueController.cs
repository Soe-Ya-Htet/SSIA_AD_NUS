using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SSISTeam9.Models;
using SSISTeam9.DAO;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class CatalogueController : Controller
    {
        public ActionResult AllCatalogue()
        {
            List<Inventory> catalogues = CatalogueService.GetAllCatalogue();

            ViewData["catalogues"] = catalogues;
            return View();
        }

        public ActionResult Delete(bool confirm, long itemId)
        {
            if (confirm)
            {
                CatalogueService.DeleteCatalogue(itemId);

                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogues;
                return View("AllCatalogue");
            }
            return null;
        }

        public ActionResult CreateCatalogue()
        {
            return View();
        }

        public ActionResult CreateNew(Inventory catalogue, List<string> supplierCodes)
        {

            if (CatalogueService.VerifyExist(catalogue.ItemCode))
            {
                TempData["errorMsg"] = "<script>alert('Catalogue code already exists! Please Verify.');</script>";
                return View("CreateCatalogue");
            }
            else
            {
                CatalogueService.CreateCatalogueDetaills(catalogue, supplierCodes);

                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogue;               
            }
            return View("AllCatalogue");
        }

        public ActionResult DisplayCatalogueDetails(long itemId)
        {
            ViewData["catalogues"] = CatalogueService.GetCatalogueById(itemId);
            return View();
        }

        public ActionResult UpdateCatalogue(Inventory catalogue)
        {
            CatalogueService.UpdateCatalogue(catalogue);

            List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
            ViewData["catalogues"] = catalogues;
            return View("AllCatalogue");
        }
    }
}