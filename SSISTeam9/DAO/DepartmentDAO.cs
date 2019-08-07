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
                            LEFT JOIN Employee c ON d.contact IS NULL OR d.contact = c.empId
                            LEFT JOIN Employee h ON d.head IS NULL OR d.head = h.empId
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

                string q = @"SELECT d.deptId, d.deptCode, d.deptName, d.collectionPointId, c.empName AS contact, 
                            d.telephone, d.fax, h.empName AS head, e.empName AS rep, cp.name AS collectionPoint
                            FROM Department d
                            LEFT JOIN Employee c ON d.contact IS NULL OR d.contact = c.empId
                            LEFT JOIN Employee h ON d.head IS NULL OR d.head = h.empId
                            LEFT JOIN Employee e ON d.representativeId = e.empId
                            LEFT JOIN CollectionPoint cp ON d.collectionPointId = cp.placeId
                            WHERE d.deptId = '" + DeptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CollectionPoint cP = new CollectionPoint()
                    {
                        PlacedId = (long)reader["collectionPointId"]
                    };
                    department = new Department()
                    {
                        DeptId = (long)reader["deptId"],
                        DeptCode = (string)reader["deptCode"],
                        DeptName = (string)reader["deptName"],
                        Contact = (reader["contact"] == DBNull.Value) ? "Nil" : (string)reader["contact"],
                        Telephone = (string)reader["telephone"],
                        Fax = (reader["fax"] == DBNull.Value) ? "Nil" : (string)reader["fax"],
                        Head = (reader["head"] == DBNull.Value) ? "Nil" : (string)reader["head"],
                        CollectionPoint = cP
                        

                    };
                    department.Representative = new Employee();
                    department.Representative.EmpName = (reader["rep"] == DBNull.Value) ? " " : (string)reader["rep"];
                    //department.CollectionPoint = new CollectionPoint();
                    //department.CollectionPoint.Name = (reader["collectionPoint"] == DBNull.Value) ? " " : (string)reader["collectionPoint"];
                    //Roy commented out these lines because they were giving me problems with collectionPointId
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

                string q1 = @"SELECT c.empId AS contact, h.empId AS head
                            FROM Employee c, Employee h
                            WHERE c.empName = '" + department.Contact + "' AND h.empName = '" + department.Head + "'";

                SqlCommand cmd1 = new SqlCommand(q1, conn);

                SqlDataReader reader = cmd1.ExecuteReader();
                long contact = 0;
                long head = 0;
                while (reader.Read())
                {
                    contact = (long)reader["contact"];
                    head = (long)reader["head"];
                }

                reader.Close();

                string q = @"INSERT INTO department (deptCode,deptName,contact,telephone,fax,head)" + 
                            "VALUES ('" +  department.DeptCode +
                            "','" + department.DeptName +
                            "','" + contact +
                            "','" + department.Telephone +
                            "','" + department.Fax +
                            "','" + head + "')";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateDepartmentRepById(long deptId,long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"Update Department Set representativeId="+empId+" where deptId =" + deptId;
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateDepartmentHead(long deptId, long empId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"Update Department Set head=" + empId + " where deptId =" + deptId;
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static long GetCurrentRepById(long deptId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                long currentRep = 0;
                string q = @"SELECT representativeId from Department where deptId = '" + deptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    currentRep = (long)reader[0];
                }
                return currentRep;
            }
        }

        public static long GetCurrentHeadById(long deptId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                string currentHead = "";
                string q = @"SELECT head from Department where deptId = '" + deptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    currentHead = (string)reader[0];
                }
                return long.Parse(currentHead);
            }
        }

        public static long GetPreviousHead(long deptId)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();
                long previousHead = 0;
                string q = @"SELECT empId from Employee where empRole='EMPLOYEE' and empDisplayRole='HEAD' and deptId = '" + deptId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    previousHead = (long)reader[0];
                }
                return previousHead;
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

                string q1 = @"SELECT c.empId AS contact, h.empId AS head
                            FROM Employee c, Employee h
                            WHERE c.empName = '" + department.Contact + "' AND h.empName = '" + department.Head + "'";

                SqlCommand cmd1 = new SqlCommand(q1, conn);

                SqlDataReader reader = cmd1.ExecuteReader();
                long contact = 0;
                long head = 0;
                while (reader.Read())
                {
                    contact = (long)reader["contact"];
                    head = (long)reader["head"];
                }
                reader.Close();


                string q = @"UPDATE department SET deptCode = '" + department.DeptCode +
                    "', deptName = '" + department.DeptName +
                    "', contact = '" + contact +
                    "', telephone = '" + department.Telephone +
                    "', fax = '" + department.Fax +
                    "', head = '" + head +
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

        public static List<Department> GetDepartmentsByIdList(List<long> deptIds)
        {

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Department where deptId IN ({0})";

                var parms = deptIds.Select((s, i) => "@id" + i.ToString()).ToArray();
                var inclause = string.Join(",", parms);

                string sql = string.Format(q, inclause);
                Console.Write(sql);

                SqlCommand cmd = new SqlCommand(sql, conn);
                for (var i = 0; i < parms.Length; i++)
                {
                    cmd.Parameters.AddWithValue(parms[i], deptIds[i]);
                }

                Department department = null;

                List<Department> departments = new List<Department>();
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
                    
                    departments.Add(department);
                }
                return departments;
            }
        }


    }
}