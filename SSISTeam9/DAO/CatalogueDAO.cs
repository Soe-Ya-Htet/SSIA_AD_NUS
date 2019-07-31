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
        public static List<Inventory> DisplayAllCatalogue()
        {
            List<Inventory> catalogues = new List<Inventory>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory catalogue = new Inventory()
                    {
                        ItemId = (int)reader["itemId"],
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


        public static Inventory DisplayCatalogueDetails(int ItemId)
        {

            Inventory catalogue = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory WHERE itemId = '" + ItemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    catalogue = new Inventory()
                    {
                        ItemId = (int)reader["itemId"],
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

                string q = @"SELECT * from Inventory WHERE description LIKE '%" + Description + "%'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory catalogue = new Inventory()
                    {
                        ItemId = (int)reader["itemId"],
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


        public static List<string> DisplayAllCategory()
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


        public static List<string> DisplayAllUnit()
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


        public static void DeleteCatalogue(int ItemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from Inventory where itemId = '" + ItemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


        public static void UpdateCatalogueDetails(Inventory Catalogue)
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

        public static void UpdatePriceList(Supplier supplier, int number)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

            }

        }

        //public static void CreateCatalogue(string ItemCode, string Category, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q1 = @"SELECT MAX(itemId) from Inventory";
        //        SqlCommand cmd1 = new SqlCommand(q1, conn);
        //        int ItemId = (int)cmd1.ExecuteScalar() + 1;

        //        string q2 = "INSERT INTO Inventory (itemId,itemCode,category,description,unitOfMeasure,supplier1Id,supplier2Id,supplier3Id)" + "VALUES ('" + ItemId + "','" + ItemCode + "','" + Category + "','" + Description + "','" + UnitOfMeasure + "','" + Supplier1Id + "','" + Supplier2Id + "','" + Supplier3Id + "')";
        //        SqlCommand cmd2 = new SqlCommand(q2, conn);
        //        cmd2.ExecuteNonQuery();
        //    }
        //}


        //public static void UpdateCatagolue(string ItemId, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
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