﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;

namespace SSISTeam9.Controllers
{
    public class PurchaseOrderController : Controller
    {
        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult All()
        {
            List<PurchaseOrder> orders = PurchaseOrderService.GetAllOrders();

            ViewData["orders"] = orders;
            return View();
        }

        public ActionResult Edit(string orderNumber)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderNumber);

            ViewData["order"] = order;
            return View();
        }

        public ActionResult ChooseSuppliers(PurchaseOrder order, string orderNumber)
        {

            Console.WriteLine(orderNumber);
            PurchaseOrder checkedItemsInOrder = PurchaseOrderService.GetOrderDetails(orderNumber);
            
            for (int i = 0; i < order.ItemDetails.Count; i++)
            {
                checkedItemsInOrder.ItemDetails[i].Item.isChecked = order.ItemDetails[i].Item.isChecked;
            }

            ViewData["order"] = checkedItemsInOrder;

            return View();
        }

        public ActionResult Close(string orderNumber)
        {
            PurchaseOrder order = PurchaseOrderService.GetOrderDetails(orderNumber);

            ViewData["order"] = order;
            return View();
        }
    }
}