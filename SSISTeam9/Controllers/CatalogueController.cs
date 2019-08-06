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
                if (PriceListService.GetPriceListByItemId(itemId) != null)
                {
                    PriceListService.DeletePriceList(itemId);
                }

                CatalogueService.DeleteCatalogue(itemId);

                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogues;
                return RedirectToAction("All");
            }
            return null;
        }

        public ActionResult Create()
        {
            List<string> categories = CatalogueService.GetAllCategories();
            ViewData["categories"] = categories;
            List<string> unitsOfMeasure = CatalogueService.GetAllUnits();
            ViewData["unitsOfMeasure"] = unitsOfMeasure;
            return View();
        }


        public ActionResult CreateCatalogue(Inventory catalogue)
        {

            long ItemId = 0;
            try
            {
                string itemCode = catalogue.ItemCode.ToUpper();
                catalogue.ItemCode = itemCode;

                ItemId = CatalogueService.CreateCatalogueDetaills(catalogue);
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Catalogue code already exists! Please Verify.');</script>";
                return RedirectToAction("Create");
            }
            return RedirectToAction("CreateNext", new { itemId = ItemId});
        }


        public ActionResult CreateNext(long itemId)
        {
            ViewData["itemId"] = itemId;          
            
            List<string> supplierNames = SupplierService.GetAllSupplierNames();
            ViewData["supplierNames"] = supplierNames;
            return View();
        }


        public ActionResult CreatePriceList(long ItemId, PriceList priceList)
        {
            priceList.Item = new Inventory();
            priceList.Item.ItemId = ItemId;
            PriceListService.CreatePriceListDetaills(priceList);

            List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
            ViewData["catalogues"] = catalogues;
            return RedirectToAction("All");
        }

        public ActionResult Details(long itemId)
        {
            ViewData["catalogue"] = CatalogueService.GetCatalogueById(itemId);
            ViewData["pricelist"] = PriceListService.GetPriceListByItemId(itemId);
            return View();
        }

        public ActionResult Update(long itemId)
        {
            ViewData["catalogue"] = CatalogueService.GetCatalogueById(itemId);
            List<string> categories = CatalogueService.GetAllCategories();
            ViewData["categories"] = categories;
            List<string> unitsOfMeasure = CatalogueService.GetAllUnits();
            ViewData["unitsOfMeasure"] = unitsOfMeasure;
            return View();
        }

        public ActionResult UpdateCatalogue(Inventory catalogue)
        {

            long ItemId = catalogue.ItemId;
            try
            {
                string itemCode = catalogue.ItemCode.ToUpper();
                catalogue.ItemCode = itemCode;

                CatalogueService.UpdateCatalogue(catalogue);

            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Catalogue code already exists! Please Verify.');</script>";
                return RedirectToAction("Update");
            }
            return RedirectToAction("UpdateNext", new { itemId = ItemId });
        }


        public ActionResult UpdateNext(long itemId)
        {
            ViewData["itemId"] = itemId;
            ViewData["pricelist"] = PriceListService.GetPriceListByItemId(itemId);

            if(PriceListService.GetPriceListByItemId(itemId) == null)
            {
                return RedirectToAction("CreateNext", new { itemId = itemId });
            }
            else
            {
                List<string> supplierNames = SupplierService.GetAllSupplierNames();
                ViewData["supplierNames"] = supplierNames;
                return View();
            }            
        }

        public ActionResult UpdatePriceList(long ItemId, PriceList priceList)
        {
            priceList.Item = new Inventory();
            priceList.Item.ItemId = ItemId;
            PriceListService.UpdatePriceList(priceList);

            List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
            ViewData["catalogues"] = catalogues;
            return RedirectToAction("All");
        }


    }
}