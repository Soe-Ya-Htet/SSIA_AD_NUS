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
        public static List<string> monthsToDisplay = new List<string>();
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

        public ActionResult Select()
        {
            DateTime today = DateTime.Now;

            List<Department> departments = DepartmentService.GetAllDepartment();

            ViewData["month"] = today.Month;
            ViewData["year"] = today.Year;
            ViewData["departments"] = departments;

            return View();
        }
        
        public ActionResult DisplayChartByDept(FormCollection formCollection, int month, int year)
        {
            
            for (int i = 1; i <= currentMonth; i++)
            {
                monthsToDisplay.Add(months[i]);
            }

            ViewData["month"] = month;
            ViewData["year"] = year;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["selected_dept"] = formCollection["selected_dept"];
            
            List<RequisitionDetails> reqs = AnalyticsService.GetRequisitionsByDept(formCollection["selected_dept"]);

            //Dictionary: Key: Tuple<Month,Year>; Value: Tuple<itemId,quantity>
            Dictionary<Tuple<int, int>, List<Tuple<long, int>>> itemQuantitiesByMonthAndYear = AnalyticsService.GetItemQuantitiesByMonth(reqs);

            //To sum quantity for all item ids by month, year
            Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear = AnalyticsService.GetTotalQuantitiesByMonth(itemQuantitiesByMonthAndYear);

            Dictionary<string, int> monthsAndQuantitiesForChart = AnalyticsService.FormatDataForChart(totalQuantitiesByMonthAndYear, months, month,year);
            
            ViewData["monthsAndQuantitiesForChart"] = monthsAndQuantitiesForChart;
            return View();
        }
        
    }
}