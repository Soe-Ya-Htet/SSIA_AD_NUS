using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class AnalyticsDAO
    {
        public static List<RequisitionDetails> GetRequisitionsByDept(string department)
        {
            List<RequisitionDetails> reqs = new List<RequisitionDetails>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string sub1 = @"SELECT empId from Employee e, Department d WHERE e.deptId = d.deptId AND deptName = '" + department + "'";
                string q = @"SELECT dateOfRequest,itemId,quantity from Requisition r, RequisitionDetails rd WHERE r.reqId = rd.reqId AND status = 'Completed' AND empId in (" + sub1 + ")";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionDetails req = new RequisitionDetails()
                    {
                        DateOfRequest = (DateTime)reader["dateOfRequest"],
                        Quantity = (int)reader["quantity"],
                        ItemId = (long)reader["itemId"]
                    };
                    reqs.Add(req);
                }
                return reqs;
            }
        }
    }
}