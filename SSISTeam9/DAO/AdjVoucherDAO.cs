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
        public static void InsertReason(List<AdjVoucher> adjVouchers, String authPerson)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO AdjVoucher (itemId,date,authorisedBy,adjQty,reason)" +
                           "VALUES (@itemId, @date, @authorisedBy,@adjQty,@reason)";
                SqlCommand cmd = new SqlCommand(q, conn);
                foreach (var voucher in adjVouchers)
                {
                    cmd.Parameters.AddWithValue("@itemId", voucher.Item.ItemId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@authorisedBy", authPerson);
                    cmd.Parameters.AddWithValue("@adjQty", voucher.AdjQty);
                    cmd.Parameters.AddWithValue("@reason", voucher.Reason);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}