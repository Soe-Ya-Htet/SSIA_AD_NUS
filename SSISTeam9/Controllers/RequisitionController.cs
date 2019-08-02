﻿using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSISTeam9.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewRequisition()
        {
            List<Inventory> itemList = RequisitionService.GetAllInventory();
            ViewData["itemList"] = itemList;
            
            return View();
        }
        public ActionResult GetPendingRequisitions()
        {
            long deptId = 1;
            List<Requisition> requisitions = RequisitionService.DisplayPendingRequisitions(deptId);
            ViewData["requisitionsToProcess"] = requisitions;
            return View();
        }

        public ActionResult GetRequisitionDetails(long reqId)
        {
            List<RequisitionDetails> requisitionDetails = RequisitionService.DisplayRequisitionDetailsByReqId(reqId);
            ViewData["requisitionDetails"] = requisitionDetails;
            ViewData["reqId"] = reqId;
            return View();
        }

        public ActionResult ViewAllOutstandingRequisitions()
        {
            List<Requisition> requisitions = RequisitionService.ShowAllOutstandingRequisitionsByDate();
            ViewData["outstandingReqs"] = requisitions;
            return View();
            
        }


        public ActionResult CreateRetrievalForm(string[] requisition)
        {
            List<string> selected =  new List<string>();
            foreach (var id in requisition)
            {
                selected.Add(id);
            }

            ViewData["reqs"] = selected;

            return View();
        }
    }
}