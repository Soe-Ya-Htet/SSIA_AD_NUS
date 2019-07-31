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
        public List<Inventory> DisplayAllCatalogue()
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
                        BinNo = (string)reader["binNo"],
                        StockLevel = (int)reader["stockLevel"],
                        ReorderLevel = (int)reader["reorderLevel"],
                        ReorderQty = (int)reader["reorderQty"],
                        Category = (string)reader["category"],
                        Description = (string)reader["description"],
                        UnitOfMeasure = (string)reader["unitOfMeasure"],
                        ImageUrl = (string)reader["imageUrl"]
                    };
                    catalogues.Add(catalogue);
                }
            }
            return catalogues;
        }


        public Inventory DisplaySelectedCatalogue(int ItemId)
        {

            Inventory catalogue = new Inventory();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Inventory WHERE itemId = '" + ItemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    catalogue.ItemId = (int)reader["itemId"];
                    catalogue.ItemCode = (string)reader["itemCode"];
                    catalogue.BinNo = (string)reader["binNo"];
                    catalogue.StockLevel = (int)reader["stockLevel"];
                    catalogue.ReorderLevel = (int)reader["reorderLevel"];
                    catalogue.ReorderQty = (int)reader["reorderQty"];
                    catalogue.Category = (string)reader["category"];
                    catalogue.Description = (string)reader["description"];
                    catalogue.UnitOfMeasure = (string)reader["unitOfMeasure"];
                    catalogue.ImageUrl = (string)reader["imageUrl"];
                }
            }
            return catalogue;
        }


        public List<string> DisplayAllCategory()
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


        public List<string> DisplayAllUnit()
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


        public static void CreateCatalogue(string ItemCode, string Category, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q1 = @"SELECT MAX(itemId) from Inventory";
                SqlCommand cmd1 = new SqlCommand(q1, conn);
                int ItemId = (int)cmd1.ExecuteScalar() + 1;

                string q2 = "INSERT INTO Inventory (itemId,itemCode,category,description,unitOfMeasure,supplier1Id,supplier2Id,supplier3Id)" + "VALUES ('" + ItemId + "','" + ItemCode + "','" + Category + "','" + Description + "','" + UnitOfMeasure + "','" + Supplier1Id + "','" + Supplier2Id + "','" + Supplier3Id + "')";
                SqlCommand cmd2 = new SqlCommand(q2, conn);
                cmd2.ExecuteNonQuery();
            }
        }


        public static void UpdateCatagolue(string ItemId, string Description, string UnitOfMeasure, string Supplier1Id, string Supplier2Id, string Supplier3Id)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE Inventory SET description = '" + Description + "', unitOfMeasure = '" + UnitOfMeasure + "', supplier1Id = '" + Supplier1Id + "', supplier2Id = '" + Supplier2Id + "', supplier3Id = '" + Supplier3Id + " WHERE itemId ='" + ItemId +"'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}