using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class RequisitionDetailsDAO
    {
        public static void SaveRequisitionDetails(List<RequisitionDetails> reqDetailsList)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO RequisitionDetails (reqId,itemId,quantity)" +
                            "VALUES (@reqId, @itemId, @quantity)";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@reqId", DbType.Int64);
                cmd.Parameters.AddWithValue("@itemId", DbType.Int64);
                cmd.Parameters.AddWithValue("@quantity", DbType.Int32);
                foreach (var detail in reqDetailsList)
                {
                    cmd.Parameters[0].Value = detail.Requisition.ReqId;
                    cmd.Parameters[1].Value = detail.Item.ItemId;
                    cmd.Parameters[2].Value = detail.Quantity;

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}