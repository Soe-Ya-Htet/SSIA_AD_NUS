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
        public List<Department> DisplayAllDepartment()
        {
            List<Department> departments = new List<Department>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Department";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Department department = new Department()
                    {
                        DeptId = (int)reader["deptId"],
                        DeptCode = (string)reader["deptCode"],
                        RepresentativeId = (int)reader["representativeId"],
                        CollectionPointId = (int)reader["collectionPointId"],
                        Name = (string)reader["name"],
                        Contact = (string)reader["contact"],
                        Telephone = (string)reader["telephone"],
                        Fax = (string)reader["fax"],
                        Head = (string)reader["head"]
                    };
                    departments.Add(department);
                }
            }
            return departments;
        }


        public Department DisplaySelectedDepartment(int DeptId)
        {
            Department department = new Department();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Department WHERE deptId = '" + DeptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    department.DeptId = (int)reader["deptId"];
                    department.DeptCode = (string)reader["deptCode"];
                    department.RepresentativeId = (int)reader["representativeId"];
                    department.CollectionPointId = (int)reader["collectionPointId"];
                    department.Name = (string)reader["name"];
                    department.Contact = (string)reader["contact"];
                    department.Telephone = (string)reader["telephone"];
                    department.Fax = (string)reader["fax"];
                    department.Head = (string)reader["head"];
                }
            }
            return department;
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


        public static void CreateDepartment(string DeptCode, string Name, string Contact, string Telephone, string Fax, string Head)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q1 = @"SELECT MAX(deptId) from Department";
                SqlCommand cmd1 = new SqlCommand(q1, conn);
                int DeptId = (int)cmd1.ExecuteScalar() + 1;

                string q2 = "INSERT INTO Inventory (deptId,deptCode,name,contact,telephone,fax,head)" + "VALUES ('" + DeptId + "','" + DeptCode + "','" + Name + "','" + Contact + "','" + Telephone + "','" + Fax + "','" + Head + "')";
                SqlCommand cmd2 = new SqlCommand(q2, conn);
                cmd2.ExecuteNonQuery();
            }
        }


        public static void UpdateDepartment(int DeptId, string DeptCode, string Name, string Contact, string Telephone, string Fax, string Head)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE Department SET deptCode = '" + DeptCode + "', name = '" + Name + "', contact = '" + Contact + "', telephone = '" + Telephone + "', fax = '" + Fax + "', head = '" + Head + " WHERE deptId ='" + DeptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}