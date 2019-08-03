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

        public ActionResult SelectItems(Inventory inventory)
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
            return null;
        }

        public ActionResult UpdatePurchaseOrder(PurchaseOrder order, FormCollection formCollection)
        {
            PurchaseOrder selectedOrder = PurchaseOrderService.GetOrderDetails(order.OrderNumber);

            List<long> selectedItemsIds = new List<long>();
            List<Inventory> selectedItems = new List<Inventory>();

            foreach (var item in order.ItemDetails)
            {
                if (item.Item.IsChecked)
                {
                    selectedItemsIds.Add(item.Item.ItemId);
                }
            }

            foreach (var id in selectedItemsIds)
            {
                selectedItems.Add(CatalogueService.GetCatalogueById(id));
            }

            foreach (var item in selectedItems)
            {
                item.ItemSuppliersDetails = PurchaseOrderService.GetItemSuppliersDetails(item.ItemId);
                item.ItemSuppliersDetails.Supplier1Name = SupplierService.GetSupplierName(item.ItemSuppliersDetails.Supplier1Id);
                item.ItemSuppliersDetails.Supplier2Name = SupplierService.GetSupplierName(item.ItemSuppliersDetails.Supplier2Id);
                item.ItemSuppliersDetails.Supplier3Name = SupplierService.GetSupplierName(item.ItemSuppliersDetails.Supplier3Id);
            }

            ViewData["selectedItems"] = selectedItems;
            ViewData["order"] = selectedOrder;
            ViewData["deliverTo"] = formCollection["deliverTo"];
            ViewData["deliverBy"] = formCollection["deliverBy"];

            return View();
        }
    }
}