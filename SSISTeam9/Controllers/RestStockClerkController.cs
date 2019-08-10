using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SSISTeam9.Controllers.DisbursementController;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("rest/stock_clerk")]
    public class RestStockClerkController : Controller
    {
        [Route("retrievals")]
        public ActionResult Index()
        {
            List<RetrievalForm> retrievalForms = RetrievalFormService.ViewRetrievalForm();

            Dictionary<string, List<RetrievalForm>> retrievalDict = new Dictionary<string, List<RetrievalForm>>
            {
                { "retrievalForms", retrievalForms }
            };

            return Json(retrievalDict, JsonRequestBehavior.AllowGet);
        }

        [Route("disbursement/generate")]
        public ActionResult GenerateDisbursement(List<Entry> entries)
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
                    if (entry.deptId == deptId)
                    {
                        Inventory i = new Inventory()
                        {
                            ItemId = entry.itemId,


                        };
                        DisbursementListDetails dDets = new DisbursementListDetails()
                        {
                            Item = i,
                            Quantity = entry.quantity

                        };

                        d.DisbursementListDetails.Add(dDets);
                    }
                }
                disbursementLists.Add(d);
            }
            DisbursementListService.CreateDisbursementLists(disbursementLists);

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

    }
}