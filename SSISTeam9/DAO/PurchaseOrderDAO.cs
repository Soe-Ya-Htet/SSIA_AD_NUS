using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class PurchaseOrderDAO
    {
        public static List<PurchaseOrder> GetAllOrders()
        {
            List<PurchaseOrder> orders = new List<PurchaseOrder>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PurchaseOrder";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PurchaseOrder order = new PurchaseOrder()
                    {
                        OrderId = (long)reader["orderId"],
                        OrderNumber = (string)reader["orderNumber"],
                        Status = (string)reader["status"],
                        SubmittedDate= (DateTime)reader["submittedDate"],
                        OrderDate = (DateTime)reader["orderDate"],
                        SupplierId = (long)reader["supplierId"],
                        EmployeeId = (long)reader["empId"]

                    };
                    orders.Add(order);
                }

                foreach (var order in orders)
                {
                    order.Employee = EmployeeDAO.GetEmployeeById(order.EmployeeId);
                    order.Supplier = SupplierDAO.GetSupplierById(order.SupplierId);
                    order.ItemDetails = GetItemsInPurchaseOrder(order.OrderId);
                }
            }
            return orders;
        }

        public static PurchaseOrder GetOrderDetails(string orderNumber)
        {
            PurchaseOrder order = new PurchaseOrder();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PurchaseOrder WHERE orderNumber = '" + orderNumber + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    order = new PurchaseOrder()
                    {
                        OrderId = (long)reader["orderId"],
                        OrderNumber = (string)reader["orderNumber"],
                        Status = (string)reader["status"],
                        SubmittedDate = (DateTime)reader["submittedDate"],
                        OrderDate = (DateTime)reader["orderDate"],
                        SupplierId = (long)reader["supplierId"],
                        EmployeeId = (long)reader["empId"]

                    };
                }
            }

            order.Employee = EmployeeDAO.GetEmployeeById(order.EmployeeId);
            order.Supplier = SupplierDAO.GetSupplierById(order.SupplierId);
            order.ItemDetails = GetItemsInPurchaseOrder(order.OrderId);
            return order;
        }

        public static List<PurchaseOrderDetails> GetItemsInPurchaseOrder(long orderId)
        {
            List<PurchaseOrderDetails> items = new List<PurchaseOrderDetails>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PurchaseOrderDetails WHERE orderId = '" + orderId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PurchaseOrderDetails item = new PurchaseOrderDetails()
                    {
                        OrderId = (long)reader["orderId"],
                        ItemId = (long)reader["itemId"],
                        Quantity = (int)reader["quantity"]

                    };
                    items.Add(item);
                }

                foreach (var item in items)
                {
                    item.ItemSuppliersDetails = SupplierDAO.GetItemSuppliersDetails(item.ItemId);
                    item.Item = CatalogueDAO.DisplayCatalogueDetails(item.ItemId);
                }

            }
            return items;
        }
    }
}