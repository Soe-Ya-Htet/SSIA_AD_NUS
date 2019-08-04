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
            List<Inventory> items = StockService.GetAllItemsOrdered();

            ViewData["items"] = items;
            return View();
        }

        public ActionResult EnterQuantities(Inventory inventory)
        {
            List<Inventory> stock = StockService.GetAllItemsOrdered();
            List<Inventory> selectedItems = new List<Inventory>();

            for (int i = 0; i < stock.Count; i++)
            {
                if (inventory.checkedItems[i] == true)
                {
                    selectedItems.Add(stock[i]);
                }
            }

            ViewData["selectedItems"] = selectedItems;
            return View();
        }

        public ActionResult CreatePurchaseOrders(Inventory item, FormCollection formCollection)
        {
            Dictionary<string, int> itemsQuantities = new Dictionary<string, int>();

            string counter = formCollection["counter"];

            for(int i = 0; i < int.Parse(counter); i++)
            {
                itemsQuantities.Add(formCollection["item_" + i], int.Parse(formCollection["quantity_" + i]));
            }

            //To create Service method to create. Group items with same first supplier together...
            return View("All");
        }
    }
}