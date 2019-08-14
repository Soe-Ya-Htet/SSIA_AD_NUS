using SSISTeam9.DAO;
using SSISTeam9.Filters;
using SSISTeam9.Models;
using SSISTeam9.Services;
using SSISTeam9.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [BasicAuthenticationAttribute]
    [RoutePrefix("rest/representative")]
    public class RestRepresentativeController : Controller
    {
        //private readonly int repId = 1;
        //private readonly int deptId = 1;

        private readonly IRestService restService;

        public RestRepresentativeController(IRestService restService)
        {
            this.restService = restService;
        }

        [Route("disbursement/acknowledge/{id:long}")]
        public ActionResult Acknowledge(long id)
        {
            /* id = listId; The following method is for update ChargeBack and StockCard*/

            DisbursementList disbursementList = DisbursementListService.GetDisbursementListByListId(id);
            List<DisbursementListDetails> disbursementDetails = DisbursementListService.ViewDisbursementDetails(id);

            
            foreach(DisbursementListDetails d in disbursementDetails)
            {
                /*The following code is for ChargeBack table*/
                //By the time disburse item, calculate the amount of this list, update ChargeBack table
                PriceList priceList = PriceListService.GetPriceListByItemId(d.Item.ItemId);
                double price = 0;
                if (priceList != null)
                {
                    price = priceList.Supplier1UnitPrice;
                }

                double amount = price * d.Quantity;

                ChargeBackService.UpdateChargeBackData(amount, disbursementList);

                /*The following code is for StockCard table*/
                //By the time disburse item, update StockCard table with itemId, deptId and date, souceType = 2

                int balance = CatalogueService.GetCatalogueById(d.Item.ItemId).StockLevel - d.Quantity;
                StockCardService.CreateStockCardFromDisburse(d, disbursementList, balance);

                ////following code will update and close requisitions
                //int disbursedAmount = d.Quantity;
                //List<Requisition> requisitions = RequisitionService.GetOutstandingRequisitionsAndDetailsByDeptIdAndItemId(disbursementList.Department.DeptId, d.Item.ItemId); //will get those status assigned/partially completed(assigned)

                //    foreach (var requisition in requisitions)
                //    {
                        
                //        if (requisition.Requisition.disbursedAmount <= disbursedAmount)// if the balance is less than what was disbursed
                //        {
                             
                //            RequisitionService.UpdateBalanceAmount(requisition.ReqId, d.Item.ItemId, 0);//change balance to 0

                //            if(RequisitionService.GetRequisitionDetailsByReqId(requisition.ReqId) != null) //will get those the remaining amounts !=0 if 
                //            {
                //                RequisitionService.UpdateStatus(requisition.ReqId, "Partially Completed");
                //            }
                //            else
                //            {
                //                RequisitionService.UpdateStatus(requisition.ReqId, "Completed");
                //            }
                //            disbursedAmount -= requisition.Requisition.disbursedAmount; // minusing the balance from what was disbursed
                //        }
                //        else// when the balance amount is more than the remainder of the disbursed amount
                //        {
                //            RequisitionService.UpdateBalanceAmount(requisition.ReqId, d.Item.ItemId, disbursedAmount);// change balance to remainder of disbursed amount
                           
                //            RequisitionService.UpdateStatus(requisition.ReqId, "Partially Completed");
                            
                //            break;//break out of for loop when disbursed amount become 0
                //        }
                //    }
                
                
               
            }
            

            

            return Json(restService.AcknowledgementOfRepresentative(id), JsonRequestBehavior.AllowGet);
        }

        [Route("disbursements/pending")]
        public ActionResult GetAllPendingDisbursements()
        {
            return Json(restService.GetAllPendingDisbursementsOfRep(), JsonRequestBehavior.AllowGet);
        }

        [Route("disbursements/past")]
        public ActionResult GetAllPastDisbursements()
        {
            return Json(restService.GetAllPastDisbursementsOfRep(), JsonRequestBehavior.AllowGet);
        }

        [Route("disbursement/{listId:long}")]
        public ActionResult GetAllPendingDisbursementDetailsList(long listId)
        {
            return Json(restService.GetAllDisbursementDetailsByIdOfRep(listId), JsonRequestBehavior.AllowGet);
        }

    }
}