using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class PriceListDAO
    {
        public static PriceList GetPriceListById(long ItemId)
        {

            PriceList pricelist = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PriceList WHERE itemId = '" + ItemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pricelist = new PriceList()
                    {                        
                        Supplier1Id = (long)reader["supplier1Id"],
                        Supplier2Id = (long)reader["supplier2Id"],
                        Supplier3Id = (long)reader["supplier3Id"],
                        Supplier1UnitPrice = (reader["supplier1UnitPrice"] == DBNull.Value) ? "Nil" : (string)reader["supplier1UnitPrice"],
                        Supplier2UnitPrice = (reader["supplier2UnitPrice"] == DBNull.Value) ? "Nil" : (string)reader["supplier2UnitPrice"],
                        Supplier3UnitPrice = (reader["supplier3UnitPrice"] == DBNull.Value) ? "Nil" : (string)reader["supplier3UnitPrice"]
                    };
                }
            }
            return pricelist;
        }


        public static void UpdatePriceList(long itemId, string supplierCode, int number)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE p SET p.supplier" + number + "Id = s.supplierId " +
                            "FROM PriceList p, Supplier s" +
                            "WHERE s.supplierCode = '" + supplierCode +
                            "' AND p.itemId = '" + itemId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void CreatePriceList(long itemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO PriceList (itemId)" +
                            "VALUES ('" + itemId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}