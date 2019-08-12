using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SSISTeam9.DAO
{
    public class DelegateDAO
    {
        public static void InsertNewDelegate(Models.Delegate d)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO Delegate (empId,deptId,fromDate,toDate)" +
                            "VALUES (@empId, @deptId, @fromDate,@toDate)";
                Console.WriteLine(q);
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empId", d.Employee.EmpId);
                cmd.Parameters.AddWithValue("@deptId", d.Department.DeptId);
                cmd.Parameters.AddWithValue("@fromDate", d.FromDate);
                cmd.Parameters.AddWithValue("@toDate", d.ToDate);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteDelegate(long deptId,long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from Delegate where empId=@empId and deptId =@deptId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@empId", empId);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                cmd.ExecuteNonQuery();
            }
        }

        public static Models.Delegate GetDelegateByDept(long deptId)
        {
            Models.Delegate d = new Models.Delegate();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Delegate where deptId =@depId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@depId", deptId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    d = new Models.Delegate()
                    {
                       FromDate=(DateTime)reader["fromDate"],
                       ToDate=(DateTime)reader["toDate"]
                    };
                }
                return d;
            }
        }

        public static Models.Delegate GetDelegateInfoByDeptId(int deptId)
        {
            Models.Delegate del = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string sql = @"SELECT * FROM Delegate WHERE deptId=@deptId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    del = new Models.Delegate
                    {
                        Employee = new Employee
                        {
                            EmpId = (long)reader["empId"]
                        },
                        Department = new Department
                        {
                            DeptId = (long)reader["deptId"]
                        },
                        FromDate = (DateTime)reader["fromDate"],
                        ToDate = (DateTime)reader["toDate"]
                    };
                }
            }
            return del;

        }

    }
}