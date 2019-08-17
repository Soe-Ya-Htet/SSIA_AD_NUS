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

        /*
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
                TempData["errorMsg"] = "<script>alert('There are no discrepancies in stock.');</script>";
                ViewData["userName"] = EmployeeService.GetUserBySessionId(sessionId).EmpName;
                ViewData["sessionId"] = sessionId;
                return View("~/Views/StoreLandingPage/Home.cshtml");
            }


            return RedirectToAction("PutReason", new { sessionId});
        }*/


        public ActionResult Generate(Inventory item, FormCollection formCollection, string sessionId)
        {
            List<int> itemsQuantities = new List<int>();
            List<long> itemIds = new List<long>();
            long adjId = (long)AdjVoucherService.GetLastId() + 1;
            int flag = 0;

            string counter = formCollection["counter"];

            for (int i = 0; i < int.Parse(counter); i++)
            {
                int actualStock = int.Parse(formCollection["actualStock_" + i]);
                int lastStock = int.Parse(formCollection["lastStock_" + i]);
                int qty = actualStock - lastStock;
                if (qty != 0)
                {
                    flag = 1;
                    long itemId = long.Parse(formCollection["itemId_" + i]);
                    AdjVoucherService.CreateAdjVoucher(adjId, itemId, qty);
                    StockService.UpdateInventoryStockById(itemId, actualStock);
                }
            }
            if (flag == 0)
            {
                TempData["errorMsg"] = "<script>alert('There are no discrepancies in stock.');</script>";
                ViewData["userName"] = EmployeeService.GetUserBySessionId(sessionId).EmpName;
                ViewData["sessionId"] = sessionId;
                return View("~/Views/StoreLandingPage/Home.cshtml");
            }
            return RedirectToAction("PutReason", new { sessionId });
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
                //Inform Store supervisor
                List<Employee> supervisors = new List<Employee>();
                EmailNotification notice = new EmailNotification();
                supervisors = EmployeeService.GetEmployeeByRole("STORE_SUPERVISOR");
                foreach(Employee s in supervisors)
                {                    
                    notice.ReceiverMailAddress = EmployeeService.GetUserEmail(s.EmpId);
                    EmailService emailService = new EmailService();
                    Task.Run(() => emailService.SendMail(notice, EmailTrigger.ON_PENDING_ADJVOUCHER));
                }
                TempData["errorMsg"] = "<script>alert('Total discrepancy amount is less than $250, pending for Store Supervisor to authorise.');</script>";
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

                //Inform manager
                List<Employee> managers = new List<Employee>();
                EmailNotification notice = new EmailNotification();
                managers = EmployeeService.GetEmployeeByRole("STORE_MANAGER");
                foreach (Employee m in managers)
                {
                    notice.ReceiverMailAddress = EmployeeService.GetUserEmail(m.EmpId);
                    EmailService emailService = new EmailService();
                    Task.Run(() => emailService.SendMail(notice, EmailTrigger.ON_PENDING_ADJVOUCHER));
                }
                TempData["errorMsg"] = "<script>alert('Total discrepancy amount is more than $250, pending for Store Manager to authorise.');</script>";

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


        public ActionResult Authorise(string sessionId)
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

                //The function below is for update stock card
                //By the time authorise adjustment voucher, update StockCard table with itemId and date, souceType = 1
                StockCardService.CreateStockCardFromAdj(adj);
            }

            TempData["errorMsg"] = "<script>alert('Adjustment vouchers have been authorised successfully.');</script>";
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
                Inventory c = CatalogueService.GetCatalogueById(adj.ItemId);
                adj.ItemCode = c.ItemCode;
                adj.Description = c.Description;
                PriceList p = PriceListService.GetPriceListByItemId(adj.ItemId);
                if(p != null)
                {
                    adj.UnitPrice = p.Supplier1UnitPrice;
                }
                else
                {
                    adj.UnitPrice = 1;
                }
                double total = adj.AdjQty * adj.UnitPrice;
                adj.TotalPrice = Math.Abs(total);
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