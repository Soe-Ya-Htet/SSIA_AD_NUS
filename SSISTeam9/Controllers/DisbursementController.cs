using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;
using SSISTeam9.Filters;
using System.Threading.Tasks;

namespace SSISTeam9.Controllers
{
    public class DisbursementController : Controller
    {
        private readonly IEmailService emailService;

        public DisbursementController()
        {
            emailService = new EmailService();
        }
        // GET: Disbursement
        [StoreAuthorisationFilter]
        public ActionResult ViewAllDisbursements(string collectionPt,string sessionId)
        {
            ViewData["disbursements"] = DisbursementListService.ViewOutstandingDisbursementsByCollection(collectionPt);
            if (collectionPt == null)
            {
                ViewData["collectionPoint"] = "1";
            }
            else
            {
                ViewData["collectionPoint"] = collectionPt;
            }

            ViewData["sessionId"] = sessionId;
            return View();
        }
        [StoreAuthorisationFilter]
        public ActionResult ViewAllCompletedDisbursements(string sessionId)
        {
            ViewData["disbursements"] = DisbursementListService.ViewAllCompletedDisbursements();
            ViewData["sessionId"] = sessionId;
            return View();
        }
        [StoreAuthorisationFilter]
        public ActionResult ViewDeptCompletedDisburse(long deptId, string sessionId)
        {
            ViewData["disbursements"] = DisbursementListService.ViewCompletedDisbursByDept(deptId);
            ViewData["dept"] = DepartmentService.GetDepartmentById(deptId);
            ViewData["sessionId"] = sessionId;
            return View();
        }


        [StoreAuthorisationFilter]
        public ActionResult ViewDisbursementDetails(long listId, string collectionPt, string sessionId)
        {
            
            ViewData["details"] = DisbursementListService.ViewDisbursementDetails(listId);
            
            ViewData["listId"] = listId;
            ViewData["collectionPt"] = collectionPt;
            ViewData["sessionId"] = sessionId;
            ViewData["status"] = (DisbursementListService.GetDisbursementListByListId(listId)).AcknowledgedBy;
            return View();

        }


        [StoreAuthorisationFilter]
        public ActionResult ViewOnlyDisbursementDetails(long listId, string collectionPt, string sessionId)
        {

            ViewData["details"] = DisbursementListService.ViewDisbursementDetails(listId);

            ViewData["listId"] = listId;
            ViewData["collectionPt"] = collectionPt;
            ViewData["sessionId"] = sessionId;
            ViewData["status"] = (DisbursementListService.GetDisbursementListByListId(listId)).AcknowledgedBy;
            return View();

        }

        [StoreAuthorisationFilter]
        public ActionResult CreateDisbursementLists(List<Entry> entries, string sessionId)
        {
            List<long> deptIds = new List<long>();
            
            List<DisbursementList> disbursementLists = new List<DisbursementList>();
            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    if (deptIds.Contains(entry.deptId))
                    {

                    }
                    else
                    {
                        deptIds.Add(entry.deptId);

                    }
                }
            }
            

            List<EmailNotification> notices = new List<EmailNotification>();


            

            foreach (var deptId in deptIds)
            {
                List<DisbursementListDetails> disbursementListDetails = new List<DisbursementListDetails>();
                Department dept = new Department()
                {
                    DeptId = deptId
                };
                DisbursementList d = new DisbursementList()
                {
                    Department = dept,
                    DisbursementListDetails = disbursementListDetails,
                    date = entries[0].collectionDate
                    
                };

                string repMail = RequisitionService.GetRep(d.Department.DeptId); //change to rep
                EmailNotification notice = new EmailNotification();
                notice.ReceiverMailAddress = repMail;
                notice.CollectionDate = d.date.ToString("dd/MM/yyyy");
                notices.Add(notice);

                foreach (var entry in entries)
                {
                    if (entry.deptId==deptId)
                    {
                        Inventory i = new Inventory()
                        {
                            ItemId = entry.itemId,
                            

                        };
                        DisbursementListDetails dDets = new DisbursementListDetails()
                        {
                            Item = i,
                            Quantity= entry.quantity
                            
                        };
                        
                        d.DisbursementListDetails.Add(dDets);
                    }
                }
                disbursementLists.Add(d);
            }
            DisbursementListService.CreateDisbursementLists(disbursementLists);
            Task.Run(() => {
                foreach (var notice in notices)
                {
                    emailService.SendMail(notice, EmailTrigger.ON_DISBURSEMENT_CREATION); //need to send to multiple reps

                }
            });


