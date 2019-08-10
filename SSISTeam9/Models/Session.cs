using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace SSISTeam9.Models
{
    public class Session
    {
        public static string CreateSession(string userName)
        {
            string sessionId = Guid.NewGuid().ToString();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update UserInfo set SessionID = '" + sessionId + "'where Username ='" + userName + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
            return sessionId;
        }

        public static bool IsActiveSessionId(string sessionId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Select COUNT(*) from UserInfo where sessionId = '" + sessionId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                int count = (int)cmd.ExecuteScalar();
                return (count == 1);
            }
        }

        public static void RemoveSession(string sessionId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update UserInfo set SessionID = NULL where SessionID ='" + sessionId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}