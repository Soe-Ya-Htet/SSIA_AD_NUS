﻿using SSISTeam9.DAO;
using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace SSISTeam9.Controllers
{
    public class DelegateController : Controller
    {
        private readonly IEmailService emailService;

        public DelegateController()
        {
            emailService = new EmailService();
        }
        // GET: Delegate
        public ActionResult Index()
        {
            return View();
        }
        
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
                EmailNotification notice = new EmailNotification();
                Employee MailReceiver = EmployeeService.GetEmployeeById(d.Employee.EmpId);
                notice.ReceiverMailAddress = MailReceiver.Email;
                notice.From = d.FromDate;
                notice.To = d.ToDate;
                Task.Run(() => emailService.SendMail(notice, EmailTrigger.ON_DELEGATED_AS_DEPT_HEAD));
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