using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System.Threading.Tasks;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    //[StoreAuthorisationFilter]
    public class StockController : Controller
    {
        public async Task<ActionResult> All(string userName, string sessionId)
        {
            //Contact Python API to get predicted re-order amount and level  for item with code 'P021'
            //Done via StockService
            List<Inventory> items = await StockService.GetAllItemsOrdered();

            //Show the quantities which are being ordered by all store staff
            items = StockService.GetPendingOrderQuantities(items); 

            ViewData["empId"] = EmployeeService.GetUserBySessionId(sessionId).EmpId.ToString(); 
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


        //The following code used for Stock taking in order to generate Adjustment Voucher
        public ActionResult Check()
        {
            List<Inventory> inventories = CatalogueService.GetAllCatalogue();

            ViewData["inventories"] = inventories;

            return View();
        }

        public ActionResult Generate(List<Inventory> inventories)
        {
            foreach(Inventory inventory in inventories)
            {
                int qty = inventory.ActualStock - inventory.StockLevel;
                if(qty != 0)
                {
                    StockService.CreateAdjVoucher(inventory.ItemId, qty);
                }
            }

            return RedirectToAction("Check");
        }

    }
}