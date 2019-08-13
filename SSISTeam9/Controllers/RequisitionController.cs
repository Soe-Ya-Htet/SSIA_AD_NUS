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
            string desc = "";
            string cat = "";
            desc = null == Request.Form["desSearch"]? "" : Request.Form["desSearch"];
            cat = null == Request.Form["catSearch"] ? "" : Request.Form["catSearch"];
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<Inventory> itemList = RequisitionService.ShowItems(desc, cat);
            List<string> category = new List<string>();
            category = RequisitionService.GetALLCategories();
            ViewData["emp"] = emp;
            ViewData["sessionId"] = sessionId;
            ViewData["itemList"] = itemList;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            ViewData["category"] = category;

            return View(itemList);
        }
        
        [DepartmentFilter]
        public ActionResult AddtoCart(int itemId, string sessionId)
        {
            string qty = Request.Form["itemQuantity"] == "" ? "1" : Request.Form["itemQuantity"];
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            RequisitionService.SaveToCart(itemId, emp.EmpId, Convert.ToInt32(qty));
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
            List<Requisition> reqList = RequisitionService.GetRequisitionByEmpId(emp.EmpId);
            reqList.Sort((x, y) => DateTime.Compare(x.DateOfRequest, y.DateOfRequest));
            return View(reqList);
        }

        [DepartmentFilter]
        public ActionResult DeptRequisition(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            ViewData["sessionId"] = sessionId;
            ViewData["isRep"] = (emp.EmpRole == "REPRESENTATIVE");
            List<Requisition> reqList = RequisitionService.GetRequisitionByEmpId(emp.EmpId);
            reqList.Sort((x, y) => DateTime.Compare(x.DateOfRequest, y.DateOfRequest));
            return View(reqList);
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
        public ActionResult GetPendingRequisitions(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            List<Requisition> requisitions = RequisitionService.DisplayPendingRequisitions(deptId);
            bool all = DelegateService.CheckPreviousHeadForNav(deptId);
            bool permanentHead =((emp.EmpRole=="HEAD" && emp.EmpDisplayRole=="HEAD") || (emp.EmpRole == "EMPLOYEE" && emp.EmpDisplayRole == "HEAD"));
            ViewData["all"] = all;
            ViewData["permanentHead"] = permanentHead;
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
            bool all = DelegateService.CheckPreviousHeadForNav(emp.DeptId);
            bool permanentHead = ((emp.EmpRole == "HEAD" && emp.EmpDisplayRole == "HEAD") || (emp.EmpRole == "EMPLOYEE" && emp.EmpDisplayRole == "HEAD"));
            ViewData["all"] = all;
            ViewData["permanentHead"] = permanentHead;
            return View();
        }

        [DepartmentFilter]
        public ActionResult ViewPastRequisitions(string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            List<Requisition> requisitions = RequisitionService.DisplayPastRequisitions(deptId);
            bool all = DelegateService.CheckPreviousHeadForNav(deptId);
            bool permanentHead = ((emp.EmpRole == "HEAD" && emp.EmpDisplayRole == "HEAD") || (emp.EmpRole == "EMPLOYEE" && emp.EmpDisplayRole == "HEAD"));
            ViewData["all"] = all;
            ViewData["permanentHead"] = permanentHead;
            ViewData["pastRequisitions"] = requisitions;
            ViewData["sessionId"] = sessionId;
            return View();
        }

        [DepartmentFilter]
        public ActionResult ViewPastRequisitionDetails(long reqId, string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            List<RequisitionDetails> requisitionDetails = RequisitionService.DisplayRequisitionDetailsByReqId(reqId);
            bool all = DelegateService.CheckPreviousHeadForNav(emp.DeptId);
            bool permanentHead = ((emp.EmpRole == "HEAD" && emp.EmpDisplayRole == "HEAD") || (emp.EmpRole == "EMPLOYEE" && emp.EmpDisplayRole == "HEAD"));
            ViewData["all"] = all;
            ViewData["permanentHead"] = permanentHead;
            ViewData["requisitionDetails"] = requisitionDetails;
            ViewData["sessionId"] = sessionId;
            ViewData["reqId"] = reqId;
            return View();
        }

        [DepartmentFilter]
        public ActionResult ProcessRequisition(long reqId, string status, string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long currentHead = emp.EmpId;
            RequisitionService.ProcessRequisition(reqId, status, currentHead);
            bool all = DelegateService.CheckPreviousHeadForNav(emp.DeptId);
            bool permanentHead = ((emp.EmpRole == "HEAD" && emp.EmpDisplayRole == "HEAD") || (emp.EmpRole == "EMPLOYEE" && emp.EmpDisplayRole == "HEAD"));
            ViewData["all"] = all;
            ViewData["permanentHead"] = permanentHead;
            ViewData["sessionId"] = sessionId;
            return RedirectToAction("GetPendingRequisitions",new { sessionId = sessionId });
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