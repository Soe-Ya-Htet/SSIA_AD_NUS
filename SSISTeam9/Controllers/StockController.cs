using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System.Threading.Tasks;

namespace SSISTeam9.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        public async Task<ActionResult> All()
        {
            //Contact Python API to get predicted re-order amount and level  for item with code 'P021'
            //Done via StockService
            List<Inventory> items = await StockService.GetAllItemsOrdered();

            //Show the quantities which are being ordered by all store staff
            items = StockService.GetPendingOrderQuantities(items); 

            ViewData["empId"] = "3"; //to change once log in/log out is implemented
            ViewData["items"] = items;
            return View();
        }

        public async Task<ActionResult> EnterQuantities(Inventory inventory, string empId)
        {
            //Contact Python API to get predicted re-order amount and level for item with code 'P021'
            List<Inventory> stock = await StockService.GetAllItemsOrdered();
            List<Inventory> selectedItems = new List<Inventory>();

            for (int i = 0; i < stock.Count; i++)
            {
                if (inventory.CheckedItems[i] == true)
                {
                    selectedItems.Add(stock[i]);
                }
            }

            ViewData["empId"] = empId;
            ViewData["selectedItems"] = selectedItems;
            return View();
        }

        public ActionResult CreatePurchaseOrders(Inventory item, FormCollection formCollection)
        {
            List<int> itemsQuantities = new List<int>();
            List<long> itemIds = new List<long>();

            string counter = formCollection["counter"];
            string empId = formCollection["empId"];

            for(int i = 0; i < int.Parse(counter); i++)
            {
                itemsQuantities.Add(int.Parse(formCollection["quantity_" + i]));
                itemIds.Add(StockService.GetItemId(formCollection["item_" + i]));
            }

            //To create Service method to create. Group items with same first supplier together...

            List<long> itemsFirstSupplierIds = StockService.GetItemsFirstSupplierIds(itemIds);
            StockService.CreatePurchaseOrders(empId,itemIds,itemsFirstSupplierIds,itemsQuantities);

            return RedirectToAction("All");
        }

    }
}