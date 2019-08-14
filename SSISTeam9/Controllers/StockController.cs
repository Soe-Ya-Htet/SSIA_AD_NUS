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
    [StoreAuthorisationFilter]
    public class StockController : Controller
    {
        public async Task<ActionResult> All(string sessionId)
        {
            //Contact Python API to get predicted re-order amount and level  for item with code 'P021' and 'P030'
            //Done via StockService
            List<Inventory> items = await StockService.GetAllItemsOrdered();

            //Show the quantities which are being ordered by all store staff
            items = StockService.GetPendingOrderQuantities(items); 
            
            ViewData["items"] = items;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public async Task<ActionResult> EnterQuantities(Inventory inventory, string sessionId)
        {
            //Contact Python API to get predicted re-order amount and level for item with code 'P021' and 'P030'
            List<Inventory> stock = await StockService.GetAllItemsOrdered();
            List<Inventory> selectedItems = new List<Inventory>();

            for (int i = 0; i < stock.Count; i++)
            {
                if (inventory.CheckedItems[i] == true)
                {
                    selectedItems.Add(stock[i]);
                }
            }
            
            ViewData["selectedItems"] = selectedItems;
            ViewData["sessionId"] = sessionId;
            return View();
        }
        
        public ActionResult CreatePurchaseOrders(Inventory item, FormCollection formCollection, string sessionId)
        {
            List<int> itemsQuantities = new List<int>();
            List<long> itemIds = new List<long>();

            string counter = formCollection["counter"];
            string empId = EmployeeService.GetUserBySessionId(sessionId).EmpId.ToString();

            for (int i = 0; i < int.Parse(counter); i++)
            {
                itemsQuantities.Add(int.Parse(formCollection["quantity_" + i]));
                itemIds.Add(StockService.GetItemId(formCollection["item_" + i]));
            }
            
            List<long> itemsFirstSupplierIds = StockService.GetItemsFirstSupplierIds(itemIds);
            StockService.CreatePurchaseOrders(empId,itemIds,itemsFirstSupplierIds,itemsQuantities);

            return RedirectToAction("All", new {sessionid = sessionId});
        }

        //The following code used for Stock taking in order to generate Adjustment Voucher
        public ActionResult Check(string sessionId)
        {
            List<Inventory> inventories = CatalogueService.GetAllCatalogue();

            ViewData["inventories"] = inventories;
            ViewData["sessionId"] = sessionId;
            return View();
        }


    }
}