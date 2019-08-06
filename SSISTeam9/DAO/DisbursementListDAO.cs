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
        public static void CreateDisbursmentList(List<long> selected)
        {
            int listId;
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"INSERT INTO DisbursmentList (deptId)" +
                        "VALUES (@deptId, @reqCode, @dateOfRequest, @status)" +
                        "SELECT CAST(scope_identity() AS int)";
                var parms = selected.Select((s, i) => "@id" + i.ToString()).ToArray();
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

        public static List<DisbursementList> ViewDisbursements(string collectionPt)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"SELECT * FROM DisbursementList d, DisbursementListDetails dDets 
                            WHERE d.listId = dDets.listId AND acknowledgedBy IS NULL AND d.collectionPointId";

                return null;
        }
    }
}