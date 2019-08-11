using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class AdjVoucherDAO
    {
        public static List<AdjVoucher> GetAllAdjVouchers()
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from AdjVoucher WHERE flag != 1";
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
                }
            }

            return adjVouchers;

        }

        public static void GenerateDisbursement(List<AdjVoucher> adjVouchers)
        {
            long adjId = GetLastId() ?? 1;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                adjId += 1;

                string q = @"INSERT INTO AdjVoucher (adjId,itemId,date,authorisedBy,adjQty,reason,status) " +
                           "VALUES (@adjId,@itemId,@date,@authorisedBy,@adjQty,@reason,@status)";
                           
                foreach (var adjVoucher in adjVouchers)
                {
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@adjId", adjId);
                    cmd.Parameters.AddWithValue("@itemId", adjVoucher.Item.ItemId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@authorisedBy", adjVoucher.AuthorisedBy);
                    cmd.Parameters.AddWithValue("@adjQty", adjVoucher.AdjQty);
                    cmd.Parameters.AddWithValue("@reason", adjVoucher.Reason);
                    cmd.Parameters.AddWithValue("@status", 0);
                    cmd.ExecuteNonQuery();
                }

            }

        }

        private static long? GetLastId()
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT MAX(adjId) AS COUNT FROM AdjVoucher";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return (reader["COUNT"] == DBNull.Value) ? 0 : Convert.ToInt64(reader["COUNT"]);
                }
            }

            return null;
        }

        public static void UpdateStock(List<AdjVoucher> adjVouchers)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE Inventory SET stockLevel = stockLevel + @actual WHERE itemId = @itemId";
                foreach (var adjVoucher in adjVouchers)
                {
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@itemId", adjVoucher.Item.ItemId);
                    cmd.Parameters.AddWithValue("@actual", adjVoucher.AdjQty);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}