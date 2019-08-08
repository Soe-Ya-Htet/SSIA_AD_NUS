using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;
namespace SSISTeam9.Controllers
{
    public class DisbursementController : Controller
    {
        // GET: Disbursement
        public ActionResult ViewAllDisbursements(string collectionPt)
        {
            ViewData["disbursements"] = DisbursementListService.ViewOutstandingDisbursementsByCollection(collectionPt);
            ViewData["collectionPoint"] = collectionPt;
            
            return View();
        }

        public ActionResult ViewDisbursementDetails(long listId, long collectionPt)
        {
            
            ViewData["details"] = DisbursementListService.ViewDisbursementDetails(listId);
            ViewData["listId"] = listId;
            ViewData["collectionPt"] = collectionPt;

            return View();

        }

        public ActionResult CreateDisbursementLists(List<Entry> entries)
        {
            List<long> deptIds = new List<long>();
            
            List<DisbursementList> disbursementLists = new List<DisbursementList>();

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
                    DisbursementListDetails = disbursementListDetails
                    
                };
                

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

            return Json(Url.Action("ViewAllDisbursements","Disbursement"));
        }

        public ActionResult UpdateDisbursementLists(long listId, long collectionPt, List<PerItem> items)
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
            }

            
            return Json(Url.Action("ViewAllDisbursements", "Disbursement", new {collectionPt = collectionPt }));
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
        }
    }
}