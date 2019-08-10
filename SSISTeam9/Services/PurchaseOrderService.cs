using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.DAO;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Services
{
    public class PurchaseOrderService
    {
        public static List<PurchaseOrder> GetAllOrders(long empId)
        {
            List<PurchaseOrder> orders = PurchaseOrderDAO.GetAllOrders(empId);

            foreach (var order in orders)
            {
                order.Employee = EmployeeDAO.GetEmployeeById(order.EmployeeId);
                order.Supplier = SupplierDAO.GetSupplierById(order.SupplierId);
                order.ItemDetails = PurchaseOrderDAO.GetItemsInPurchaseOrder(order.OrderId);
            }

            return orders;
        }

        public static PurchaseOrder GetOrderDetails(string orderNumber)
        {
            PurchaseOrder order = PurchaseOrderDAO.GetOrderDetails(orderNumber);

            order.Employee = EmployeeDAO.GetEmployeeById(order.EmployeeId);
            order.Supplier = SupplierDAO.GetSupplierById(order.SupplierId);
            order.ItemDetails = PurchaseOrderDAO.GetItemsInPurchaseOrder(order.OrderId);

            foreach (var item in order.ItemDetails)
            {
                item.Item = CatalogueDAO.GetCatalogueById(item.ItemId);
                item.Item.ItemSuppliersDetails = SupplierDAO.GetItemSuppliersDetails(item.ItemId);
                item.Item.ItemSuppliersDetails.Supplier1Name = SupplierDAO.GetSupplierName(item.Item.ItemSuppliersDetails.Supplier1Id);
                item.Item.ItemSuppliersDetails.Supplier2Name = SupplierDAO.GetSupplierName(item.Item.ItemSuppliersDetails.Supplier2Id);
                item.Item.ItemSuppliersDetails.Supplier3Name = SupplierDAO.GetSupplierName(item.Item.ItemSuppliersDetails.Supplier3Id);
            }
            return order;
        }
        
        public static PriceList GetItemSuppliersDetails(long itemId)
        {
            return SupplierDAO.GetItemSuppliersDetails(itemId);
        }

        public static void ClosePurchaseOrder(PurchaseOrder order, List<long> itemIds, List<int> itemQuantities)
        {
            PurchaseOrderDAO.ClosePurchaseOrder(order.OrderNumber);

            int totalQuantity = itemQuantities.Sum(m => m);

            //if no more items in Purchase Order, to remove record from Purchase Order and Purchase Order Details
            if (totalQuantity == 0)
            {
                PurchaseOrderDAO.DeleteAllPurchaseOrderDetails(order.OrderId);
                PurchaseOrderDAO.DeletePurchaseOrder(order.OrderId);
            }
            else
            {
                //Update quantities(if any)
                for (int i = 0; i < itemIds.Count; i++)
                {
                    if (itemQuantities[i] == 0)
                    {
                        PurchaseOrderDAO.DeleteItemFromPurchaseOrder(order.OrderId, itemIds[i]);
                    }
                    else
                    {
                        PurchaseOrderDAO.UpdatePurchaseOrderItemQuantity(order.OrderId, itemIds[i], itemQuantities[i]);
                    }
                }
            }

            //Updated inventory stock level
            PurchaseOrder updatedOrder = GetOrderDetails(order.OrderNumber);
            Dictionary<long, int> itemAndNewStock = new Dictionary<long, int>();
            foreach(var item in updatedOrder.ItemDetails)
            {
                itemAndNewStock[item.ItemId] = item.Quantity + item.Item.StockLevel;
            }
            
            //Update stock level for items in Purchase Order to be closed
            StockDAO.UpdateInventoryStock(itemAndNewStock);
        }

        public static int UpdatePurchaseOrder(PurchaseOrder order, List<string>itemIds, List<string> updatedQuantities, int itemCount, string deliverTo, DateTime deliverBy)
        {
            PurchaseOrderDAO.UpdatePurchaseOrderDeliveryDetails(order.OrderId, deliverTo, deliverBy);

            for (int i = 0; i < itemCount; i++)
            {
                if (int.Parse(updatedQuantities[i]) == 0)
                {
                    PurchaseOrderDAO.DeleteItemFromPurchaseOrder(order.OrderId, long.Parse(itemIds[i]));
                }
                else if (order.ItemDetails[i].Quantity != int.Parse(updatedQuantities[i]))
                {
                    PurchaseOrderDAO.UpdatePurchaseOrderItemQuantity(order.OrderId, long.Parse(itemIds[i]), int.Parse(updatedQuantities[i]));
                }
            }

            //if no items left, to delete PO.
            return updatedQuantities.Sum(m => int.Parse(m));
        }

        public static void CreatePurchaseOrders(PurchaseOrder order, List<string> itemIds, List<long> altSuppliers, List<string> quantities, int itemCount)
        {
            //Key: SupplierId
            //Value: List of Tuple of Item Id and Corresponding Quantity
            Dictionary<long, List<Tuple<long,int>>> uniquePurchaseOrders = new Dictionary<long, List<Tuple<long, int>>>();
            string newOrderNumber = null;
            
            //To group items with same alt supplier together into one Purchase Order
            for (int i = 0; i < itemCount; i++)
            {
                if (int.Parse(quantities[i]) > 0)
                {
                    if(uniquePurchaseOrders.ContainsKey(altSuppliers[i]))
                    {
                        uniquePurchaseOrders[altSuppliers[i]].Add(new Tuple<long,int>(long.Parse(itemIds[i]),int.Parse(quantities[i])));
                    }
                    else
                    {
                        List<Tuple<long, int>> selectedItemsIdsAndQuantites = new List<Tuple<long, int>>();
                        uniquePurchaseOrders.Add(altSuppliers[i], selectedItemsIdsAndQuantites);
                        uniquePurchaseOrders[altSuppliers[i]].Add(new Tuple<long, int>(long.Parse(itemIds[i]), int.Parse(quantities[i])));
                    }
                }
            }

            foreach (KeyValuePair<long, List<Tuple<long, int>>> po in uniquePurchaseOrders)
            {
                //Create new record under PurchaseOrder table.
                newOrderNumber = PurchaseOrderDAO.CreatePurchaseOrderAfterChangeSuppliers(order, po.Key);

                foreach (var itemIdAndQty in po.Value)
                {
                    //To insert new record under PurchaseOrderDetails table for each Item Id for same order Id.
                    PurchaseOrder newOrder = PurchaseOrderDAO.GetOrderDetails(newOrderNumber);

                    PurchaseOrderDAO.CreatePurchaseOrderDetailsAfterChangeSuppliers(newOrder.OrderId, itemIdAndQty.Item1, itemIdAndQty.Item2);
                }
            }
        }

        public static void DeletePurchaseOrder(string orderNumber)
        {
            PurchaseOrder order = PurchaseOrderDAO.GetOrderDetails(orderNumber);

            //Delete records from both PurchaseOrder and PurchaseOrderDetails
            PurchaseOrderDAO.DeleteAllPurchaseOrderDetails(order.OrderId);
            PurchaseOrderDAO.DeletePurchaseOrder(order.OrderId);
        }

        public static void ConfirmOrder(string orderNumber)
        {
            PurchaseOrderDAO.ConfirmOrder(orderNumber);
        }

        public static PurchaseOrder GetOrderById(long orderId)
        {
            return PurchaseOrderDAO.GetOrderById(orderId);
        }

        public static void UpdatePurchaseOrderDeliveryDetails(PurchaseOrder order)
        {
            PurchaseOrder orderForId = PurchaseOrderDAO.GetOrderDetails(order.OrderNumber);
            PurchaseOrderDAO.UpdatePurchaseOrderDeliveryDetails(orderForId.OrderId, order.DeliverTo, order.DeliverBy);
        } 
    }
}