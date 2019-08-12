using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        public ActionResult Index()
        {
            return View();
        }
        
        [DepartmentFilter]
        public ActionResult NewRequisition(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<Inventory> itemList = RequisitionService.GetAllInventory();
            ViewData["emp"] = emp;
            ViewData["sessionId"] = sessionId;
            ViewData["itemList"] = itemList;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");

            return View(itemList);
        }
        
        [DepartmentFilter]
        public ActionResult AddtoCart(int itemId, string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            int quantity = 1;
            RequisitionService.SaveToCart(itemId, emp.EmpId, quantity);
            return RedirectToAction("NewRequisition", "Requisition", new { sessionId = sessionId });
        }

        [DepartmentFilter]
        public ActionResult RequisitionList(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<Cart> empCarts =  RequisitionService.GetCartsByEmpId(emp.EmpId);
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            return View(empCarts);
        }

        [DepartmentFilter]
        public ActionResult CreateRequisition(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<Cart> empCarts = RequisitionService.GetCartsByEmpId(emp.EmpId);
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

            RequisitionService.CreateRequisition(cartsToRequest, emp.EmpId);
            return RedirectToAction("NewRequisition", "Requisition", new { sessionId = sessionId });
        }

        [DepartmentFilter]
        public ActionResult MyRequisition(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            return View(RequisitionService.GetRequisitionByEmpId(emp.EmpId));
        }

        [DepartmentFilter]
        public ActionResult DeptRequisition(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            return View(RequisitionService.GetRequisitionByDeptId(emp.DeptId));
        }

        [DepartmentFilter]
        public ActionResult Reorder(int reqId,string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            RequisitionService.ReorderCart(reqId, emp.EmpId);
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            return RedirectToAction("RequisitionList", "Requisition", new { sessionId = sessionId });
        }

        [DepartmentFilter]
        public ActionResult GetPendingRequisitions()
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            List<Requisition> requisitions = RequisitionService.DisplayPendingRequisitions(deptId);
            ViewData["requisitionsToProcess"] = requisitions;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        [DepartmentFilter]
        public ActionResult GetRequisitionDetails(long reqId,string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<RequisitionDetails> requisitionDetails = RequisitionService.DisplayRequisitionDetailsByReqId(reqId);
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            ViewData["requisitionDetails"] = requisitionDetails;
            ViewData["reqId"] = reqId;
            return View();
        }

        [DepartmentFilter]
        public ActionResult ViewPastRequisitions()
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            List<Requisition> requisitions = RequisitionService.DisplayPastRequisitions(deptId);
            ViewData["pastRequisitions"] = requisitions;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        [DepartmentFilter]
        public ActionResult ViewPastRequisitionDetails(long reqId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<RequisitionDetails> requisitionDetails = RequisitionService.DisplayRequisitionDetailsByReqId(reqId);
            ViewData["requisitionDetails"] = requisitionDetails;
            ViewData["sessionId"] = sessionId;
            ViewData["reqId"] = reqId;
            return View();
        }

        [DepartmentFilter]
        public ActionResult ProcessRequisition(long reqId, string status)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long currentHead = emp.EmpId;
            RequisitionService.ProcessRequisition(reqId, status, currentHead);
            ViewData["sessionId"] = sessionId;
            return RedirectToAction("GetPendingRequisitions");
        }

        [StoreAuthorisationFilter]
        public ActionResult ViewAllRequisitionsByStatus(string status,string sessionId)
        {
            List<Requisition> requisitions = RequisitionService.ShowAllRequisitionsByStatusByDate(status);
            if (DisbursementListService.CheckForPendingDisbursements().Count != 0)
            {
                ViewData["alreadyAssigned"] = "YES";
            }
            else
            {
                ViewData["alreadyAssigned"] = "NO";
            }
            ViewData["Reqs"] = requisitions;
            ViewData["status"] = status;
            ViewData["sessionId"] = sessionId;
            return View();
        }
    }
}