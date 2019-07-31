using System.Web.Mvc;
using SSISTeam9.Models;
using System.Collections.Generic;

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

        public ActionResult CreateForm(Supplier supplier)
        {
            SupplierService.CreateNewSupplier(supplier);

            return View();
        }

        public ActionResult CreateNew()
        {
            return View();
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