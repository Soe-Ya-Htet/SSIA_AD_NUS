using SSISTeam9.DAO;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class DelegateController : Controller
    {
        // GET: Delegate
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult ViewDelegate(Models.Delegate d, string sessionId)
        //{
        //    Employee emp = EmployeeService.GetUserBySessionId(sessionId);
        //    long deptId = emp.DeptId;
        //    long headId = DepartmentService.GetCurrentHead(deptId);
        //    List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
        //    ViewData["employees"] = employees;
        //    ViewData["sessionId"] = sessionId;
        //    bool all = DelegateService.CheckPreviousHeadForNav(deptId);
        //    ViewData["all"] = all;
        //    if (d.Employee != null)
        //    {
        //        d.Department = new Department();
        //        d.Department.DeptId = deptId;
        //        DelegateService.AddNewDelegate(d, headId);
        //        return RedirectToAction("ViewRemoveDelegate", new { sessionId = sessionId });
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}

        //public ActionResult ViewRemoveDelegate(string headId, string sessionId)
        //{
        //    Employee emp = EmployeeService.GetUserBySessionId(sessionId);
        //    long deptId = emp.DeptId;
        //    long head = DepartmentService.GetCurrentHead(deptId);
        //    Employee e = EmployeeDAO.GetEmployeeById(head);
        //    ViewData["currentHead"] = e;
        //    ViewData["sessionId"] = sessionId;
        //    bool all = DelegateService.CheckPreviousHeadForNav(deptId);
        //    ViewData["all"] = all;
        //    if (headId == null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        DelegateService.DelegateToPreviousHead(deptId);
        //        return RedirectToAction("ViewDelegate", new { sessionId = sessionId });
        //    }
        //}
        public ActionResult ViewDelegate(Models.Delegate d, string delegatedhead, string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            long headId = DepartmentService.GetCurrentHead(deptId);
            Employee e = EmployeeDAO.GetEmployeeById(headId);
            ViewData["currentHead"] = e;
            ViewData["employees"] = employees;
            bool all = DelegateService.CheckPreviousHeadForNav(deptId);
            ViewData["all"] = all;
            ViewData["sessionId"] = sessionId;
            if (delegatedhead == null && d.Employee == null)
            {
                if (DelegateService.CheckDelegatedByDept(deptId))
                { ViewData["delegated"] = true; }
                else
                {
                    ViewData["delegated"] = false;
                }
                return View();
            }
            else if (delegatedhead == null && d.Employee != null)
            {
                d.Department = new Department();
                d.Department.DeptId = deptId;
                DelegateService.AddNewDelegate(d, headId);
                long head = DepartmentService.GetCurrentHead(deptId);
                Employee h = EmployeeDAO.GetEmployeeById(head);
                ViewData["currentHead"] = h;
                ViewData["delegated"] = true;
                return View();
            }
            else if (delegatedhead != null && d.Employee == null)
            {
                DelegateService.DelegateToPreviousHead(deptId);
                List<Employee> emps = RepresentativeService.GetEmployeesByDepartment(deptId);
                ViewData["employees"] = emps;
                ViewData["delegated"] = false;
                bool show = DelegateService.CheckPreviousHeadForNav(deptId);
                ViewData["all"] = show;
                return View();
            }
            else
            {
                ViewData["delegated"] = false;
                return View();
            }
            
        }
    }
}