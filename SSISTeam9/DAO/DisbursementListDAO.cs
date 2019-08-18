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
                string q = @"INSERT INTO DisbursementList (deptId,collectionPointId,date)" +
                        "VALUES (@deptId, @collectionPointId, @date)" +
                        "SELECT CAST(scope_identity() AS int)";



                Console.WriteLine(q);
                SqlCommand cmd = new SqlCommand(q, conn);
               
                cmd.Parameters.AddWithValue("@deptId", disbursement.Department.DeptId);
                cmd.Parameters.AddWithValue("@collectionPointId", disbursement.Department.CollectionPoint.PlacedId);
                cmd.Parameters.AddWithValue("@date", disbursement.date);
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
                        date = (DateTime)reader["date"],
                        CollectionPoint = cP
                        
                    };
                    disbursementLists.Add(disbursementList);
                }


                    return disbursementLists;
            }
        }

        public static List<DisbursementList> FindAllDisbursements(long collectionPt)
        {
            List<DisbursementList> disbursementLists = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId=cP.placeId AND acknowledgedBy IS NULL AND d.deptId = dept.deptId";

                if(collectionPt > 0)
                {
                    q += " AND cP.placeId=@cpid";
                }

                SqlCommand cmd = new SqlCommand(q, conn);
                if (collectionPt > 0)
                {
                    cmd.Parameters.AddWithValue("@cpid", collectionPt);
                }

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
                        date = (DateTime) reader["date"],
                        Department = d,
                        CollectionPoint = cP
                    };
                    disbursementLists.Add(disbursementList);
                }

                return disbursementLists;
            }
        }
        public static DisbursementList GetGeneratedDisbursementListByDeptId(long deptId)
        {
            DisbursementList disbursementList = new DisbursementList();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT d.*,c.name,c.placeId,c.time from DisbursementList d,CollectionPoint c WHERE "+
                            "d.deptId = @deptId and d.acknowledgedBy IS NULL and d.collectionPointId = c.placeId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    disbursementList = new DisbursementList()
                    {
                        date = (DateTime)reader["date"],
                        ListId = (long)reader["listId"]

                    };
                    disbursementList.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"]
                    };
                    disbursementList.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"],
                        Time = (TimeSpan)reader["time"]
                    };
                }
            }
            return disbursementList;
        }

        public static void UpdateDisCollectionPoint(DisbursementList disbursement)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update DisbursementList Set collectionPointId = @cPointId where listId = @listId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@cPointId",disbursement.CollectionPoint.PlacedId);
                cmd.Parameters.AddWithValue("@listId",disbursement.ListId);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<DisbursementList> CheckForPendingDisbursements()
        {

            List<DisbursementList> disbursementLists = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT * FROM DisbursementList WHERE acknowledgedBy IS NULL";

                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    
                    DisbursementList disbursementList = new DisbursementList()
                    {
                        ListId = (long)reader["listId"]
                        
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
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"],
                        AcknowledgedBy = reader["acknowledgedBy"].ToString(),
                        
                    };
                    disbursementList.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"]
                    };
                    disbursementList.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["collectionPointId"]
                    };
                }
            }
            return disbursementList;
        }

        public static void AcknowledgeDisbursement(long listId, string empName)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update DisbursementList Set acknowledgedBy = @empName where listId = @listId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empName", empName);
                cmd.Parameters.AddWithValue("@listId", listId);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<DisbursementList> GetAllPendingDisbursementList(long deptId)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy IS NULL AND d.deptId = dept.deptId
                            AND d.deptId = @deptId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    DisbursementList disbursement = new DisbursementList
                    {
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"]
                    };

                    disbursement.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptName = (string)reader["deptName"]
                    };
                    disbursement.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"]
                    };

                    disbursements.Add(disbursement);
                }
            }
            return disbursements;

        }

        public static List<DisbursementList> GetAllPastDisbursementList(long deptId, long empId)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy=@empId AND d.deptId = dept.deptId
                            AND d.deptId = @deptId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.Parameters.AddWithValue("@empId", empId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    DisbursementList disbursement = new DisbursementList
                    {
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"]
                    };

                    disbursement.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptName = (string)reader["deptName"]
                    };
                    disbursement.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"]
                    };

                    disbursements.Add(disbursement);
                }
            }
            return disbursements;

        }

        public static List<DisbursementList> ViewAllCompletedDisbursementList()
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM DisbursementList d, CollectionPoint cP, Department dept 
                            WHERE d.collectionPointId= cP.placeId AND acknowledgedBy IS NOT NULL AND d.deptId = dept.deptId
                            ORDER BY date";
                SqlCommand cmd = new SqlCommand(q, conn);
                

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    DisbursementList disbursement = new DisbursementList
                    {
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"],
                        AcknowledgedBy = (string)reader["acknowledgedBy"]
                    };

                    disbursement.Department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptName = (string)reader["deptName"]
                    };
                    disbursement.CollectionPoint = new CollectionPoint()
                    {
                        PlacedId = (long)reader["placeId"],
                        Name = (string)reader["name"]
                    };

                    disbursements.Add(disbursement);
                }
            }
            return disbursements;

        }


        public static List<DisbursementList> ViewCompletedDisbursByDept(long deptId)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM DisbursementList
                            WHERE deptId ='" + deptId +
                            "' ORDER BY date";
                SqlCommand cmd = new SqlCommand(q, conn);


                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    DisbursementList disbursement = new DisbursementList
                    {
                        ListId = (long)reader["listId"],
                        date = (DateTime)reader["date"],
                    };

                    disbursements.Add(disbursement);
                }
            }
            return disbursements;

        }


    }
}