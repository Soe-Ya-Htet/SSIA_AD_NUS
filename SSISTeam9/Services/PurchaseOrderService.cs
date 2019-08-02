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
        public static List<PurchaseOrder> GetAllOrders()
        {
            List<PurchaseOrder> orders = PurchaseOrderDAO.GetAllOrders();

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
    }
}