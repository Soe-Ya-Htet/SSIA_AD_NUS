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
        public static List<Requisition> GetRequisitionsByStatuses(params string[] status)
        {
            List<Requisition> requisitions = new List<Requisition>();
            

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Requisition where status='"+status[0]+"'";
                for (int i = 1; i < status.Length; i++)
                {
                    q = q + " OR status= '" + status[i]+"'";
                }
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee e = new Employee()
                    {
                        EmpId = (long)reader["empId"]
                    };

                    Requisition requisition = new Requisition()
                    {
                        ReqId = (long)reader["reqId"],
                        ReqCode = (String)reader["reqCode"],
                        DateOfRequest = (DateTime)reader["dateOfRequest"],
                        Status = (String)reader["status"],
                        //PickUpDate = (DateTime)reader["pickUpDate"],
                        ApprovedBy = (reader["approvedBy"] == DBNull.Value) ? "Nil" : (string)reader["approvedBy"],
                        Employee = e
                    };


                    requisitions.Add(requisition);
                }
            }
            return requisitions;
        }

        public static List<RequisitionDetails> GetRequisitionDetailsByReqId(long reqId)
        {
            List<RequisitionDetails> reqDetails = new List<RequisitionDetails>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from RequisitionDetails where reqId=" + reqId;
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Inventory i = new Inventory()
                    {
                        ItemId = (long)reader["itemId"]
                    };
                    RequisitionDetails requisitionDetail = new RequisitionDetails()
                    {
                        Quantity = (int)reader["quantity"],
                        Item = i
                    };

                    reqDetails.Add(requisitionDetail);
                }
            }
            return reqDetails;

        }

        public static long SaveRequisition(Requisition req)
        {
            int reqId;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO Requisition (empId,reqCode,dateOfRequest,status)" +
                            "VALUES (@empId, @reqCode, @dateOfRequest, @status)"+
                            "SELECT CAST(scope_identity() AS int)";
                Console.WriteLine(q);
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empId", req.Employee.EmpId);
                cmd.Parameters.AddWithValue("@reqCode", req.ReqCode);
                cmd.Parameters.AddWithValue("@dateOfRequest", req.DateOfRequest);
                cmd.Parameters.AddWithValue("@status", req.Status);
                reqId = (int)cmd.ExecuteScalar();
                
            }
            return (long)reqId;
        }

        public static void UpdateRequisitionStatus(long reqId, string status, long currentHead)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                    string q = @"UPDATE Requisition SET status = '" + status + "'" + ",approvedBy=" + currentHead + " WHERE reqId =" + reqId;
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

    }
}