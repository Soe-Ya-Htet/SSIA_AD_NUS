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
        public List<Inventory> DisplayCatalogue()
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

        public static void DeleteCatalogue(int ItemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update UserInfo set SessionID = NULL where SessionID ='"  + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}