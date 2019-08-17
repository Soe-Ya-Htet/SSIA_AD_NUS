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
    public class RepresentativeController : Controller
    {
        private readonly IEmailService emailService;

        public RepresentativeController()
        {
            emailService = new EmailService();
        }

        // GET: Representative
        public ActionResult ChangeRepresentative(string employee,string sessionId)
        {
            Employee emp = EmployeeService.GetUserBySessionId(sessionId);
            long deptId = emp.DeptId;
            long currentRep = DepartmentService.GetCurrentRep(deptId);
            bool all = DelegateService.CheckPreviousHeadForNav(deptId);
            bool permanentHead = ((emp.EmpRole == "HEAD" && emp.EmpDisplayRole == "HEAD") || (emp.EmpRole == "EMPLOYEE" && emp.EmpDisplayRole == "HEAD"));
            ViewData["all"] = all;
            ViewData["permanentHead"] = permanentHead;
            if (employee != null)
            {
                long newRep = long.Parse(employee);
                EmailNotification notice = new EmailNotification();
                RepresentativeService.UpdateEmployeeRole(newRep, currentRep, deptId);
                Employee newRepMailReceiver = EmployeeService.GetEmployeeById(newRep);
                Employee oldRepMailReceiver = EmployeeService.GetEmployeeById(currentRep);
                Task.Run(() => {
                    notice.ReceiverMailAddress = newRepMailReceiver.Email;
                    emailService.SendMail(notice, EmailTrigger.ON_ASSIGNED_AS_DEPT_REP);
                    notice.ReceiverMailAddress = oldRepMailReceiver.Email;
                    emailService.SendMail(notice, EmailTrigger.ON_REMOVED_DEPT_REP);
                });

            }
            List<Employee> employees = RepresentativeService.GetEmployeesByDepartment(deptId);
            ViewData["employees"] = employees;
            ViewData["sessionId"] = sessionId;
            return View();
        }
    }
}