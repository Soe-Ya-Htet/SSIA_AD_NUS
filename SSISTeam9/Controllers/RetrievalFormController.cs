using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class RetrievalFormController : Controller
    {
        // GET: RetrievalForm
        public ActionResult ViewRetrievalForm()
        {
            return View();
        }

        public ActionResult CreateRetrievalForm(string[] requisition)
        {
            List<string> selected = new List<string>();
            foreach (var id in requisition)
            {
                selected.Add(id);
            }

            ViewData["reqs"] = selected;
            //using list to DAO search by list

            return RedirectToAction("ViewRetrievalForm", "RetrievalForm");
        }  
    }
}