using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class SupplierDAO
    {
        public List<Supplier> DisplayAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Supplier";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Supplier supplier = new Supplier()
                    {
                       SupplierCode = (long)reader["supplierCode"],
                       name = (string)reader["name"],
                       GstNumber = (string)reader["gstNumber"],
                       Address = (string)reader["address"],
                       ContactName = (string)reader["contactName"],
                       PhoneNumber = (string)reader["phoneNumber"],
                       FaxNumber = (string)reader["faxNumber"]
                       
                    };
                    suppliers.Add(supplier);
                }
            }
            return suppliers;
        }
    }
}