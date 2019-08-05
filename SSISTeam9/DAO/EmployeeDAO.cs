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

        public static void UpdateEmployeeRoleById(long newRep,long currentRep)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string qq = @"Update Employee Set empRole='EMPLOYEE',empDisplayRole='EMPLOYEE' where empId =" + currentRep;
                SqlCommand cmd = new SqlCommand(qq, conn);
                cmd.ExecuteNonQuery();
                string q = @"Update Employee Set empRole='REPRESENTATIVE',empDisplayRole='REPRESENTATIVE' where empId =" + newRep;
                SqlCommand cmd1 = new SqlCommand(q, conn);
                cmd1.ExecuteNonQuery();
            }
        }

        public static void UpdateEmployeeHead(long newHead, long currentHead)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string qq = @"Update Employee Set empRole='EMPLOYEE' where empId =" + currentHead;
                SqlCommand cmd = new SqlCommand(qq, conn);
                cmd.ExecuteNonQuery();
                string q = @"Update Employee Set empRole='HEAD' where empId =" + newHead;
                SqlCommand cmd1 = new SqlCommand(q, conn);
                cmd1.ExecuteNonQuery();
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
                    Department d = new Department()
                    {
                        DeptId = (long)reader["deptId"]
                    };
                    employee = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"],
                        Department = d

                    };
                    employees.Add(employee);
                }
                return employees;
            }
        }

        public static List<Employee> GetEmployeesByDepartment(long deptId)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where deptId="+deptId;
                Employee employee = null;
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department d = new Department()
                    {
                        DeptId = (long)reader["deptId"]
                    };
                    employee = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"],
                        Department = d

                    };
                    employees.Add(employee);
                } 
            }
            return employees;
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