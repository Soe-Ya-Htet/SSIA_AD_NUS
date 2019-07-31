using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.DAO;

namespace SSISTeam9.Models
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

        public static void UpdateSupplierDetails(Supplier supplier)
        {
            SupplierDAO.UpdateSupplierDetails(supplier);
        }

    }
}