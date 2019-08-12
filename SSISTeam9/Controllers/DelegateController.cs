﻿using SSISTeam9.DAO;
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
        public ActionResult ViewDelegate(Models.Delegate d,string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            long headId = DepartmentService.GetCurrentHead(deptId);
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            ViewData["employees"] = employees;
            ViewData["sessionId"] = sessionId;
            if (d.Employee != null)
            {
                d.Department = new Department();
                d.Department.DeptId = deptId;
                DelegateService.AddNewDelegate(d,headId);
                return RedirectToAction("ViewRemoveDelegate",new { sessionId=sessionId});
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult ViewRemoveDelegate(string headId,string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            long head = DepartmentService.GetCurrentHead(deptId);
            Employee e = EmployeeDAO.GetEmployeeById(head);
            ViewData["currentHead"] = e;
            ViewData["sessionId"] = sessionId;
            if (headId == null)
            {
                return View();
            }
            else
            {
                DelegateService.DelegateToPreviousHead(deptId);
                return RedirectToAction("ViewDelegate",new { sessionId=sessionId});
            }
        }
    }
}