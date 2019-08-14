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
        //public static List<AdjVoucher> GetAllAdjVouchers()
        //{
        //    List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q = @"SELECT * from AdjVoucher WHERE flag != 1";
        //        SqlCommand cmd = new SqlCommand(q, conn);

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Inventory catalogue = new Inventory()
        //            {
        //                ItemId = (long)reader["itemId"],
        //                ItemCode = (string)reader["itemCode"],
        //                BinNo = (reader["binNo"] == DBNull.Value) ? "Nil" : (string)reader["binNo"],
        //                StockLevel = (int)reader["stockLevel"],
        //                ReorderLevel = (int)reader["reorderLevel"],
        //                ReorderQty = (int)reader["reorderQty"],
        //                Category = (string)reader["category"],
        //                Description = (string)reader["description"],
        //                UnitOfMeasure = (string)reader["unitOfMeasure"],
        //                ImageUrl = (reader["imageUrl"] == DBNull.Value) ? "Nil" : (string)reader["imageUrl"]
        //            };
        //        }
        //    }

        //    return adjVouchers;

        //}

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
                    cmd.Parameters.AddWithValue("@itemId", adjVoucher.ItemId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@authorisedBy", adjVoucher.AuthorisedBy);
                    cmd.Parameters.AddWithValue("@adjQty", adjVoucher.AdjQty);
                    cmd.Parameters.AddWithValue("@reason", adjVoucher.Reason);
                    cmd.Parameters.AddWithValue("@status", 0);
                    cmd.ExecuteNonQuery();
                }

            }

        }

        public static long? GetLastId()
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
                    cmd.Parameters.AddWithValue("@itemId", adjVoucher.ItemId);
                    cmd.Parameters.AddWithValue("@actual", adjVoucher.AdjQty);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //The following method is used to create AdjVoucher after stock taking
        public static void CreateAdjVoucher(long adjId, long itemId, int qty)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;

                string q = @"INSERT INTO AdjVoucher (adjId,itemId,date,authorisedBy,adjQty)" +
                            "VALUES ('" + adjId + "','" + itemId +
                            "','" + date +
                            "','0','" + qty + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<AdjVoucher> GetAdjByStatus(int status)
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from AdjVoucher WHERE status = '" + status + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdjVoucher adjVoucher = new AdjVoucher()
                    {
                        AdjId = (long)reader["adjId"],
                        ItemId = (long)reader["itemId"],
                        Date = (DateTime)reader["date"],
                        AuthorisedBy = (long)reader["authorisedBy"],
                        AdjQty = (int)reader["adjQty"],
                        Reason = (reader["reason"] == DBNull.Value) ? " " : (string)reader["reason"],
                        status = (int)reader["status"]
                    };
                    adjVouchers.Add(adjVoucher);
                }
            }

            return adjVouchers;
        }


        public static List<AdjVoucher> GetAdjByAdjId(long adjId)
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from AdjVoucher WHERE adjId = '" + adjId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AdjVoucher adjVoucher = new AdjVoucher()
                    {
                        AdjId = (long)reader["adjId"],
                        ItemId = (long)reader["itemId"],
                        Date = (DateTime)reader["date"],
                        AuthorisedBy = (long)reader["authorisedBy"],
                        AdjQty = (int)reader["adjQty"],
                        Reason = (reader["reason"] == DBNull.Value) ? " " : (string)reader["reason"],
                        status = (int)reader["status"]
                    };
                    adjVouchers.Add(adjVoucher);
                }
            }

            return adjVouchers;
        }


        public static void UpdateReason(AdjVoucher adjVoucher)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE AdjVoucher SET reason = '" + adjVoucher.Reason + 
                             "' WHERE itemId = '" + adjVoucher.ItemId + "' AND adjId = '" + adjVoucher.AdjId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateStatus(long adjId, int status)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE AdjVoucher SET status = '" + status +
                             "' WHERE adjId = '" + adjId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void AuthoriseBy(long adjId, long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE AdjVoucher SET authorisedBy = '" + empId + "', status = '1'" +
                             " WHERE adjId = '" + adjId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}