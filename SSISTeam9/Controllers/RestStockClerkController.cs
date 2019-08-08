using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    [RoutePrefix("rest/stock_clerk")]
    public class RestStockClerkController : Controller
    {
        [Route("retrievals")]
        public ActionResult Index()
        {
            List<RetrievalForm> retrievalForms = RetrievalFormService.ViewRetrievalForm();

            Dictionary<string, List<RetrievalForm>> retrievalDict = new Dictionary<string, List<RetrievalForm>>();
            retrievalDict.Add("retrievalForms", retrievalForms);

            return Json(retrievalDict, JsonRequestBehavior.AllowGet);
        }
    }
}