using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;

namespace SSISTeam9.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All()
        {
            List<PurchaseOrder> orders = PurchaseOrderService.GetAllOrders();

            ViewData["orders"] = orders;
            return View();
        }

        public ActionResult Edit(string orderNumber)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderNumber);

            ViewData["order"] = order;
            return View();
        }

        public ActionResult ChooseSuppliers(PurchaseOrder order)
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

            return View();
        }

        public ActionResult ConfirmUpdate(FormCollection formCollection, string counter)
        {
            string supplier1Name = formCollection["item_0"];
            string quantity1 = formCollection["quantity_0"];
            string supplier2Name = formCollection["item_1"];
            string quantity2 = formCollection["quantity_1"];
            return null;
        }

        public ActionResult Close(string orderNumber)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderNumber);

            ViewData["order"] = order;
            return View();
        }
    }
}