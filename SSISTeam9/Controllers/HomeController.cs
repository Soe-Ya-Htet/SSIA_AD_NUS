using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Models;
using SSISTeam9.Services;

namespace SSISTeam9.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string userName, string password)
        {
            if (userName == null)
                return View();

            Employee user = EmployeeService.GetUserPassword(userName);

            if (user.Password != password)
                return View();

            string sessionId = EmployeeService.CreateSession(userName);

            return RedirectToAction("All", "Home", new { sessionid = sessionId });
        }

        public ActionResult Logout(string sessionId)
        {
            EmployeeService.RemoveSession(sessionId);
            return RedirectToAction("Login", "Home");
        }

        public ActionResult All(string sessionid)
        {
            if (sessionid == null)
            {
                RedirectToAction("Login");
            }

            // for login employee sessin data
            Employee emp = EmployeeService.GetUserBySessionId(sessionid);
            string empRole = emp.EmpRole;
            string userName = emp.UserName;
            string empDisplayRole = emp.EmpDisplayRole;

            if (empRole == "STORE_CLERK" || empRole == "STORE_SUPERVISOR" || empRole == "STORE_MANAGER")
            {
                ViewData["userName"] = userName;
                ViewData["sessionId"] = sessionid;
                return View("~/Views/StoreLandingPage/Home.cshtml");
            }
            else if ((empRole == "EMPLOYEE" || empRole == "REPRESENTATIVE") && (empDisplayRole != "HEAD"))
            {
                return RedirectToAction("NewRequisition", "Requisition", new { sessionId = sessionid});
            }
            else if ((empRole=="HEAD" && empDisplayRole=="HEAD"))
            {
                return RedirectToAction("GetPendingRequisitions","Requisition",new { sessionId=sessionid});
            }
            else if ((empRole=="EMPLOYEE" && empDisplayRole=="HEAD"))
            {
                bool between = DelegateService.CheckDate(emp.DeptId);
                bool after = DelegateService.AfterDate(emp.DeptId);
                if (between && !after)
                {
                    return RedirectToAction("ViewDelegate", "Delegate", new { sessionId = sessionid });
                }
                else if(!between && !after)
                {
                    return RedirectToAction("GetPendingRequisitions", "Requisition", new { sessionId = sessionid });
                }
                else if(!between && after)
                {
                    DelegateService.DelegateToPreviousHead(emp.DeptId);
                    return RedirectToAction("GetPendingRequisitions", "Requisition", new { sessionId = sessionid });
                }
                else {
                    return RedirectToAction("GetPendingRequisitions", "Requisition", new { sessionId = sessionid });
                }
            }
            else if((empRole=="HEAD" && empDisplayRole == "EMPLOYEE"))
            {
                bool between = DelegateService.CheckDate(emp.DeptId);
                bool after = DelegateService.AfterDate(emp.DeptId);
                if (between && !after)
                {
                    return RedirectToAction("GetPendingRequisitions", "Requisition", new { sessionId = sessionid });
                }
                else if (!between && !after)
                {
                    return RedirectToAction("NewRequisition", "Requisition", new { sessionId = sessionid });
                }
                else if (!between && after)
                {
                    DelegateService.DelegateToPreviousHead(emp.DeptId);
                    return RedirectToAction("NewRequisition", "Requisition", new { sessionId = sessionid });
                }
                else
                {
                    return RedirectToAction("NewRequisition", "Requisition", new { sessionId = sessionid });
                }

            }
            else
            {
                ViewData["userName"] = userName;
                ViewData["sessionId"] = sessionid;
                return null; //For departments' head landing page
            }
        }

        public ActionResult NotAuthorised()
        {
            return View();
        }
    }
}