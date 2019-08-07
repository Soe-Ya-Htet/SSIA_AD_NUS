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
                        DeliverTo = (string)reader["deliverTo"],
                        DeliverBy = (DateTime)reader["deliverBy"],
                        SupplierId = (long)reader["supplierId"],
                        EmployeeId = (long)reader["empId"]

                    };
                    orders.Add(order);
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
                        DeliverTo = (string)reader["deliverTo"],
                        DeliverBy = (DateTime)reader["deliverBy"],
                        SupplierId = (long)reader["supplierId"],
                        EmployeeId = (long)reader["empId"]

                    };
                }
            }

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

            }
            foreach (var item in items)
            {
                item.Item = new Inventory();
                item.Item.ItemSuppliersDetails = new PriceList();
            }
            return items;
        }

        public static void DeleteItemFromPurchaseOrder(long orderId, long itemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from PurchaseOrderDetails WHERE orderId = '" + orderId + "' AND itemId = '" + itemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void ClosePurchaseOrder(string orderNumber)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE PurchaseOrder SET STATUS = 'Closed'  WHERE orderNumber = '" + orderNumber + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeletePurchaseOrder(long orderId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from PurchaseOrder WHERE orderID = '" + orderId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteAllPurchaseOrderDetails(long orderId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from PurchaseOrderDetails WHERE orderID = '" + orderId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdatePurchaseOrderDeliveryDetails (long orderId, string deliverTo, DateTime deliverBy)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE PurchaseOrder SET deliverTo = '" + deliverTo +
                            "', deliverBy = '" + deliverBy +
                           "' WHERE orderId  = '" + orderId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdatePurchaseOrderItemQuantity(long orderId, long itemId, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE PurchaseOrderDetails SET quantity = '" + quantity +
                           "' WHERE itemId = '" + itemId + "' AND orderId = '" + orderId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static string CreatePurchaseOrderAfterChangeSuppliers(PurchaseOrder order, long altSupplierId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                
                string getMaxOrderNumber = @"SELECT MAX(orderNumber) from PurchaseOrder";
                SqlCommand cmd = new SqlCommand(getMaxOrderNumber, conn);
                long newOrderNumber = long.Parse((string)cmd.ExecuteScalar()) + 1;

                string q = "INSERT INTO PurchaseOrder (supplierId,empId,orderNumber,status,submittedDate,orderDate,deliverTo,deliverBy)"
                                + "VALUES ('" + altSupplierId + "','" + order.EmployeeId + "','" + newOrderNumber + "','" + "Pending Delivery"
                                + "','" + DateTime.Now.Date + "','" + DateTime.Now.Date + "','" + order.DeliverTo + "','"
                                + order.DeliverBy + "')"; 

                cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();

                return newOrderNumber.ToString();
            }
        }

        public static string GetNewOrderNumber()
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string getMaxOrderNumber = @"SELECT MAX(orderNumber) from PurchaseOrder";
                SqlCommand cmd = new SqlCommand(getMaxOrderNumber, conn);
                long newOrderNumber = long.Parse((string)cmd.ExecuteScalar()) + 1;

                return newOrderNumber.ToString();
            }
        }

        public static void CreatePurchaseOrderDetailsAfterChangeSuppliers(long orderId, long itemId, int quantity)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = "INSERT INTO PurchaseOrderDetails (orderId,itemId,quantity)"
                            + "VALUES ('" + orderId + "','" 
                            + itemId + "','"
                            + quantity + "')"; 

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }
        
        public static string GetQuantityByOrderIdAndItemId(long orderId, long itemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT quantity FROM PurchaseOrderDetails WHERE orderId = '" + orderId + "' AND itemId = '" + itemId + "'";
                         
                SqlCommand cmd = new SqlCommand(q, conn);
                return (string) cmd.ExecuteScalar();
            }
        }

        public static void ConfirmOrder(string orderNumber)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE PurchaseOrder SET STATUS = 'Pending Delivery' WHERE orderNumber = '" + orderNumber + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static PurchaseOrder GetOrderById(long orderId)
        {
            PurchaseOrder order = new PurchaseOrder();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PurchaseOrder WHERE orderId = '" + orderId + "'";
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
                        DeliverTo = (string)reader["deliverTo"],
                        DeliverBy = (DateTime)reader["deliverBy"],
                        SupplierId = (long)reader["supplierId"],
                        EmployeeId = (long)reader["empId"]

                    };
                }
            }

            return order;
        }

    }
}