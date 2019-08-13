using System.Web.Mvc;
using SSISTeam9.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using SSISTeam9.Services;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    [StoreAuthorisationFilter]
    public class SupplierController : Controller
    {
        //Display all suppliers
        public ActionResult All(string sessionId)
        {
            List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();

            ViewData["suppliers"] = suppliers;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult CreateForm(string sessionId)
        {
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult CreateNew(Supplier supplier, string sessionId)
        {
            try
            {
                SupplierService.CreateNewSupplier(supplier);

                List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();
                ViewData["suppliers"] = suppliers;
                return RedirectToAction("All", new { sessionid = sessionId }); 
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Supplier code already exists! Please enter another one.');</script>";
                ViewData["sessionId"] = sessionId;
                return View("CreateForm");
            }
            
        }

        public ActionResult Delete(bool confirm, string supplierCode, string sessionId)
        {
            if (confirm)
            {
                SupplierService.DeleteSupplier(supplierCode);

                List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();
                ViewData["suppliers"] = suppliers;
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            return null;

        }

        public ActionResult DisplaySupplierDetails(string supplierCode, string sessionId)
        {
            ViewData["supplier"] = SupplierService.DisplaySupplierDetails(supplierCode);
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult Update(Supplier supplier, string sessionId)
        {
            try
            {
                SupplierService.UpdateSupplierDetails(supplier);

                List<Supplier> suppliers = SupplierService.DisplayAllSuppliers();
                ViewData["suppliers"] = suppliers;
                return RedirectToAction("All", new { sessionid = sessionId });
            }
            catch (SqlException)
            {
                TempData["errorMsg"] = "<script>alert('Supplier code already exists! Please enter another one.');</script>";
                ViewData["supplier"] = supplier;
                ViewData["sessionId"] = sessionId;
                return View("DisplaySupplierDetails");
            }
        }
    }
}