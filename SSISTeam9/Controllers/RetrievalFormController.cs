using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;
using SSISTeam9.DAO;

namespace SSISTeam9.Controllers
{
    public class RetrievalFormController : Controller
    {
        // GET: RetrievalForm
        public ActionResult ViewRetrievalForm()
        {
            
            ViewData["retrievalForms"] = RetrievalFormService.ViewRetrievalForm();
            if (DisbursementListService.CheckForPendingDisbursements().Count != 0)
            {
                ViewData["alreadyAssigned"] = "YES";
            }
            else
            {
                ViewData["alreadyAssigned"] = "NO";
            }
            return View();
        }

        public ActionResult CreateRetrievalForm(long[] requisition)
        {
            List<long> selected = new List<long>();
            foreach (var id in requisition)
            {
                selected.Add(id);
            }


            //using list to DAO search by list
            RetrievalFormService.UpdateStatuses(selected);

            return RedirectToAction("ViewRetrievalForm", "RetrievalForm");
        }  
    }
}