using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.DAO;
using SSISTeam9.Models;

namespace SSISTeam9.Services
{
    public class SupplierService
    {
        public static List<Supplier> DisplayAllSuppliers()
        {
            return SupplierDAO.DisplayAllSuppliers();
        }

        public static Supplier DisplaySupplierDetails(string supplierCode)
        {
            return SupplierDAO.DisplaySupplierDetails(supplierCode);
        }

        public static void CreateNewSupplier(Supplier supplier)
        {
            SupplierDAO.CreateNewSupplier(supplier);
        }

        public static void DeleteSupplier(string supplierCode)
        {
            SupplierDAO.DeleteSupplier(supplierCode);
        }

        public static void UpdateSupplierDetails(Supplier supplier)
        {
            SupplierDAO.UpdateSupplierDetails(supplier);
        }

        public static string GetSupplierName(long supplierId)
        {
            return SupplierDAO.GetSupplierName(supplierId);
        }
    }
}