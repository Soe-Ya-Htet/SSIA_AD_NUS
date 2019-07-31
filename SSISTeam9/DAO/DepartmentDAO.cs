using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class DepartmentDAO
    {
        public static List<Department> DisplayAllDepartment()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM Department d
                            LEFT JOIN Employee e ON d.representativeId = e.empId
                            LEFT JOIN CollectionPoint c ON d.collectionPointId = c.placeId";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department department = new Department()
                    {
                        DeptId = (int)reader["deptId"],
                        DeptCode = (string)reader["deptCode"],
                        Name = (string)reader["name"],
                        Contact = (reader["contact"] == DBNull.Value) ? "Nil" : (string)reader["contact"],
                        Telephone = (string)reader["telephone"],
                        Fax = (reader["fax"] == DBNull.Value) ? "Nil" : (string)reader["fax"],
                        Head = (reader["head"] == DBNull.Value) ? "Nil" : (string)reader["head"]
                    };
                    department.Representative.EmpName = (reader["empName"] == DBNull.Value) ? "Nil" : (string)reader["empName"];
                    department.CollectionPoint.Name = (reader["name"] == DBNull.Value) ? "Nil" : (string)reader["name"];
                    departments.Add(department);
                }
            }
            return departments;
        }


        public static Department DisplayDepartmentDetails(int DeptId)
        {
            Department department = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * FROM Department d
                            LEFT JOIN Employee e ON d.representativeId = e.empId
                            LEFT JOIN CollectionPoint c ON d.collectionPointId = c.placeId 
                            WHERE d.deptId = '" + DeptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    department = new Department()
                    {
                        DeptId = (int)reader["deptId"],
                        DeptCode = (string)reader["deptCode"],
                        Name = (string)reader["name"],
                        Contact = (reader["contact"] == DBNull.Value) ? "Nil" : (string)reader["contact"],
                        Telephone = (string)reader["telephone"],
                        Fax = (reader["fax"] == DBNull.Value) ? "Nil" : (string)reader["fax"],
                        Head = (reader["head"] == DBNull.Value) ? "Nil" : (string)reader["head"]
                    };
                    department.Representative.EmpName = (reader["empName"] == DBNull.Value) ? "Nil" : (string)reader["empName"];
                    department.CollectionPoint.Name = (reader["name"] == DBNull.Value) ? "Nil" : (string)reader["name"];
                }
                return department;
            }
        }


        public static void DeleteDepartment(int DeptId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from Department where deptId = '" + DeptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


        public static void CreateDepartment(Department department)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"INSERT INTO department (deptCode,name,contact,telephone,fax,head)" + 
                            "VALUES ('" +  department.DeptCode +
                            "','" + department.Name +
                            "','" + department.Contact +
                            "','" + department.Telephone +
                            "','" + department.Fax +
                            "','" + department.Head + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        //public static void CreateDepartment(string DeptCode, string Name, string Contact, string Telephone, string Fax, string Head)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q1 = @"SELECT MAX(deptId) from Department";
        //        SqlCommand cmd1 = new SqlCommand(q1, conn);
        //        int DeptId = (int)cmd1.ExecuteScalar() + 1;

        //        string q2 = "INSERT INTO Department (deptId,deptCode,name,contact,telephone,fax,head)" + "VALUES ('" + DeptId + "','" + DeptCode + "','" + Name + "','" + Contact + "','" + Telephone + "','" + Fax + "','" + Head + "')";
        //        SqlCommand cmd2 = new SqlCommand(q2, conn);
        //        cmd2.ExecuteNonQuery();
        //    }
        //}


        public static void UpdateDepartment(Department department)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE department SET deptId = '" + department.DeptId +
                    "', deptCode = '" + department.DeptCode +
                    "', name = '" + department.Name +
                    "', contact = '" + department.Contact +
                    "', telephone = '" + department.Telephone +
                    "', fax = '" + department.Fax +
                    "', head = '" + department.Head +
                    "' WHERE deptId = '" + department.DeptId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


        //public static void UpdateDepartment(int DeptId, string DeptCode, string Name, string Contact, string Telephone, string Fax, string Head)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q = @"UPDATE Department SET deptCode = '" + DeptCode + "', name = '" + Name + "', contact = '" + Contact + "', telephone = '" + Telephone + "', fax = '" + Fax + "', head = '" + Head + " WHERE deptId ='" + DeptId + "'";
        //        SqlCommand cmd = new SqlCommand(q, conn);
        //        cmd.ExecuteNonQuery();
        //    }
        //}


        public static List<string> GetAllDepartmentCodes()
        {
            List<string> departmentCodes = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT deptCode from Department";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string departmentCode = (string)reader["deptCode"];
                    departmentCodes.Add(departmentCode);
                }
            }
            return departmentCodes;
        }


        public static List<string> GetAllDepartmentNames()
        {
            List<string> departmentNames = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT name from Department";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string departmentName = (string)reader["name"];
                    departmentNames.Add(departmentName);
                }
            }
            return departmentNames;
        }


    }
}