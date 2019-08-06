using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;
namespace SSISTeam9.DAO
{
    public class ChargeBackDAO
    {
        public static List<ChargeBack> GetChargeBackByDept(long deptId, int year)
        {
            List<ChargeBack> chargeBacks = new List<ChargeBack>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT deptId, YEAR(date) AS yearOfOrder, MONTH(date) AS monthOfOrder, amount from ChargeBack WHERE deptId = '" 
                            + deptId + "' AND YEAR(date) = '" + year + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ChargeBack chargeBack = new ChargeBack()
                    {
                        YearOfOrder = (int)reader["yearOfOrder"],
                        MonthOfOrder = (int)reader["monthOfOrder"],
                        DeptId = (long)reader["deptId"],
                        Amount = (int)reader["amount"]
                    };
                    chargeBacks.Add(chargeBack);
                }
            }
            return chargeBacks;
        }

        public static List<ChargeBack> GetChargeBackByMonth(int month)
        {
            List<ChargeBack> chargeBacks = new List<ChargeBack>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT deptId, YEAR(date) AS yearOfOrder, MONTH(date) AS monthOfOrder, amount from ChargeBack WHERE MONTH(date) = '" + month + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ChargeBack chargeBack = new ChargeBack()
                    {
                        YearOfOrder = (int)reader["yearOfOrder"],
                        MonthOfOrder = (int)reader["monthOfOrder"],
                        DeptId = (long)reader["deptId"],
                        Amount = (int)reader["amount"]
                    };
                    chargeBacks.Add(chargeBack);
                }
            }
            return chargeBacks;
        }


        public static ChargeBack GetDistinctChargeBack(long deptId, int month)
        {
            ChargeBack chargeBack = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT deptId, YEAR(date) AS yearOfOrder, MONTH(date) AS monthOfOrder, amount from ChargeBack WHERE deptId = '"
                            + deptId + "' AND MONTH(date) = '" + month + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    chargeBack = new ChargeBack()
                    {
                        YearOfOrder = (int)reader["yearOfOrder"],
                        MonthOfOrder = (int)reader["monthOfOrder"],
                        DeptId = (long)reader["deptId"],
                        Amount = (int)reader["amount"]
                    };
                }
            }
            return chargeBack;
        }

        public static void CreateChargeBack(ChargeBack chargeBack)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO ChargeBack (deptId,date,amount)" +
                            "VALUES ('" + chargeBack.DeptId +
                            "','" + chargeBack.YearOfOrder +
                            "-" + chargeBack.MonthOfOrder +
                            "-01'" + chargeBack.Amount + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateChargeBack(int amount, long deptId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"Update ChargeBack Set amount=" + amount + " where deptId =" + deptId;
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}