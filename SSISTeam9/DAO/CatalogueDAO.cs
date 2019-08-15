using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class CatalogueDAO
    {
        public static List<Inventory> GetAllCatalogue()
        {
            List<Inventory> catalogues = new List<Inventory>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory WHERE flag != 1 ORDER BY itemCode";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory catalogue = new Inventory()
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
                    catalogues.Add(catalogue);
                }
            }
            return catalogues;
        }

        public static List<Inventory> GetCatalogueByCategory(string category)
        {
            List<Inventory> catalogues = new List<Inventory>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory WHERE category = @category and flag != 1";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@category", category);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory catalogue = new Inventory()
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
                    catalogues.Add(catalogue);
                }
            }
            return catalogues;
        }

        public static List<Inventory> GetCataloguesByIdList(List<long> itemIds)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory where itemId IN ({0}) AND flag != 1";

                var parms = itemIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], itemIds[i]);
                }

                Inventory item = null;

                List<Inventory> items = new List<Inventory>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new Inventory()
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
                return items;
            }
        }

        public static Inventory GetCatalogueById(long ItemId)
        {

            Inventory catalogue = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory WHERE itemId = '" + ItemId + "' AND flag != 1";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    catalogue = new Inventory()
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
                }
            }
            return catalogue;
        }


        public static List<Inventory> SearchCatalogue(string Description)
        {
            List<Inventory> catalogues = new List<Inventory>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory WHERE description LIKE '%" + Description + "%' AND flag != 1";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory catalogue = new Inventory()
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
                    catalogues.Add(catalogue);
                }
            }
            return catalogues;
        }


        public static List<string> GetAllCategories()
        {
            List<string> categories = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT category from Inventory";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string category = (string)reader["category"];
                    categories.Add(category);
                }
            }
            return categories;
        }


        public static List<string> GetAllUnits()
        {
            List<string> units = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT unitOfMeasure from Inventory";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string unit = (string)reader["unitOfMeasure"];
                    units.Add(unit);
                }
            }
            return units;
        }

        public static List<string> GetAllItemCodes()
        {
            List<string> itemCodes = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT itemCode from Inventory";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string itemCode = (string)reader["unitOfMeasure"];
                    itemCodes.Add(itemCode);
                }
            }
            return itemCodes;
        }


        public static void DeleteCatalogue(long ItemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                //Update flag = 1 to indicate deleted item
                string q = @"UPDATE Inventory SET flag = 1 where itemId = '" + ItemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


        public static void UpdateCatalogue(Inventory Catalogue)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE Inventory SET category = '" + Catalogue.Category +
                    "', description = '" + Catalogue.Description +
                    "', unitOfMeasure = '" + Catalogue.UnitOfMeasure +
                    "' WHERE itemId = '" + Catalogue.ItemId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static long CreateCatalogue(Inventory Catalogue)
        {
            int itemId;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO Inventory (itemCode,category,description,unitOfMeasure,stockLevel,reorderLevel,reorderQty,flag)" +
                            "VALUES ('" + Catalogue.ItemCode +
                            "','" + Catalogue.Category +
                            "','" + Catalogue.Description +
                            "','" + Catalogue.UnitOfMeasure+ "','0','0','0','0')" +
                            "SELECT CAST(scope_identity() AS int)";

                SqlCommand cmd = new SqlCommand(q, conn);
                itemId = (int)cmd.ExecuteScalar();
            }
            return (long)itemId;
        }

        

        public static List<Inventory> GetInventoriesByIdList(List<long> inventoryIds)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory where itemId IN ({0})";

                var parms = inventoryIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], inventoryIds[i]);
                }

                Inventory item = null;

                List<Inventory> items = new List<Inventory>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new Inventory()
                    {
                        ItemId = (long)reader["itemId"],
                        ItemCode = (string)reader["itemCode"],
                        BinNo = (string)reader["binNo"],
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
                return items;
            }
        }
        

        //public static void CreateCatalogue(string ItemCode, string Category, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q1 = @"SELECT MAX(itemId) from Inventory";
        //        SqlCommand cmd1 = new SqlCommand(q1, conn);
        //        long ItemId = (int)cmd1.ExecuteScalar() + 1;

        //        string q2 = "INSERT INTO Inventory (itemId,itemCode,category,description,unitOfMeasure,supplier1Id,supplier2Id,supplier3Id)" + "VALUES ('" + ItemId + "','" + ItemCode + "','" + Category + "','" + Description + "','" + UnitOfMeasure + "','" + Supplier1Id + "','" + Supplier2Id + "','" + Supplier3Id + "')";
        //        SqlCommand cmd2 = new SqlCommand(q2, conn);
        //        cmd2.ExecuteNonQuery();
        //    }
        //}


        //public static void UpdateCatagolue(string ItemCode, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q = @"UPDATE Inventory SET description = '" + Description + "', unitOfMeasure = '" + UnitOfMeasure + "', supplier1Id = '" + Supplier1Id + "', supplier2Id = '" + Supplier2Id + "', supplier3Id = '" + Supplier3Id + " WHERE itemId ='" + ItemId +"'";
        //        SqlCommand cmd = new SqlCommand(q, conn);
        //        cmd.ExecuteNonQuery();
        //    }
        //}
    }
}