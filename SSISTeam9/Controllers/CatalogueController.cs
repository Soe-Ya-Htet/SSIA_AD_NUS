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
        public ActionResult All()
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
                return RedirectToAction("All");
            }
            return null;
        }

        public ActionResult Create()
        {
            List<string> supplierNames = SupplierService.GetAllSupplierNames();
            ViewData["supplierNames"] = supplierNames;
            List<string> unitsOfMeasure = CatalogueService.GetAllUnits();
            ViewData["unitsOfMeasure"] = unitsOfMeasure;
            return View();
        }

        public ActionResult CreateNew(Inventory catalogue, List<string> supplierCodes)
        {

            
            try
            {
                PriceList test = new PriceList();
                CatalogueService.CreateCatalogueDetaills(test, supplierCodes);

                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogue;               
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Catalogue code already exists! Please Verify.');</script>";
                return RedirectToAction("Create");
            }
            return RedirectToAction("All");
        }

        public ActionResult Details(long itemId)
        {
            ViewData["catalogue"] = CatalogueService.GetCatalogueById(itemId);
            ViewData["pricelist"] = PriceListService.GetPriceListByItemId(itemId);
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }

        public ActionResult UpdateDetails(Inventory catalogue, List<string> supplierCodes)
        {
            try
            {
                CatalogueService.UpdateCatalogue(catalogue, supplierCodes);
                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogues;
            }
            catch(SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Catalogue code already exists! Please Verify.');</script>";
                return RedirectToAction("Create");
            }
       
            return RedirectToAction("All");
        }
    }
}