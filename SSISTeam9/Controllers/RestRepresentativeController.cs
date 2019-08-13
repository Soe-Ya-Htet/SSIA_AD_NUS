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
                PriceList priceList = PriceListService.GetPriceListByItemId(id);
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