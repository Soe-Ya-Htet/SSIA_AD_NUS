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
        public static List<Department> GetAllDepartment()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT d.deptId, d.deptCode, d.deptName, c.empName AS contact, 
                            d.telephone, d.fax, h.empName AS head, e.empName AS rep, cp.name AS collectionPoint
                            FROM Department d
                            INNER JOIN Employee c ON d.contact = c.empId
                            INNER JOIN Employee h ON d.head = h.empId
                            LEFT JOIN Employee e ON d.representativeId = e.empId
                            LEFT JOIN CollectionPoint cp ON d.collectionPointId = cp.placeId";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptCode = (string)reader["deptCode"],
                        DeptName = (string)reader["deptName"],
                        Contact = (reader["contact"] == DBNull.Value) ? "Nil" : (string)reader["contact"],
                        Telephone = (string)reader["telephone"],
                        Fax = (reader["fax"] == DBNull.Value) ? "Nil" : (string)reader["fax"],
                        Head = (reader["head"] == DBNull.Value) ? "Nil" : (string)reader["head"]
                    };
                    department.Representative = new Employee();
                    department.Representative.EmpName = (reader["rep"] == DBNull.Value) ? " " : (string)reader["rep"];
                    department.CollectionPoint = new CollectionPoint();
                    department.CollectionPoint.Name = (reader["collectionPoint"] == DBNull.Value) ? " " : (string)reader["collectionPoint"];
                    departments.Add(department);
                }



            }
            return departments;
        }


        public static Department GetDepartmentById(long DeptId)
        {
            Department department = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT d.deptId, d.deptCode, d.deptName, c.empName AS contact, 
                            d.telephone, d.fax, h.empName AS head, e.empName AS rep, cp.name AS collectionPoint
                            FROM Department d
                            INNER JOIN Employee c ON d.contact = c.empId
                            INNER JOIN Employee h ON d.head = h.empId
                            LEFT JOIN Employee e ON d.representativeId = e.empId
                            LEFT JOIN CollectionPoint cp ON d.collectionPointId = cp.placeId
                            WHERE d.deptId = '" + DeptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptCode = (string)reader["deptCode"],
                        DeptName = (string)reader["deptName"],
                        Contact = (reader["contact"] == DBNull.Value) ? "Nil" : (string)reader["contact"],
                        Telephone = (string)reader["telephone"],
                        Fax = (reader["fax"] == DBNull.Value) ? "Nil" : (string)reader["fax"],
                        Head = (reader["head"] == DBNull.Value) ? "Nil" : (string)reader["head"]
                    };
                    department.Representative = new Employee();
                    department.Representative.EmpName = (reader["rep"] == DBNull.Value) ? " " : (string)reader["rep"];
                    department.CollectionPoint = new CollectionPoint();
                    department.CollectionPoint.Name = (reader["collectionPoint"] == DBNull.Value) ? " " : (string)reader["collectionPoint"];
                }
                return department;
            }
        }


        public static void DeleteDepartment(long DeptId)
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

                string q = @"INSERT INTO department (deptCode,deptName,contact,telephone,fax,head)" + 
                            "VALUES ('" +  department.DeptCode +
                            "','" + department.DeptName +
                            "','" + department.Contact +
                            "','" + department.Telephone +
                            "','" + department.Fax +
                            "','" + department.Head + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        //public static void CreateDepartment(string DeptCode, string DeptName, string Contact, string Telephone, string Fax, string Head)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q1 = @"SELECT MAX(deptId) from Department";
        //        SqlCommand cmd1 = new SqlCommand(q1, conn);
        //        long DeptId = (long)cmd1.ExecuteScalar() + 1;

        //        string q2 = "INSERT INTO Department (deptId,deptCode,deptName,contact,telephone,fax,head)" + "VALUES ('" + DeptId + "','" + DeptCode + "','" + DeptName + "','" + Contact + "','" + Telephone + "','" + Fax + "','" + Head + "')";
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
                    "', deptName = '" + department.DeptName +
                    "', contact = '" + department.Contact +
                    "', telephone = '" + department.Telephone +
                    "', fax = '" + department.Fax +
                    "', head = '" + department.Head +
                    "' WHERE deptId = '" + department.DeptId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }


        //public static void UpdateDepartment(long DeptId, string DeptCode, string DeptName, string Contact, string Telephone, string Fax, string Head)
        //{
        //    using (SqlConnection conn = new SqlConnection(Data.db_cfg))
        //    {
        //        conn.Open();

        //        string q = @"UPDATE Department SET deptCode = '" + DeptCode + "', deptName = '" + DeptName + "', contact = '" + Contact + "', telephone = '" + Telephone + "', fax = '" + Fax + "', head = '" + Head + " WHERE deptId ='" + DeptId + "'";
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

                string q = @"SELECT DISTINCT deptName from Department";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string departmentName = (string)reader["deptName"];
                    departmentNames.Add(departmentName);
                }
            }
            return departmentNames;
        }


    }
}