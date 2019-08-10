﻿using System;
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

            return RedirectToAction("All", "Home", new { username = userName, sessionid = sessionId });
        }

        public ActionResult Logout(string sessionId)
        {
            EmployeeService.RemoveSession(sessionId);
            return RedirectToAction("Login", "Home");
        }

        public ActionResult All(string username, string sessionid)
        {
            if (sessionid == null)
            {
                RedirectToAction("Login");
            }

            string empRole = EmployeeService.GetUserBySessionId(sessionid).EmpRole;

            if (empRole == "STORE_CLERK")
            {
                ViewData["userName"] = username;
                ViewData["sessionId"] = sessionid;
                return View("~/Views/StoreClerk/Home.cshtml");
            }
            else if (empRole == "STORE_SUPERVISOR" || empRole == "STORE_MANAGER")
            {
                ViewData["userName"] = username;
                ViewData["sessionId"] = sessionid;
                return View("~/Views/StoreMS/Home.cshtml");
            }
            else
            {
                ViewData["userName"] = username;
                ViewData["sessionId"] = sessionid;
                return null; //For other departments' employees landing page
            }
        }

        public ActionResult NotAuthorised()
        {
            return View();
        }
    }
}