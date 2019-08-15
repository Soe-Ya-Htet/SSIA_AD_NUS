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
    [StoreAuthorisationFilter]
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

        public ActionResult Index(string sessionId)
        {
            Employee user = EmployeeService.GetUserBySessionId(sessionId);
            if (user.EmpRole == "STORE_SUPERVISOR" || user.EmpRole == "STORE_MANAGER")
            {
                return RedirectToAction("PendingApprove", new { sessionId});
            }
            else
            {
                return RedirectToAction("PutReason", new { sessionId });
            }

        }

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
                    StockService.UpdateInventoryStockById(inventory.ItemId, inventory.ActualStock);
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

            //status = 0 means need to be submit for reason.
            adjVouchers = AdjVoucherService.GetAdjByStatus(0);
            if(adjVouchers.Count == 0)
            {
                return RedirectToAction("AllAdjVouchers", new { sessionId });
            }
            else
            {
                foreach (AdjVoucher adj in adjVouchers)
                {
                    adj.Reason = null;
                    adj.Item = CatalogueService.GetCatalogueById(adj.ItemId);
                    adj.Item.ItemSuppliersDetails = PurchaseOrderService.GetItemSuppliersDetails(adj.ItemId);
                }
                ViewData["adjVouchers"] = adjVouchers;
                ViewData["sessionId"] = sessionId;
                return View();
            }
            
        }

        public ActionResult UpdateReason(List<AdjVoucher> adjVouchers, string sessionId)
        {
            Employee user = EmployeeService.GetUserBySessionId(sessionId);
            double totalAmount = 0;

            //To check if user entered reason and if not, to return back to form to show validation message
            foreach (AdjVoucher adj in adjVouchers)
            {
                if (string.IsNullOrWhiteSpace(adj.Reason))
                {
                    ViewData["adjVouchers"] = adjVouchers;
                    ViewData["sessionId"] = sessionId;
                    return View("PutReason");
                }
            }

            foreach (AdjVoucher adj in adjVouchers)
            {
                PriceList priceList = PriceListService.GetPriceListByItemId(adj.ItemId);
                double price = 0;
                if (priceList != null)
                {
                    price = priceList.Supplier1UnitPrice;
                }

                double amount = price * adj.AdjQty;
                totalAmount = totalAmount + amount;
                AdjVoucherService.UpdateReason(adj);
            }

            if (totalAmount > -250)
            {
                //status = 2, Pending authorisation by Supervisor
                long adjId = 0;
                foreach (AdjVoucher adj in adjVouchers)
                {
                    if (adj.AdjId != adjId)
                    {
                        adjId = adj.AdjId;
                        AdjVoucherService.UpdateStatus(adjId, 2);
                    }
                }                
                TempData["errorMsg"] = "<script>alert('Total discrepancy is less than $250, pending for Store Supervisor to authorise.');</script>";
            }
            else
            {
                //status = 3, Pending authorisation by Manager
                long adjId = 0;
                foreach (AdjVoucher adj in adjVouchers)
                {
                    if (adj.AdjId != adjId)
                    {
                        adjId = adj.AdjId;
                        AdjVoucherService.UpdateStatus(adjId, 3);
                    }
                }
                TempData["errorMsg"] = "<script>alert('Total discrepancy is more than $250, pending for Store Manager to authorise.');</script>";

            }
            ViewData["userName"] = user.EmpName;
            ViewData["sessionId"] = sessionId;
            return View("~/Views/StoreLandingPage/Home.cshtml");
        }


        //public ActionResult AuthoriseByS(List<AdjVoucher> adjVouchers, string sessionId)
        //{
        //    Employee user = EmployeeService.GetUserBySessionId(sessionId);
        //    double totalAmount = 0;
        //    foreach (AdjVoucher adj in adjVouchers)
        //    {
        //        PriceList priceList = PriceListService.GetPriceListByItemId(adj.ItemId);
        //        double price = 0;
        //        if (priceList != null)
        //        {
        //            price = priceList.Supplier1UnitPrice;
        //        }

        //        double amount = price * adj.AdjQty;
        //        totalAmount = totalAmount + amount;
        //        AdjVoucherService.UpdateReason(adj);
        //    }

        //    if (totalAmount > -250)
        //    {
        //        //status = 1, auto approved by supervisor
        //        AdjVoucherService.UpdateStatus(adjVouchers[0].AdjId, 1);
        //        AdjVoucherService.AuthoriseBy(adjVouchers[0].AdjId, user.EmpId);
        //        TempData["errorMsg"] = "<script>alert('Total discrepancy is less than $250, authorised already.');</script>";
        //    }
        //    else
        //    {
        //        //status = 2, pending approve for manager
        //        AdjVoucherService.UpdateStatus(adjVouchers[0].AdjId, 2);
        //        TempData["errorMsg"] = "<script>alert('Total discrepancy is more than $250, pending for Store Manager to authorise.');</script>";

        //    }
        //    ViewData["userName"] = user.EmpName;
        //    ViewData["sessionId"] = sessionId;
        //    return View("~/Views/StoreLandingPage/Home.cshtml");
        //}


        public ActionResult PendingApprove(string sessionId)
        {
            Employee user = EmployeeService.GetUserBySessionId(sessionId);
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            string authoriseBy = null;
            if (user.EmpRole == "STORE_SUPERVISOR")
            {
                adjVouchers = AdjVoucherService.GetAdjByStatus(2);

                foreach(var adj in adjVouchers)
                {
                    //to get item details for view
                    adj.Item = CatalogueService.GetCatalogueById(adj.ItemId);
                    adj.Item.ItemSuppliersDetails = PurchaseOrderService.GetItemSuppliersDetails(adj.ItemId);
                }
                if (adjVouchers.Count == 0)
                {
                    return RedirectToAction("AllAdjVouchers", new { sessionId });
                }
                authoriseBy = "Supervisor";
            }
            else if(user.EmpRole == "STORE_MANAGER")
            {
                adjVouchers = AdjVoucherService.GetAdjByStatus(3);

                foreach (var adj in adjVouchers)
                {
                    //to get item details for view
                    adj.Item = CatalogueService.GetCatalogueById(adj.ItemId);
                    adj.Item.ItemSuppliersDetails = PurchaseOrderService.GetItemSuppliersDetails(adj.ItemId);
                }

                if (adjVouchers.Count == 0)
                {
                    return RedirectToAction("AllAdjVouchers", new { sessionId });
                }
                authoriseBy = "Manager";
            }

            //To group by vouchers
            Dictionary<long, List<AdjVoucher>> byVouchers = new Dictionary<long, List<AdjVoucher>>();

            foreach (var adj in adjVouchers)
            {
                if (byVouchers.ContainsKey(adj.AdjId))
                {
                    byVouchers[adj.AdjId].Add(adj);
                }
                else
                {
                    List <AdjVoucher> adjs = new List<AdjVoucher>();
                    adjs.Add(adj);
                    byVouchers[adj.AdjId] = adjs;
                }
            }

            ViewData["authoriseBy"] = authoriseBy;
            ViewData["adjVouchers"] = adjVouchers;
            ViewData["byVouchers"] = byVouchers;
            ViewData["sessionId"] = sessionId;
            return View();
        }


        public ActionResult Authorise( string sessionId)
        {
            Employee user = EmployeeService.GetUserBySessionId(sessionId);
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            if (user.EmpRole == "STORE_SUPERVISOR")
            {
                adjVouchers = AdjVoucherService.GetAdjByStatus(2);
            }
            else if (user.EmpRole == "STORE_MANAGER")
            {
                adjVouchers = AdjVoucherService.GetAdjByStatus(3);
            }
            
            long adjId = 0;
            foreach(AdjVoucher adj in adjVouchers)
            {
                if(adj.AdjId != adjId)
                {
                    adjId = adj.AdjId;
                    AdjVoucherService.AuthoriseBy(adjId, user.EmpId);
                }
            }
            
            TempData["errorMsg"] = "<script>alert('Adjustment voucher authorise successfully.');</script>";
            ViewData["userName"] = user.EmpName;
            ViewData["sessionId"] = sessionId;
            return View("~/Views/StoreLandingPage/Home.cshtml");

        }


        public ActionResult AllAdjVouchers(string sessionId)
        {
            long totalAdjNumber = (long)AdjVoucherService.GetLastId();
            List<string> dates = new List<string>();
            List<string> authorisedBys = new List<string>();
            List<string> statuses = new List<string>();
            List<string> adjIds = new List<string>();
            for (long i = 1; i <= totalAdjNumber; i++)
            {
                string adjId = i.ToString("000/000/00");
                adjIds.Add(adjId);
                AdjVoucher adj = AdjVoucherService.GetAdjByAdjId(i)[0];
                string date = adj.Date.Day.ToString("00") + "/" + adj.Date.Month.ToString("00") + "/" + adj.Date.Year;
                dates.Add(date);
                string authorisedBy = "Nil";
                if (adj.AuthorisedBy != 0)
                {
                    authorisedBy = EmployeeService.GetEmployeeById(adj.AuthorisedBy).EmpName;
                }
                authorisedBys.Add(authorisedBy);
                string status = null;
                switch (adj.status)
                {                   
                    case 0:
                        status = "Pending submit reason by Clerk";
                        break;
                    case 1:
                        status = "Authorised";
                        break;
                    case 2:
                        status = "Pending authorisation by Supervisor";
                        break;
                    case 3:
                        status = "Pending authorisation by Manager";
                        break;
                }
                statuses.Add(status);                                      
            }
            ViewData["sessionId"] = sessionId;
            ViewData["dates"] = dates;
            ViewData["authorisedBys"] = authorisedBys;
            ViewData["statuses"] = statuses;
            ViewData["adjIds"] = adjIds;


            return View();
        }


        public ActionResult AdjDetails(long adjId, string sessionId)
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            adjVouchers = AdjVoucherService.GetAdjByAdjId(adjId);
            foreach(AdjVoucher adj in adjVouchers)
            {
                adj.ItemCode = CatalogueService.GetCatalogueById(adj.ItemId).ItemCode;
            }
            string adjIdstring = adjId.ToString("000/000/00");
            string authorisedBy = "Nil";
            if (adjVouchers[0].AuthorisedBy != 0)
            {
                authorisedBy = EmployeeService.GetEmployeeById(adjVouchers[0].AuthorisedBy).EmpName;
            }

            ViewData["adjIdstring"] = adjIdstring;
            ViewData["adjVouchers"] = adjVouchers;
            ViewData["authorisedBy"] = authorisedBy;
            ViewData["sessionId"] = sessionId;
            return View();
        }

    }

}