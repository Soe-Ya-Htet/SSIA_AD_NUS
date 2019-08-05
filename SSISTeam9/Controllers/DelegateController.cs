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
        public ActionResult ViewDelegate(Models.Delegate d)
        {
            long deptId = 1;
            long headId = DepartmentService.GetCurrentHead(deptId);
            if (d.Employee != null)
            {
                d.Department = new Department();
                d.Department.DeptId = deptId;
                DelegateService.AddNewDelegate(d,headId);
            }
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            ViewData["employees"] = employees;
            return View();
        }
    }
}