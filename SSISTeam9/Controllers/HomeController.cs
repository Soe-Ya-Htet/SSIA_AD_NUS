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

        public ActionResult Login(string UserName, string Password)
        {
            if (UserName == null)
                return View();

            Employee user = EmployeeService.GetUserPassword(UserName);

            string sessionId = Session.CreateSession(UserName);

            return RedirectToAction("ViewProducts", "Gallery", new { username = UserName, sessionid = SessionId });
        }

        public ActionResult Logout(string sessionId)
        {
            ShoppingCart.Models.Session.RemoveSession(sessionId);
            return RedirectToAction("Login", "Home");
        }
    }
}