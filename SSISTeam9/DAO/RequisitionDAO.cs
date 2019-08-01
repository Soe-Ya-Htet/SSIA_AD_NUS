using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SSISTeam9.DAO
{
    public class RequisitionDAO
    {
        public static List<Requisition> GetPendingRequisitionsFromDB()
        {
            List<Requisition> requisitions = new List<Requisition>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Requisition r,Employee e where r.empId=e.empId and r.status='Pending'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Requisition requisition = new Requisition()
                    {
                        ReqId = (long)reader["reqId"],                     
                        ReqCode = (String)reader["reqCode"],
                        DateOfRequest = (DateTime)reader["dateOfRequest"],
                        Status = (String)reader["status"],
                        //PickUpDate = (DateTime)reader["pickUpDate"],
                        ApprovedBy = (reader["approvedBy"] == DBNull.Value) ? "Nil" : (string)reader["approvedBy"],
                        Employee = new Employee()
                        {
                            EmpId = (long)reader["empId"],
                            //EmpName = (String)reader["empName"]
                        }
                    };

                    requisitions.Add(requisition);
                }
            }
            return requisitions;
        }
    }
}