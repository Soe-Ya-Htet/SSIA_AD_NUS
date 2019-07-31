using System.Web.Mvc;
using SSISTeam9.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }

        //Display all suppliers
        public ActionResult All()
        {
            List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();

            ViewData["suppliers"] = suppliers;
            return View();
        }

        public ActionResult CreateForm()
        {
            return View();
        }

        public ActionResult CreateNew(Supplier supplier)
        {
            try
            {
                SupplierService.CreateNewSupplier(supplier);

                List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();
                ViewData["suppliers"] = suppliers;
                return View("All");
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Supplier code already exists! Please enter another one.');</script>";
                return View("CreateForm");
            }
            
        }

        public ActionResult Delete(bool confirm, string supplierCode)
        {
            if (confirm)
            {
                SupplierService.DeleteSupplier(supplierCode);

                List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();
                ViewData["suppliers"] = suppliers;
                return View("All");
            }
            return null;

        }

        public ActionResult DisplaySupplierDetails(string supplierCode)
        {
            ViewData["supplier"] = SupplierService.DisplaySupplierDetails(supplierCode);
            return View();
        }

        public ActionResult Update(Supplier supplier)
        {
            SupplierService.UpdateSupplierDetails(supplier);

            List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();
            ViewData["suppliers"] = suppliers;
            return View("All");
        }

    }
}