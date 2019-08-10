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

            if (user.EmpRole == "STORE_CLERK")
            {
                return RedirectToAction("Home", "StoreClerk", new { username = userName, sessionid = sessionId });
            }
            else if (user.EmpRole == "STORE_SUPERVISOR" || user.EmpRole == "STORE_MANAGER")
            {
                return RedirectToAction("Home", "StoreMS", new { username = userName, sessionid = sessionId });
            }
            return null;
        }

        public ActionResult Logout(string sessionId)
        {
            EmployeeService.RemoveSession(sessionId);
            return RedirectToAction("Login", "Home");
        }
    }
}