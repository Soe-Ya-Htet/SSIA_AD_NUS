using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SSISTeam9.Models;
using SSISTeam9.DAO;
using SSISTeam9.Services;
using SSISTeam9.Filters;


namespace SSISTeam9.Controllers
{
    [CatalogueFilter]
    public class CatalogueController : Controller
    {
        //For store employee to view and add catalogue
        public ActionResult All(string sessionId)
        {
            string desc = "";
            string cat = "";
            desc = null == Request.Form["desSearch"] ? "" : Request.Form["desSearch"];
            cat = null == Request.Form["catSearch"] ? "" : Request.Form["catSearch"];
            List<Inventory> catalogues = RequisitionService.ShowItems(desc, cat);
            List<string> category = new List<string>();
            category = RequisitionService.GetALLCategories();

            ViewData["catalogues"] = catalogues;
            ViewData["sessionId"] = sessionId;
            ViewData["category"] = category;
            return View();
        }



        public ActionResult Delete(bool confirm, long itemId, string sessionId)
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
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            return null;
        }

        public ActionResult Create(string sessionId)
        {
            List<string> categories = CatalogueService.GetAllCategories();
            ViewData["categories"] = categories;
            List<string> unitsOfMeasure = CatalogueService.GetAllUnits();
            ViewData["unitsOfMeasure"] = unitsOfMeasure;
            ViewData["sessionId"] = sessionId;
            return View();
        }


        public ActionResult CreateCatalogue(Inventory catalogue, string sessionId)
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
                TempData["errorMsg"] = "<script>alert('Catalogue code already exists! Please verify.');</script>";
                return RedirectToAction("Create", new { sessionid = sessionId });
            }
            return RedirectToAction("CreateNext", new { itemId = ItemId, sessionid = sessionId });
        }


        public ActionResult CreateNext(long itemId, string sessionId)
        {
            ViewData["itemId"] = itemId;          
            
            List<string> supplierNames = SupplierService.GetAllSupplierNames();
            ViewData["supplierNames"] = supplierNames;
            ViewData["sessionId"] = sessionId;
            return View();
        }


        public ActionResult CreatePriceList(long ItemId, PriceList priceList, string sessionId)
        {
            if(priceList.Supplier1Name == priceList.Supplier2Name || priceList.Supplier2Name == priceList.Supplier3Name || priceList.Supplier3Name == priceList.Supplier1Name)
            {
                TempData["errorMsg"] = "<script>alert('Please select three different suppliers.');</script>";
                return RedirectToAction("CreateNext", new { itemId = ItemId, sessionid = sessionId });
            }
            else
            {
                priceList.Item = new Inventory();
                priceList.Item.ItemId = ItemId;
                PriceListService.CreatePriceListDetaills(priceList);

                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogues;
                return RedirectToAction("All", new { sessionid = sessionId });
            }           
        }

        //For store employee to view catalogue details and update & delete catalogue
        public ActionResult Details(long itemId, string sessionId)
        {
            ViewData["catalogue"] = CatalogueService.GetCatalogueById(itemId);
            ViewData["pricelist"] = PriceListService.GetPriceListByItemId(itemId);
            ViewData["sessionId"] = sessionId;
            return View();
        }


        public ActionResult Update(long itemId, string sessionId)
        {
            ViewData["catalogue"] = CatalogueService.GetCatalogueById(itemId);
            List<string> categories = CatalogueService.GetAllCategories();
            ViewData["categories"] = categories;
            List<string> unitsOfMeasure = CatalogueService.GetAllUnits();
            ViewData["unitsOfMeasure"] = unitsOfMeasure;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult UpdateCatalogue(Inventory catalogue, string sessionId)
        {

            long ItemId = catalogue.ItemId;          

            CatalogueService.UpdateCatalogue(catalogue);
            return RedirectToAction("UpdateNext", new { itemId = ItemId, sessionid = sessionId });
        }


        public ActionResult UpdateNext(long itemId, string sessionId)
        {
            ViewData["itemId"] = itemId;
            ViewData["pricelist"] = PriceListService.GetPriceListByItemId(itemId);

            if(PriceListService.GetPriceListByItemId(itemId) == null)
            {
                return RedirectToAction("CreateNext", new { itemId, sessionid = sessionId });
            }
            else
            {
                List<string> supplierNames = SupplierService.GetAllSupplierNames();
                ViewData["supplierNames"] = supplierNames;
                ViewData["sessionId"] = sessionId;
                return View();
            }            
        }

        public ActionResult UpdatePriceList(long ItemId, PriceList priceList, string sessionId)
        {
            if (priceList.Supplier1Name == priceList.Supplier2Name || priceList.Supplier2Name == priceList.Supplier3Name || priceList.Supplier3Name == priceList.Supplier1Name)
            {
                TempData["errorMsg"] = "<script>alert('Please select three different suppliers.');</script>";
                return RedirectToAction("UpdateNext", new { itemId = ItemId, sessionid = sessionId });
            }
            else
            {
                priceList.Item = new Inventory();
                priceList.Item.ItemId = ItemId;
                PriceListService.UpdatePriceList(priceList);

                List<Inventory> catalogues = CatalogueService.GetAllCatalogue();
                ViewData["catalogues"] = catalogues;
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            
        }


    }
}