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
        public static int currentMonth = DateTime.Now.Month;
        public static int currentYear = DateTime.Now.Year;
        public static List<string> monthsToDisplay = new List<string>();
        public static Dictionary<int, string> months = new Dictionary<int, string>()
        {
            [1] = "Jan",
            [2] = "Feb",
            [3] = "Mar",
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

        public static Dictionary<string, int> monthsInInt = new Dictionary<string, int>
        {
            ["Jan"] = 1,
            ["Feb"] = 2,
            ["Mar"] = 3,
            ["Apr"] = 4,
            ["May"] = 5,
            ["Jun"] = 6,
            ["Jul"] = 7,
            ["Aug"] = 8,
            ["Sep"] = 9,
            ["Oct"] = 10,
            ["Nov"] = 11,
            ["Dec"] = 12,
        };

        public ActionResult Home()
        {
            List<string> analytics = new List<string>();
            analytics.Add("By Department");
            analytics.Add("By Stationery Category");
            analytics.Add("By Supplier");

            ViewData["analytics"] = analytics;

            monthsToDisplay = new List<string>();
            for (int i = 1; i <= currentMonth; i++)
            {
                monthsToDisplay.Add(months[i]);
            }

            return View();
        }

        public ActionResult Redirect(FormCollection formCollection)
        {
            string analytic = formCollection["analytic"];

            if (analytic == "By Department")
            {
                return RedirectToAction("Select", "Analytics");
            }
            else if (analytic == "By Stationery Category")
            {
                return RedirectToAction("DisplayChartByStationery", "Analytics"); 
            }
            else
            {
                return null;
            }
        }

        public ActionResult Select()
        {
            List<Department> departments = DepartmentService.GetAllDepartment();
           
            ViewData["month"] = months[currentMonth];
            ViewData["year"] = currentYear;
            ViewData["departments"] = departments;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["monthsInInt"] = monthsInInt;
            return View();
        }
        
        public ActionResult DisplayChartByDept(FormCollection formCollection)
        {
          
            int month = monthsInInt[formCollection["month"]];
            int year = int.Parse(formCollection["year"]);

            ViewData["monthsInInt"] = monthsInInt;
            ViewData["month"] = month;
            ViewData["year"] = year;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["department"] = formCollection["department"];
            
            List<RequisitionDetails> reqs = AnalyticsService.GetRequisitionsByDept(formCollection["department"]);
            
            Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear = AnalyticsService.GetTotalQuantitiesByMonthAndYear(reqs);
            //For months with no value, to fill it with 0.
            Dictionary<string, int> monthsAndQuantitiesForChart = AnalyticsService.FillEmptyData(totalQuantitiesByMonthAndYear, months, month, year);

            ViewData["monthsAndQuantitiesForChart"] = monthsAndQuantitiesForChart;

            return View();
        }

        public ActionResult DisplayChartByStationery(FormCollection formCollection)
        {
            List<RequisitionDetails> reqs = AnalyticsService.GetRequisitionsByStationeryCategory();

            //To get list of pair of category and quantity for a particular month
            Dictionary<string,int> dataForDisplay = new Dictionary<string, int>();

            foreach(var r in reqs)
            {
                if (r.MonthOfRequest == currentMonth && r.YearOfRequest == currentYear)
                {
                    dataForDisplay[r.Category] = r.Quantity;
                }
            }
            ViewData["dataForDisplay"] = dataForDisplay;

            ViewData["month"] = currentMonth;
            ViewData["year"] = currentYear;
            ViewData["monthsInInt"] = monthsInInt;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["chartTitle"] = "Total Quantity Requested By Stationery Category For " + months[currentMonth] + " " + currentYear;
            return View();
        }

        public ActionResult DisplayChartByStationeryBySelectedMonth(FormCollection formCollection)
        {
            List<RequisitionDetails> reqs = AnalyticsService.GetRequisitionsByStationeryCategory();

            //To get list of pair of category and quantity for a particular month
            Dictionary<string, int> dataForDisplay = new Dictionary<string, int>();

            int month = monthsInInt[formCollection["month"]];

            foreach (var r in reqs)
            {
                if (r.MonthOfRequest == month && r.YearOfRequest == currentYear)
                {
                    dataForDisplay[r.Category] = r.Quantity;
                }
            }
            ViewData["dataForDisplay"] = dataForDisplay;

            ViewData["month"] = month;
            ViewData["year"] = currentYear;
            ViewData["monthsInInt"] = monthsInInt;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["chartTitle"] = "Total Quantity Requested By Stationery Category For " + months[month] + " " + currentYear;

            return View("DisplayChartByStationery");
        }
    }
}