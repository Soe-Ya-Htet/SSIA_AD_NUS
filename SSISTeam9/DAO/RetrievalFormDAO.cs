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
        public static List<RetrievalForm> GetItemAndQuantity()
        {

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT i.binNo, i.description, r.itemId, i.stockLevel, SUM(quantity) as needed from RequisitionDetails r, Inventory i, Requisition req 
                            where req.status IN ('Assigned','Partially Completed(Assigned)') AND  r.itemId=i.itemId AND r.reqId=req.reqid 
                            GROUP BY r.itemId, binNo, description, i.stockLevel 
                            ORDER BY binNo";



                SqlCommand cmd = new SqlCommand(q, conn);
               

                RetrievalForm retrievalForm = null;

                List<RetrievalForm> retForms = new List<RetrievalForm>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                { 
                   
                    retrievalForm = new RetrievalForm()
                    {
                        itemId = (long)reader["itemId"],
                        stockLevel = (int)reader["stockLevel"],
                        binNo = (string)reader["binNo"],
                        description = (string)reader["description"],
                        totalNeeded = (int)reader["needed"] 

                    };
                    retForms.Add(retrievalForm);
                }
                return retForms;
            }

            
        }
        public static List<DeptNeeds> GetDeptNeeds(long itemId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT SUM(quantity) as needed, d.deptCode, d.deptId 
                            FROM RequisitionDetails r, Inventory i, Requisition req, Employee e, Department d 
                            WHERE /*reqId IN ({0})*/ req.status IN ('Assigned', 'Partially Completed(Assigned)') AND  r.itemId='" +itemId + "' AND r.itemId=i.itemId AND r.reqId=req.reqId AND req.empId=e.empId AND e.deptId=d.deptId " +
                            "GROUP BY r.itemId, d.deptCode, d.deptId " +
                            "ORDER BY d.deptCode";

                
                SqlCommand cmd = new SqlCommand(q, conn);
                

                DeptNeeds deptNeeds = null;

                List<DeptNeeds> deptNeedsList = new List<DeptNeeds>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    deptNeeds = new DeptNeeds()
                    {
                        deptCode = (string)reader["deptCode"],
                        deptNeeded = (int)reader["needed"],
                        deptId = (long)reader["deptId"]
                    };
                    deptNeedsList.Add(deptNeeds);
                }
                return deptNeedsList;
            }
            

                
            
        }
    }
}