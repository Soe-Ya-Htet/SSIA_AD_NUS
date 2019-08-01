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

        public static List<Employee> GetEmployeesByIdList(List<long> empIds)
        {

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where empId IN ({0})";

                var parms = empIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for(var i=0; i<parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], empIds[i]);
                }

                Employee employee = null;

                List<Employee> employees = new List<Employee>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
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
                    employees.Add(employee);
                }
                return employees;
            }
        }

        public static List<string> GetAllEmployeeNames()
        {
            List<string> employeeNames = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT empName from Employee";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string employeeName = (string)reader["empName"];
                    employeeNames.Add(employeeName);
                }
            }
            return employeeNames;
        }

    }
}