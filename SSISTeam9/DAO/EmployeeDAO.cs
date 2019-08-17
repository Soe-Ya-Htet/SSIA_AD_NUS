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
        public static Employee GetUserPassword(string userName)
        {
            Employee employee = new Employee();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where userName = '" + userName + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        DeptId = (long)reader["deptId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"],
                        Email = (reader["email"] == DBNull.Value) ? null : (string)reader["email"],
                        SessionId = (reader["email"] == DBNull.Value) ? null : (string)reader["sessionId"]
                    };
                }
                return employee;
            }
        }

        public static string CreateSession(string userName)
        {
            string sessionId = Guid.NewGuid().ToString();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string q = @"Update Employee set sessionId = '" + sessionId + "'where userName ='" + userName + "'";
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
                string q = @"Select COUNT(*) from Employee where sessionId = '" + sessionId + "'";
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
                string q = @"Update Employee set sessionId = NULL where sessionId ='" + sessionId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static Employee GetUserBySessionId(string sessionId)
        {
            Employee employee = new Employee();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where sessionId = '" + sessionId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        DeptId = (long)reader["deptId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"],
                        Email = (reader["email"] == DBNull.Value) ? null : (string)reader["email"],
                        SessionId = (reader["email"] == DBNull.Value) ? null : (string)reader["sessionId"]
                    };
                }
                return employee;
            }
        }

        public static Employee GetDeptHeadByDeptId(long deptId)
        {
            Employee employee = new Employee();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where empRole = 'HEAD' and deptId = @deptId";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@deptId", deptId);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employee = new Employee()
                    {
                        Email = (reader["email"] == DBNull.Value) ? null : (string)reader["email"],
                    };
                }
                return employee;
            }
        }

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
                        DeptId = (long)reader["deptId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"],
                        Email = (reader["email"] == DBNull.Value) ? null : (string)reader["email"],
                        SessionId = (reader["sessionId"] == DBNull.Value) ? null : (string)reader["sessionId"]

                    };
                }
                return employee;
            }
        }


        public static List<Employee> GetEmployeeByRole(string role)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Employee where empRole = '" + role + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employee employee = new Employee()
                    {
                        EmpId = (long)reader["empId"],
                        DeptId = (long)reader["deptId"],
                        EmpName = (string)reader["empName"],
                        EmpRole = (string)reader["empRole"],
                        EmpDisplayRole = (string)reader["empDisplayRole"],
                        UserName = (string)reader["userName"],
                        Password = (string)reader["password"],
                        Email = (reader["email"] == DBNull.Value) ? null : (string)reader["email"],
                        SessionId = (reader["email"] == DBNull.Value) ? null : (string)reader["sessionId"]

                    };
                    employees.Add(employee);
                }
                return employees;
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

        public static void ChangeEmployeeRoles(long deptId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string qq = @"Update Employee Set empRole='HEAD' where empDisplayRole='HEAD' and empRole='EMPLOYEE'";
                SqlCommand cmd = new SqlCommand(qq, conn);
                cmd.ExecuteNonQuery();
                string q = @"Update Employee Set empRole='EMPLOYEE' where empDisplayRole='EMPLOYEE' and empRole='HEAD'";
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

        public static string GetUserEmail (long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT email from Employee where empId = '" + empId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                string email = "team9rockz@gmail.com";
                if (reader.Read())
                {
                    email = (string)reader["email"];
                }
                
                return email;
            }
        }
    }
}