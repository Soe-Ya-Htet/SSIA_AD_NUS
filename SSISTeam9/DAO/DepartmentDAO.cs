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
        public List<Department> DisplayAllSuppliers()
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
    }
}