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

        public ActionResult ChooseSuppliers(PurchaseOrder order, FormCollection formCollection)
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

        public ActionResult ConfirmUpdate(PurchaseOrder order, FormCollection formCollection)
        {
            PurchaseOrder selectedOrder = PurchaseOrderService.GetOrderDetails(order.OrderNumber);

            string counter = formCollection["counter"];

            List<string> selectedItemIds = new List<string>();
            List<string> selectedAltSuppliers = new List<string>();
            List<string> updateQuantities = new List<string>();
            List<string> newQuantities = new List<string>();

            for (int i = 0; i < int.Parse(counter); i++)
            {
                selectedItemIds.Add(formCollection["selectedItem_" + i]);
                selectedAltSuppliers.Add(formCollection["item_" + i]);
                updateQuantities.Add(formCollection["originalquantity_" + i]);
                newQuantities.Add(formCollection["quantity_" + i]);
            }
            
            PurchaseOrderService.UpdatePurchaseOrder(selectedOrder, selectedItemIds, updateQuantities, int.Parse(counter), order.DeliverTo, order.DeliverBy);

            selectedOrder = PurchaseOrderService.GetOrderDetails(order.OrderNumber);
            PurchaseOrderService.CreatePurchaseOrders(selectedOrder, selectedItemIds, selectedAltSuppliers, updateQuantities, int.Parse(counter));

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