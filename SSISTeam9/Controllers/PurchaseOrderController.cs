using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    [StoreAuthorisationFilter]
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All(string sessionId)
        {
            Employee user = EmployeeService.GetUserBySessionId(sessionId);
            List<PurchaseOrder> orders = PurchaseOrderService.GetAllOrders(user.EmpId);
            
            ViewData["sessionId"] = sessionId;
            ViewData["orders"] = orders;
            return View();
        }

        public ActionResult Edit(PurchaseOrder selectedOrder, string sessionId)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(selectedOrder.OrderNumber);
            
            if (order.Status == "Pending Delivery")
            {
                return RedirectToAction("Close", new { orderNumber = selectedOrder.OrderNumber, sessionid = sessionId });
            }
            else if (order.Status == "Closed")
            {
                return RedirectToAction("ViewClosedPO", new { orderNumber = selectedOrder.OrderNumber, sessionid = sessionId });
            }
            
            ViewData["sessionId"] = sessionId;
            ViewData["order"] = order;
            return View();
        }

        public ActionResult ConfirmOrder(bool confirm, string orderNumber, string sessionId)
        {
            if (confirm)
            {
                PurchaseOrderService.ConfirmOrder(orderNumber);

                //List<PurchaseOrder> orders = PurchaseOrderService.GetAllOrders(EmployeeService.GetUserBySessionId(sessionId).EmpId);
                
                //ViewData["sessionId"] = sessionId;
                //ViewData["orders"] = orders;
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            return null;
        }

        public ActionResult UpdatePurchaseOrder(PurchaseOrder order, FormCollection formCollection, string sessionId)
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
            ViewData["sessionId"] = sessionId;

            return View();
        }

        public ActionResult ConfirmUpdate(PurchaseOrder order, FormCollection formCollection, string sessionId)
        {
            PurchaseOrder selectedOrder = PurchaseOrderService.GetOrderDetails(order.OrderNumber);

            string counter = formCollection["counter"];

            List<string> selectedItemIds = new List<string>();
            List<string> selectedAltSuppliersNames = new List<string>();
            List<long> selectedAltSuppliersIds = new List<long>();
            List<string> updateQuantities = new List<string>();
            List<string> newQuantities = new List<string>();

            for (int i = 0; i < int.Parse(counter); i++)
            {
                selectedItemIds.Add(formCollection["selectedItem_" + i]);
                selectedAltSuppliersNames.Add(formCollection["item_" + i]);
                updateQuantities.Add(formCollection["originalquantity_" + i]);
                newQuantities.Add(formCollection["quantity_" + i]);
            }

            foreach (var c in selectedAltSuppliersNames)
            {
                selectedAltSuppliersIds.Add(SupplierService.GetSupplierId(c));
            }
            int totalQuantity = PurchaseOrderService.UpdatePurchaseOrder(selectedOrder, selectedItemIds, updateQuantities, int.Parse(counter), order.DeliverTo, order.DeliverBy);
            
            if (newQuantities.Sum(m => int.Parse(m)) != 0)
            {
                PurchaseOrderService.CreatePurchaseOrders(selectedOrder, selectedItemIds, selectedAltSuppliersIds, newQuantities, int.Parse(counter));
            }
            
            //if no more items in current Purchase Order, to remove record from Purchase Order and Purchase Order Details
            if (totalQuantity == 0)
            {
                PurchaseOrderService.DeletePurchaseOrder(selectedOrder.OrderNumber);
            }
            return RedirectToAction("All", new { sessionid = sessionId });
        }

        public ActionResult Close(string orderNumber, string sessionId)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderNumber);
            
            ViewData["order"] = order;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult Delete(bool confirm, string orderNumber, string sessionId)
        {
            if (confirm)
            {
                PurchaseOrderService.DeletePurchaseOrder(orderNumber);
                
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            return null;
        }

        public ActionResult ConfirmClose(PurchaseOrder orderToClose, FormCollection formCollection, string sessionId)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderToClose.OrderNumber);
            
            List<int> itemsQuantities = new List<int>();
            List<long> itemIds = new List<long>();

            for (int i = 0; i < order.ItemDetails.Count; i++)
            {
                itemsQuantities.Add(int.Parse(formCollection["quantity_" + i]));
                itemIds.Add(long.Parse(formCollection["item_" + i]));
            }

            /*The following code is for StockCard table*/
            //By the time close order, update StockCard table with itemId, deptId and date, souceType = 3
            StockCardService.CreateStockCardFromOrder(order, itemIds, itemsQuantities);

            //SET status to close and update quantities (if any) accordingly
            //Stock level is also updated accordingly
            PurchaseOrderService.ClosePurchaseOrder(order, itemIds, itemsQuantities);
            
            return RedirectToAction("All", new { sessionid = sessionId });
        }

        public ActionResult ViewClosedPO(string orderNumber, string sessionId)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderNumber);

            ViewData["order"] = order;
            return View("Closed");
        }

        public ActionResult UpdatePurchaseOrderDeliveryDetails(PurchaseOrder order, string sessionId)
        {
            PurchaseOrderService.UpdatePurchaseOrderDeliveryDetails(order);
            
            return RedirectToAction("All", new { sessionid = sessionId });
        }
    }
}