            return Json(Url.Action("ViewAllDisbursements","Disbursement", new { sessionId = sessionId }));
        }
        [StoreAuthorisationFilter]
        public ActionResult UpdateDisbursementLists(long listId, long collectionPt, List<PerItem> items, string sessionId)
        {
            foreach (var item in items)
            {
                Inventory i = new Inventory()
                {
                    ItemId = item.itemId
                };
                DisbursementListDetails disbursementDetails = new DisbursementListDetails()
                {
                    Quantity = item.quantity,
                    Item = i
                };
                DisbursementListService.UpdateDisbursementListDetails(listId, disbursementDetails);

                /* Move the following code to RestRepresentativeController*/
                ////Attention: DisbursementList can only disburse once, date for that list is not null

                ///*The following code is for ChargeBack table*/
                ////By the time disburse item, calculate the amount of this list, update ChargeBack table               
                //PriceList priceList = PriceListService.GetPriceListByItemId(i.ItemId);
                //double price = 0;
                //if (priceList != null)
                //{
                //    price = priceList.Supplier1UnitPrice;
                //}

                //double amount = price * disbursementDetails.Quantity;
                //DisbursementList disbursementList = DisbursementListService.GetDisbursementListByListId(listId);
                //ChargeBackService.UpdateChargeBackData(amount, disbursementList);

                ///*The following code is for StockCard table*/
                ////By the time disburse item, update StockCard table with itemId, deptId and date, souceType = 2

                //int balance = CatalogueService.GetCatalogueById(disbursementDetails.Item.ItemId).StockLevel - disbursementDetails.Quantity;
                //StockCardService.CreateStockCardFromDisburse(disbursementDetails, disbursementList, balance);
                              
            }


            return Json(Url.Action("ViewAllDisbursements", "Disbursement", new {collectionPt = collectionPt, sessionId= sessionId }));
        }

        [DepartmentFilter]
        public ActionResult RepDisbursementList(string sessionId, bool timeErr)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            DisbursementList disbursementList = new DisbursementList();
            List<DisbursementListDetails> disDetailList = new List<DisbursementListDetails>();
            disbursementList = DisbursementListService.GetDisbursementListByDeptId(emp.DeptId);
            disbursementList.date = disbursementList.date.Date;
            if (disbursementList.ListId != 0)
            {
                disDetailList = DisbursementListService.ViewDisbursementDetails(disbursementList.ListId);
            }
            List<CollectionPoint> collectionPoints = DisbursementListService.GetAllCollectionPoints();

            Dictionary<string, string> errDict = new Dictionary<string, string>();
            if (timeErr)
            {
                errDict.Add("time", "To change the collection point, you need at least 30 minutes before the pick up time of selected one.");
            }
            ViewData["errDict"] = errDict;
            ViewData["disbursement"] = disbursementList;
            ViewData["disDetailList"] = disDetailList;
            ViewData["collectionPoints"] = collectionPoints;
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");

            return View();
        }

        [DepartmentFilter]
        public ActionResult ChangeCollectionPoint(string sessionId, DisbursementList disbursement,FormCollection frm)
        {
            bool timeErr = false;
            disbursement = DisbursementListService.GetDisbursementListByListId(disbursement.ListId);
            long selectedPoint = long.Parse(frm["collect"].ToString());
            if (disbursement.CollectionPoint.PlacedId == selectedPoint)
            {
                return RedirectToAction("RepDisbursementList", new { sessionId = sessionId, timeErr = timeErr });
            }
            CollectionPoint c = DisbursementListService.GetCollectionPointByPlaceId(selectedPoint);
            DateTime selectedTime = disbursement.date.Date + c.Time;
            DateTime changeTime = DateTime.Now;
            TimeSpan timeDifference = selectedTime - changeTime;
            double minuteDifference = timeDifference.TotalMinutes;
            if (minuteDifference < 30)
            {
                timeErr = true;
                return RedirectToAction("RepDisbursementList", new { sessionId = sessionId, timeErr = timeErr });
            }

            disbursement.CollectionPoint = c;
            DisbursementListService.ChangeCollectionPoint(disbursement);
            return RedirectToAction("RepDisbursementList",new { sessionId = sessionId, timeErr = timeErr });
        }

        public ActionResult Acknowledge(string sessionId)
        {

            return RedirectToAction("Home", "Employee");
        }

    public class PerItem
        {
            public long itemId { get; set; }
            public int quantity { get; set; }
        }
    
    public class Entry
        {
            public long deptId { get; set; }
            public long itemId { get; set; }
            public int quantity { get; set; }
            public DateTime collectionDate { get; set; }
        }
    }
}