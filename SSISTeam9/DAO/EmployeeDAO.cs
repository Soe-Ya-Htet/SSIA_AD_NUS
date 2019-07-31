using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class EmployeeDAO
    {
        public static Employee GetEmployeeById(long empId)
        {
            Employee employee = new Employee();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where empId = '" + empId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"]

                    };
                }
                return employee;
            }
        }
    }
}