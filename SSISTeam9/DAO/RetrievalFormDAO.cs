using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class RetrievalFormDAO
    {
        public static List<RequisitionDetails> GetEmployeesByIdList(List<long> reqIds)
        {

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where reqId IN ({0})";

                var parms = reqIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], reqIds[i]);
                }

                RequisitionDetails requisitionDetails = null;

                List<RequisitionDetails> reqDetails = new List<RequisitionDetails>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Requisition r = new Requisition()
                    {
                        ReqId = (long)reader["reqId"]
                    };
                    Inventory i = new Inventory()
                    {
                        ItemId = (long)reader["itemId"]
                    };
                    requisitionDetails = new RequisitionDetails()
                    {
                        Quantity = (int)reader["quantity"],
                        Item = i,
                        Requisition = r

                    };
                    reqDetails.Add(requisitionDetails);
                }
                return reqDetails;
            }
        }
    }
}