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

                string q = @"SELECT * from Requisition where status='" + status[0] + "'";
                for (int i = 1; i < status.Length; i++)
                {
                    q = q + " OR status= '" + status[i] + "'";
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

        public static Requisition GetReqByReqId(long reqId)
        {
            Requisition req = null;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT * from Requisition WHERE reqId = @reqId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@reqId", reqId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee e = new Employee()
                    {
                        EmpId = (long)reader["empId"]
                    };

                    req = new Requisition()
                    {
                        ReqId = (long)reader["reqId"],
                        ReqCode = (String)reader["reqCode"],
                        DateOfRequest = (DateTime)reader["dateOfRequest"],
                        Status = (String)reader["status"],
                        //PickUpDate = (DateTime)reader["pickUpDate"],
                        ApprovedBy = (reader["approvedBy"] == DBNull.Value) ? "Nil" : (string)reader["approvedBy"],
                        Employee = e
                    };
                }
            }
            return req;
        }

        public static List<Requisition> GetRequisitionByDeptId(long deptId)
        {
            List<Requisition> requisitions = new List<Requisition>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT * from Requisition WHERE empId in (SELECT empId from Employee WHERE deptId = @deptId)";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
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

        public static List<long> GetAllRequisitions()
        {
            List<long> reqIds = new List<long>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT reqId from Requisition";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reqIds.Add((long)reader["reqId"]);
                }
            }
            return reqIds;
        }

        public static bool HavingRequisition(long empId)
        {
            int count = 0;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"select count(empId) from Cart where empId = @empId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empId", empId);
                count = (int)cmd.ExecuteScalar();
            }
            return count != 0 ? true : false;
        }

        public static List<Requisition> GetRequisitionByEmpId(long empId)
        {
            List<Requisition> requisitions = new List<Requisition>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT * from Requisition where empId = @empId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empId", empId);
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

        public static void UpdateApprovedStatusByIdList(List<long> reqIds)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"UPDATE Requisition SET status = 'Assigned'  WHERE reqId IN ({0}) AND status = 'Approved'";
                var parms = reqIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], reqIds[i]);
                }

                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdatePartiallyCompletedStatusByIdList(List<long> reqIds)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"UPDATE Requisition SET status = 'Partially Completed(Assigned)'  WHERE reqId IN ({0}) AND status = 'Partially Completed'";
                var parms = reqIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], reqIds[i]);
                }

                cmd.ExecuteNonQuery();

            }
        }

        public static List<Requisition> GetAllPastOrderReqs(int deptId)
        {
            string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status != @status ORDER BY r.reqId DESC";
            return GetAllReqs(deptId, sql);
        }

        public static List<Requisition> GetAllPendingOrderReqs(int deptId)
        {
            string sql = @"SELECT r.*, e.empName FROM Requisition r, Employee e WHERE r.empId=e.empId AND e.deptId=@deptId AND r.status = @status";
            return GetAllReqs(deptId, sql);

        }

        private static List<Requisition> GetAllReqs(int deptId, string sql)
        {
            List<Requisition> reqs = new List<Requisition>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.Parameters.AddWithValue("@status", "Pending Approval");
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee e = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        EmpName = (string)reader["empName"]
                    };

                    Requisition requisition = new Requisition()
                    {
                        ReqId = (long)reader["reqId"],
                        ReqCode = (string)reader["reqCode"],
                        DateOfRequest = (DateTime)reader["dateOfRequest"],
                        Status = (string)reader["status"],
                        //PickUpDate = (DateTime)reader["pickUpDate"],
                        ApprovedBy = (reader["approvedBy"] == DBNull.Value) ? "Nil" : (string)reader["approvedBy"],
                        Employee = e
                    };
                    reqs.Add(requisition);
                }
            }

            return reqs;
        }

    }
}