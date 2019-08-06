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
                        Supplier1UnitPrice = (string)reader["supplier1UnitPrice"],
                        Supplier2UnitPrice = (string)reader["supplier2UnitPrice"],
                        Supplier3UnitPrice = (string)reader["supplier3UnitPrice"]
                    };
                }
            }
            return pricelist;
        }


        public static void UpdatePriceList(PriceList priceList)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE PriceList SET supplier1Id = '" + priceList.Supplier1Id +
                    "', supplier2Id = '" + priceList.Supplier2Id +
                    "', supplier3Id = '" + priceList.Supplier3Id +
                    "', supplier1UnitPrice = '" + priceList.Supplier1UnitPrice +
                    "', supplier2UnitPrice = '" + priceList.Supplier2UnitPrice +
                    "', supplier3UnitPrice = '" + priceList.Supplier3UnitPrice +
                    "' WHERE itemId = '" + priceList.Item.ItemId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void CreatePriceList(PriceList priceList)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO PriceList (itemId,supplier1Id,supplier2Id,supplier3Id,supplier1UnitPrice,supplier2UnitPrice,supplier3UnitPrice)" +
                            "VALUES ('" + priceList.Item.ItemId +
                            "','" + priceList.Supplier1Id +
                            "','" + priceList.Supplier2Id +
                            "','" + priceList.Supplier3Id + 
                            "','" + priceList.Supplier1UnitPrice +
                            "','" + priceList.Supplier2UnitPrice +
                            "','" + priceList.Supplier3UnitPrice + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


        public static void DeletePriceList(long ItemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from PriceList where itemId = '" + ItemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


    }
}