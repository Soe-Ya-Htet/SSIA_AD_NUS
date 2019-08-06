using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class DisbursementListDAO
    {
        //    public static void CreateDisbursmentList(List<long> selected)
        //    {
        //        int listId;
        //        using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //        {
        //            conn.Open();
        //            string q = @"INSERT INTO DisbursmentList (deptId)" +
        //                    "VALUES (@deptId, @reqCode, @dateOfRequest, @status)" +
        //                    "SELECT CAST(scope_identity() AS int)";
        //            var parms = selected.Select((s, i) => "@id" + i.ToString()).ToArray();
        //            var inclause = string.Join(",", parms);

        //            string sql = string.Format(q, inclause);
        //            Console.Write(sql);

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            for (var i = 0; i < parms.Length; i++)
        //            {
        //                cmd.Parameters.AddWithValue(parms[i], reqIds[i]);
        //            }

        //            cmd.ExecuteNonQuery();
        //        }

        //    }

        public static List<DisbursementList> ViewDisbursements(string collectionPt)
        {
            List<DisbursementList> disbursementLists = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q;
                
                if (collectionPt == null)
                {
                    q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy IS NULL AND d.deptId = dept.deptId
                            AND cP.placeId = '1'";
                }
                else
                {
                    q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy IS NULL AND d.deptId = dept.deptId
                            AND cP.placeId ='" + collectionPt + "'";
                }

                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department d = new Department()
                    {
                        DeptName = (string)reader["deptName"]
                    };
                    CollectionPoint cP = new CollectionPoint()
                    {
                        Name = (string)reader["name"]
                    };
                    DisbursementList disbursementList = new DisbursementList()
                    {
                        ListId = (long)reader["listId"],
                        Department = d,
                        CollectionPoint = cP
                    };
                    disbursementLists.Add(disbursementList);
                }


                    return disbursementLists;
            }
        }
    }
}