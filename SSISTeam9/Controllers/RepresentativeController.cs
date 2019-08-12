using SSISTeam9.Models;
using SSISTeam9.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSISTeam9.Controllers
{
    public class RepresentativeController : Controller
    {
        // GET: Representative
        public ActionResult ChangeRepresentative(string employee,string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            long currentRep = DepartmentService.GetCurrentRep(deptId);
            if (employee != null)
            {
                long newRep = long.Parse(employee);
                RepresentativeService.UpdateEmployeeRole(newRep, currentRep, deptId);
               
            }
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            ViewData["employees"] = employees;
            ViewData["sessionId"] = sessionId;
            return View();
        }
    }
}