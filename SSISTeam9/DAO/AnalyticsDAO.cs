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

                string sub1 = @"SELECT empId from Employee e, Department d WHERE e.deptId = d.deptId AND deptName = '" + department + "') GROUP BY MONTH(dateOfRequest),YEAR(dateOfRequest)";
                string q = @"SELECT MONTH(dateOfRequest) as monthOfRequest, YEAR(dateOfRequest) as yearOfRequest, SUM(quantity) as total from Requisition r, RequisitionDetails rd WHERE r.reqId = rd.reqId AND status = 'Completed' AND empId in (" + sub1;
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionDetails req = new RequisitionDetails()
                    {
                        MonthOfRequest = (int)reader["monthOfRequest"],
                        YearOfRequest = (int)reader["yearOfRequest"],
                        Quantity = (int)reader["total"],
                    };
                    reqs.Add(req);
                }
                return reqs;
            }
        }

        public static List<RequisitionDetails> GetRequisitionsByStationeryCategory()
        {
            List<RequisitionDetails> reqs = new List<RequisitionDetails>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                
                string q = @"SELECT MONTH(dateOfRequest) as monthOfRequest, YEAR(dateofRequest) as yearOfRequest, SUM(quantity) as total, category from Requisition r, RequisitionDetails rd, Inventory i WHERE r.reqId = rd.reqId AND status = 'Completed' AND rd.itemId = i.itemId GROUP BY MONTH(dateOfRequest),YEAR(dateOfRequest),category";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RequisitionDetails req = new RequisitionDetails()
                    {
                        MonthOfRequest = (int)reader["monthOfRequest"],
                        YearOfRequest = (int)reader["yearOfRequest"],
                        Quantity = (int)reader["total"],
                        Category = (string)reader["category"]
                    };
                    reqs.Add(req);
                }
                return reqs;
            }
        }
    }
}