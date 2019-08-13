using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System.Threading.Tasks;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    public class AdjVoucherController : Controller
    {
        //[HttpPost]
        //public ActionResult Index(List<Inventory> inventories)
        //{
        //    if (inventories == null)
        //    {
        //        return RedirectToRoute(new {controller = "Stock", action = "Check"});
        //    }
        //    List<AdjVoucher> adjVouchers = new List<AdjVoucher>();

        //    foreach (Inventory inventory in inventories)
        //    {
        //        int qty = inventory.ActualStock - inventory.StockLevel;
        //        if (qty != 0)
        //        {
        //            PriceList list = PriceListDAO.GetPriceListById(inventory.ItemId) ?? new PriceList();
        //            inventory.ItemSuppliersDetails = list;
        //            AdjVoucher adjVoucher = new AdjVoucher
        //            {
        //                AdjQty = qty,
        //                Item = inventory,
        //                TotalPrice = inventory.ItemSuppliersDetails.Supplier1UnitPrice * qty
        //            };
        //            adjVouchers.Add(adjVoucher);
        //        }
        //    }

        //    return View(adjVouchers);
        //}

        //public ActionResult Generate(List<AdjVoucher> adjVouchers)
        //{
        //    int managerId = 14;
        //    int supervisorId = 12;
        //    if (adjVouchers != null && adjVouchers.Count > 0)
        //    {
        //        for (var i = 0; i < adjVouchers.Count; i++)
        //        {
        //            int id = (adjVouchers[i].TotalPrice < 250.0) ? supervisorId : managerId;
        //            adjVouchers[i].AuthorisedBy = id.ToString();
        //        }
        //        AdjVoucherDAO.GenerateDisbursement(adjVouchers);
        //        AdjVoucherDAO.UpdateStock(adjVouchers);
        //    }

        //    ViewBag.Msg = "Success";
        //    return View(new List<AdjVoucher>());
        //}

        public ActionResult Generate(List<Inventory> inventories, string sessionId)
        {
            long adjId = (long)AdjVoucherService.GetLastId() + 1;
            int flag = 0;
            foreach (Inventory inventory in inventories)
            {
                int qty = inventory.ActualStock - inventory.StockLevel;
                if (qty != 0)
                {
                    flag = 1;
                    AdjVoucherService.CreateAdjVoucher(adjId, inventory.ItemId, qty);
                }
            }
            if(flag == 0)
            {
                TempData["errorMsg"] = "<script>alert('There is no any discrepancy.');</script>";
                ViewData["userName"] = EmployeeService.GetUserBySessionId(sessionId).EmpName;
                ViewData["sessionId"] = sessionId;
                return View("~/Views/StoreLandingPage/Home.cshtml");
            }


            return RedirectToAction("PutReason", new { sessionId});
        }

        public ActionResult PutReason(string sessionId)
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            adjVouchers = AdjVoucherService.GetUnauthorisedAdj();
            ViewData["adjVouchers"] = adjVouchers;
            ViewData["sessionId"] = sessionId;
            return View();
        }


    }

}