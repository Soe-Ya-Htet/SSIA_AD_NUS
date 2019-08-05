using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;


namespace SSISTeam9.Controllers
{
    public class AnalyticsController : Controller
    {
        public static Dictionary<int, string> months = new Dictionary<int, string>()
        {
            [1] = "Jan",
            [2] = "Feb",
            [3] = "March",
            [4] = "Apr",
            [5] = "May",
            [6] = "Jun",
            [7] = "Jul",
            [8] = "Aug",
            [9] = "Sep",
            [10] = "Oct",
            [11] = "Nov",
            [12] = "Dec",

        };

        public static int currentMonth = DateTime.Now.Month;

        public static List<string> monthsToDisplay = new List<string>();

        public ActionResult Index(FormCollection formCollection, int month, int year, string department)
        {

            for (int i = 1; i <= currentMonth; i++)
            {
                monthsToDisplay.Add(months[i]);
            }

            ViewData["month"] = month;
            ViewData["year"] = year;

            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["department"] = formCollection["selected_dept"];
            return View();
        }

        public ActionResult Select()
        {
            DateTime today = DateTime.Now;

            List <Department> departments = DepartmentService.GetAllDepartment();

            ViewData["month"] = today.Month;
            ViewData["year"] = today.Year;
            ViewData["departments"] = departments;

            return View();
        }

        public ActionResult SelectMonth(FormCollection formCollection, int year, string department)
        {
            int month = months
                           .FirstOrDefault(x => x.Value != null && x.Value.Contains(formCollection["selected_month"]))
                           .Key;
            ViewData["month"] = month;
            ViewData["year"] = year;
            ViewData["department"] = department;

            ViewData["monthsToDisplay"] = monthsToDisplay;

            return View("Index");
        }
    }
}