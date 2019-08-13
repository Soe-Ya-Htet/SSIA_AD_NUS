using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using SSISTeam9.DAO;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    [StoreAuthorisationFilter]
    public class RetrievalFormController : Controller
    {
        // GET: RetrievalForm
        public ActionResult ViewRetrievalForm(string sessionId)
        {
            List<RetrievalForm> retrievalForms = RetrievalFormService.ViewRetrievalForm();
            ViewData["retrievalForms"] = retrievalForms;
            if (DisbursementListService.CheckForPendingDisbursements().Count != 0)
            {
                ViewData["alreadyAssigned"] = "YES";
            }
            else
            {
                ViewData["alreadyAssigned"] = "NO";
            }
            List<int> stockLevel = new List<int>();
            foreach (var item in retrievalForms)
            {
                Inventory i= CatalogueService.GetCatalogueById(item.itemId);
                stockLevel.Add(i.StockLevel);
            }
            ViewData["stockLevel"] = stockLevel;
            ViewData["sessionId"] = sessionId;
            
            return View();
        }

        public ActionResult CreateRetrievalForm(long[] requisition, string sessionId)
        {
            List<long> selected = new List<long>();
            foreach (var id in requisition)
            {
                selected.Add(id);
            }


            //using list to DAO search by list
            RetrievalFormService.UpdateStatuses(selected);

            return RedirectToAction("ViewRetrievalForm", "RetrievalForm", new { sessionId = sessionId });
        }  
    }
}