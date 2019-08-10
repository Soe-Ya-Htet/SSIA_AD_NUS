using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class StockDAO
    {
        public static void UpdateInventoryStock(Dictionary<long, int> itemAndNewStock)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                foreach (KeyValuePair<long, int> item in itemAndNewStock)
                {
                    string q = @"UPDATE Inventory SET stockLevel = '" + item.Value + "' WHERE itemId = '" + item.Key + "'";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static List<Inventory> GetAllItemsOrdered()
        {
            List<Inventory> items = new List<Inventory>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory ORDER BY (stockLevel - reorderLevel)";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory item = new Inventory()
                    {
                        ItemId = (long)reader["itemId"],
                        ItemCode = (string)reader["itemCode"],
                        BinNo = (reader["binNo"] == DBNull.Value) ? "Nil" : (string)reader["binNo"],
                        StockLevel = (int)reader["stockLevel"],
                        ReorderLevel = (int)reader["reorderLevel"],
                        ReorderQty = (int)reader["reorderQty"],
                        Category = (string)reader["category"],
                        Description = (string)reader["description"],
                        UnitOfMeasure = (string)reader["unitOfMeasure"],
                        ImageUrl = (reader["imageUrl"] == DBNull.Value) ? "Nil" : (string)reader["imageUrl"]
                    };
                    items.Add(item);
                }
            }
            return items;
        }

        public static List<Inventory> GetPendingOrderQuantities(List<Inventory> items)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                foreach(var item in items)
                {
                    string sub = @" (SELECT orderId from PurchaseOrder WHERE status IN ('Pending Supplier Confirmation','Pending Delivery'))";
                    string q = @"SELECT SUM(quantity) from PurchaseOrderDetails WHERE orderId in" + sub + "and itemId = '" + item.ItemId + "' GROUP BY itemId";

                    SqlCommand cmd = new SqlCommand(q, conn);

                    if (cmd.ExecuteScalar() == null)
                    {
                        item.PendingOrderQuantity = 0;
                    }
                    else
                    {
                        item.PendingOrderQuantity = (int)cmd.ExecuteScalar();
                    }
                }
                return items;
            }
        }

        public static List<long> GetItemsFirstSupplierIds(List<long> itemIds)
        {
            List<long> itemsFirstSupplierIds = new List<long>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                
                string q = null;

                foreach (var itemId in itemIds)
                {
                    q = @"SELECT supplier1Id from PriceList WHERE itemId = '" + itemId + "'";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    itemsFirstSupplierIds.Add((long)cmd.ExecuteScalar());

                }
                return itemsFirstSupplierIds;
            }
        }

        public static long GetItemId(string itemCode)
        {
            long itemId = 0;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT itemId from Inventory WHERE itemCode = '" + itemCode + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                itemId = (long)cmd.ExecuteScalar();
            }
            return itemId;
        }

        public static void CreatePurchaseOrder(long supplierId, List<Tuple<long, int>> itemsQuantities, string newOrderNumber, long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = null;
                
                //Set rule for deliveryBy as one week from submitted/order date
                q = "INSERT INTO PurchaseOrder (supplierId,empId,orderNumber,status,submittedDate,orderDate,deliverTo,deliverBy)"
                            + "VALUES ('" + supplierId + "','" + empId + "','" + newOrderNumber + "','" + "Pending Supplier Confirmation"
                            + "','" + DateTime.Now.Date + "','" + DateTime.Now.Date + "','" + "Logic University 29 Heng Mui Keng Terrace Singapore 123456" + "','"
                            + DateTime.Now.AddDays(7).Date + "')";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();

                q = @"SELECT orderId from PurchaseOrder where orderNumber = '" + newOrderNumber + "'";
                cmd = new SqlCommand(q, conn);
                long orderId = (long)cmd.ExecuteScalar();

                foreach (var c in itemsQuantities)
                {
                    q = "INSERT INTO PurchaseOrderDetails (orderId,itemId,quantity)"
                      + "VALUES ('" + orderId + "','"
                      + c.Item1 + "','"
                      + c.Item2 + "')";
                    cmd = new SqlCommand(q, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        //The following method is used to create AdjVoucher after stock taking
        public static void CreateAdjVoucher(long itemId, int qty)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                string q = @"INSERT INTO AdjVoucher (itemId,date,authorisedBy,adjQty)" +
                            "VALUES ('" + itemId +
                            "','" + date +
                            "','0','" + qty + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}