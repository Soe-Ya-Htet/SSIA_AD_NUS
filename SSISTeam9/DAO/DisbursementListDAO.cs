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
        public static long CreateDisbursementList(DisbursementList disbursement)
        {
            int listId;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"INSERT INTO DisbursementList (deptId,collectionPointId)" +
                        "VALUES (@deptId, @collectionPointId)" +
                        "SELECT CAST(scope_identity() AS int)";



                Console.WriteLine(q);
                SqlCommand cmd = new SqlCommand(q, conn);
               
                cmd.Parameters.AddWithValue("@deptId", disbursement.Department.DeptId);
                cmd.Parameters.AddWithValue("@collectionPointId", disbursement.Department.CollectionPoint.PlacedId);
                listId = (int)cmd.ExecuteScalar();

            }
            return (long)listId;

        }

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
        

        //The following code is for ChargeBack controller
        public static DisbursementList GetDisbursementListByListId(long listId)
        {
            DisbursementList disbursementList = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from DisbursementList WHERE listId = '" + listId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    disbursementList = new DisbursementList()
                    {
                        date = (DateTime)reader["date"]
                    };
                    disbursementList.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"]
                    };
                }
            }
            return disbursementList;
        }

    }
}