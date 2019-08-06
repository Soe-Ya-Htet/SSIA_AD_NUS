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

            return View(itemList);
        }

        public ActionResult AddtoCart(int itemId)
        {
            Cart cart = new Cart();
            Employee emp = new Employee();
            emp.EmpId = 1;
            Inventory item = new Inventory();
            item.ItemId = itemId;
            cart.Employee = emp;
            cart.Item = item;
            cart.Quantity = 1;
            RequisitionService.SaveToCart(cart);

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RequisitionList()
        {
            List<Cart> empCarts =  RequisitionService.GetCartsByEmpId(1);
            return View(empCarts);
        }

        public ActionResult CreateRequisition(int empId)
        {
            List<Cart> empCarts = RequisitionService.GetCartsByEmpId((long)empId);
            List<Cart> cartsToRequest = new List<Cart>();
            foreach (string key in Request.Form.AllKeys)
            {
                var value = Request[key];
                int quantity = Convert.ToInt32(value);
                long itemId = Convert.ToInt64(key);
                Cart cart = empCarts.Find(c => c.Item.ItemId == itemId) ;
                cart.Quantity = quantity;
                cartsToRequest.Add(cart);
            }

            //long empId = 1;
            RequisitionService.CreateRequisition(cartsToRequest, empId);
            return RedirectToAction("NewRequisition","Requisition");
        }

        public ActionResult MyRequisition(int empId)
        {

            return View();
        }
        
        public ActionResult DeptRequisition(int empId)
        {

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

        public ActionResult ProcessRequisition(long reqId, string status)
        {
            long currentHead = 2;
            RequisitionService.ProcessRequisition(reqId, status, currentHead);
            return RedirectToAction("GetPendingRequisitions");
        }

        public ActionResult ViewAllOutstandingRequisitions()
        {
            List<Requisition> requisitions = RequisitionService.ShowAllOutstandingRequisitionsByDate();
            ViewData["outstandingReqs"] = requisitions;
            return View();
            
        }


        
    }
}