using SSISTeam9.DAO;
using SSISTeam9.Filters;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SSISTeam9.Controllers.DisbursementController;

namespace SSISTeam9.Controllers
{
    [BasicAuthenticationAttribute]
    [RoutePrefix("rest/stock_clerk")]
    public class RestStockClerkController : Controller
    {
        private readonly IRestService restService;

        public RestStockClerkController(IRestService restService)
        {
            this.restService = restService;
        }

        [Route("retrievals")]
        public ActionResult RetrievalForms()
        {
            return Json(restService.GetAllRetrievalFormsOfStockClerk(), JsonRequestBehavior.AllowGet);
        }

        [Route("disbursement/generate")]
        public ActionResult GenerateDisbursement(List<Entry> entries)
        {
            return Json(restService.GenerateDisbursementOfStockClerk(entries), JsonRequestBehavior.AllowGet);
        }

        [Route("inventory/all")]
        public ActionResult GetAllInventories()
        {
            return Json(restService.GetAllInventories(), JsonRequestBehavior.AllowGet);
        }

        [Route("inventory/price_list")]
        public ActionResult GetInventoryPriceList(List<long> itemIds)
        {
            return Json(restService.GetAllInventoryPriceListByIds(itemIds), JsonRequestBehavior.AllowGet);
        }

        [Route("inventory/adjustment")]
        public ActionResult Generate(List<AdjVoucher> adjVouchers)
        {
            return Json(restService.SubmitStockAdjustment(adjVouchers), JsonRequestBehavior.AllowGet);
        }

    }
}