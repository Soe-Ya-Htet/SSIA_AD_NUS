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
            List<RetrievalForm> retrievalForms = RetrievalFormService.ViewRetrievalForm(); 
            ViewData["retrievalForms"] = retrievalForms;
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
            RequisitionDAO.UpdateApprovedStatusByIdList(selected);
            RequisitionDAO.UpdatePartiallyCompletedStatusByIdList(selected);

            return RedirectToAction("ViewRetrievalForm", "RetrievalForm");
        }  
    }
}