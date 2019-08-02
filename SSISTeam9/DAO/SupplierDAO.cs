﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using SSISTeam9.Models;

namespace SSISTeam9.DAO
{
    public class SupplierDAO
    {
        public static List<Supplier> DisplayAllSuppliers()
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
                        SupplierId = (long)reader["supplierId"],
                        SupplierCode = (reader["supplierCode"] == DBNull.Value) ? "Nil" : (string)reader["supplierCode"],
                        Name = (reader["name"] == DBNull.Value) ? "Nil" : (string)reader["name"],
                        GstNumber = (reader["gstNumber"] == DBNull.Value) ? "Nil" : (string)reader["gstNumber"],
                        Address = (reader["address"] == DBNull.Value) ? "Nil" : (string)reader["address"],
                        ContactName = (reader["contactName"] == DBNull.Value) ? "Nil" : (string)reader["contactName"],
                        PhoneNumber = (reader["phoneNumber"] == DBNull.Value) ? "Nil" : (string)reader["phoneNumber"],
                        FaxNumber = (reader["faxNumber"] == DBNull.Value) ? "Nil" : (string)reader["faxNumber"]

                    };
                    suppliers.Add(supplier);
                }
            }
            return suppliers;
        }

        public static void CreateNewSupplier(Supplier supplier)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = "INSERT INTO Supplier (supplierCode,name,gstNumber,address,contactName,phoneNumber,faxNumber)" + "VALUES ('" + supplier.SupplierCode + "','" + supplier.Name + "','" + supplier.GstNumber + "','" + supplier.Address + "','" + supplier.ContactName + "','" + supplier.PhoneNumber + "','" + supplier.FaxNumber + "')"; ;
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
               
            }
        }

        public static void DeleteSupplier(string supplierCode)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"DELETE from Supplier WHERE supplierCode = '" + supplierCode +"'";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public static Supplier DisplaySupplierDetails(string supplierCode)
        {

            Supplier supplier = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Supplier WHERE supplierCode = '" + supplierCode + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    supplier = new Supplier()
                    {
                        SupplierId = (long)reader["supplierId"],
                        SupplierCode = (reader["supplierCode"] == DBNull.Value) ? "Nil" : (string)reader["supplierCode"],
                        Name = (reader["name"] == DBNull.Value) ? "Nil" : (string)reader["name"],
                        GstNumber = (reader["gstNumber"] == DBNull.Value) ? "Nil" : (string)reader["gstNumber"],
                        Address = (reader["address"] == DBNull.Value) ? "Nil" : (string)reader["address"],
                        ContactName = (reader["contactName"] == DBNull.Value) ? "Nil" : (string)reader["contactName"],
                        PhoneNumber = (reader["phoneNumber"] == DBNull.Value) ? "Nil" : (string)reader["phoneNumber"],
                        FaxNumber = (reader["faxNumber"] == DBNull.Value) ? "Nil" : (string)reader["faxNumber"]

                    };
                }
            }
            return supplier;
        }

        public static void UpdateSupplierDetails(Supplier supplier)
        {
            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"UPDATE Supplier SET supplierCode = '" + supplier.SupplierCode +
                    "', name = '" + supplier.Name +
                    "', gstNumber = '" + supplier.GstNumber +
                    "', address = '" + supplier.Address +
                    "', contactName = '" + supplier.ContactName +
                    "', phoneNumber = '" + supplier.PhoneNumber +
                    "', faxNumber = '" + supplier.FaxNumber +
                    "' WHERE supplierId = '" + supplier.SupplierId + "'";

                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();

            }
        }

        public static List<string> GetAllSuppliersCodes()
        {
            List<string> supplierCodes = new List<string>();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT DISTINCT supplierCode from Supplier";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string supplierCode = (string)reader["supplierCode"];
                    supplierCodes.Add(supplierCode);
                }
            }
            return supplierCodes;
        }

        public static Supplier GetSupplierById(long supplierId)
        {
            Supplier supplier = new Supplier();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Supplier where supplierId = '" + supplierId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    supplier = new Supplier()
                    {
                        SupplierId = (long)reader["supplierId"],
                        SupplierCode = (reader["supplierCode"] == DBNull.Value) ? null : (string)reader["supplierCode"],
                        Name = (reader["name"] == DBNull.Value) ? null : (string)reader["name"],
                        GstNumber = (reader["gstNumber"] == DBNull.Value) ? null : (string)reader["gstNumber"],
                        Address = (reader["address"] == DBNull.Value) ? null : (string)reader["address"],
                        ContactName = (reader["contactName"] == DBNull.Value) ? null : (string)reader["contactName"],
                        PhoneNumber = (reader["phoneNumber"] == DBNull.Value) ? null : (string)reader["phoneNumber"],
                        FaxNumber = (reader["faxNumber"] == DBNull.Value) ? null : (string)reader["faxNumber"]

                    };
                }
                return supplier;
            }
        }


        public static Supplier GetSupplierByIdstring(string supplierId)
        {
            Supplier supplier = new Supplier();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from Supplier where supplierId = '" + supplierId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    supplier = new Supplier()
                    {
                        SupplierId = (long)reader["supplierId"],
                        SupplierCode = (reader["supplierCode"] == DBNull.Value) ? null : (string)reader["supplierCode"],
                        Name = (reader["name"] == DBNull.Value) ? null : (string)reader["name"],
                        GstNumber = (reader["gstNumber"] == DBNull.Value) ? null : (string)reader["gstNumber"],
                        Address = (reader["address"] == DBNull.Value) ? null : (string)reader["address"],
                        ContactName = (reader["contactName"] == DBNull.Value) ? null : (string)reader["contactName"],
                        PhoneNumber = (reader["phoneNumber"] == DBNull.Value) ? null : (string)reader["phoneNumber"],
                        FaxNumber = (reader["faxNumber"] == DBNull.Value) ? null : (string)reader["faxNumber"]

                    };
                }
                return supplier;
            }
        }

        public static PriceList GetItemSuppliersDetails(long itemId)
        {
            PriceList itemSuppliersDetails = new PriceList();

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT * from PriceList where itemId = '" + itemId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    itemSuppliersDetails = new PriceList()
                    {
                        Supplier1Id = (string)reader["supplier1Id"],
                        Supplier2Id = (string)reader["supplier2Id"],
                        Supplier3Id = (string)reader["supplier3Id"],
                        Supplier1UnitPrice = (string)reader["supplier1UnitPrice"],
                        Supplier2UnitPrice = (string)reader["supplier2UnitPrice"],
                        Supplier3UnitPrice = (string)reader["supplier3UnitPrice"]

                    };
                }
                return itemSuppliersDetails;
            }
        }

        public static string GetSupplierName(string supplierId)
        {
            string supplierName = null;

            using (SqlConnection conn = new SqlConnection(Data.db_cfg))
            {
                conn.Open();

                string q = @"SELECT name from Supplier where supplierId = '" + supplierId + "'";
                SqlCommand cmd = new SqlCommand(q, conn);

                supplierName = (string)cmd.ExecuteScalar();
               
                return supplierName;
            }
        }
    }
}