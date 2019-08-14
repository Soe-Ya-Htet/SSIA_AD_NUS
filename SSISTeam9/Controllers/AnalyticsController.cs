using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSISTeam9.Services;
using SSISTeam9.Models;
using System.Threading.Tasks;
using SSISTeam9.Filters;

namespace SSISTeam9.Controllers
{
    [StoreAuthorisationFilter]
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

        public ActionResult Home(string sessionId)
        {
            List<string> analytics = new List<string>();
            analytics.Add("Requests By Department");
            analytics.Add("Requests By Stationery Category");
            analytics.Add("Orders By Stationery Category");
            analytics.Add("Orders By Supplier");

            ViewData["analytics"] = analytics;

            monthsToDisplay = new List<string>();
            for (int i = 1; i <= currentMonth; i++)
            {
                monthsToDisplay.Add(months[i]);
            }

            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult Redirect(FormCollection formCollection, string sessionId)
        {
            string analytic = formCollection["analytic"];

            if (analytic == "Requests By Department")
            {
                return RedirectToAction("Select", "Analytics", new { sessionid = sessionId});
            }
            else if (analytic == "Requests By Stationery Category")
            {
                return RedirectToAction("ByStationeryCategoryRequested", "Analytics", new { sessionid = sessionId }); 
            }
            else if (analytic == "Orders By Stationery Category")
            {
                return RedirectToAction("ByStationeryCategoryOrdered", "Analytics", new { sessionid = sessionId });
            }
            else
            {
                return RedirectToAction("DisplayChartBySupplier", "Analytics", new { sessionid = sessionId });
            }
        }

        public ActionResult Select(string sessionId)
        {
            List<string> departments = DepartmentService.GetAllDepartmentNames();
           
            ViewData["month"] = months[currentMonth];
            ViewData["year"] = currentYear;
            ViewData["departments"] = departments;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["monthsInInt"] = monthsInInt;

            ViewData["sessionId"] = sessionId;
            return View();
        }

        public async Task<ActionResult> DisplayChartByDept(FormCollection formCollection, string sessionId)
        {

            int month = monthsInInt[formCollection["month"]];
            int year = int.Parse(formCollection["year"]);

            ViewData["monthsInInt"] = monthsInInt;
            ViewData["month"] = month;
            ViewData["year"] = year;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["department"] = formCollection["department"];

            List<RequisitionDetails> reqs = AnalyticsService.GetRequisitionsByDept(formCollection["department"]);

            //Export reqs to CSV
            //StringWriter sw = new StringWriter();
            //sw.WriteLine("\"monthOfRequest\",\"yearOfRequest\",\"quantity\"");

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", "attachment;filename=dept_reqs.csv");
            //Response.ContentType = "text/csv";

            //foreach (var req in reqs)
            //{
            //    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\"",
            //                  req.MonthOfRequest,
            //                  req.YearOfRequest,
            //                  req.Quantity));
            //}

            //Response.Write(sw.ToString());
            //Response.End();

            Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear = AnalyticsService.GetTotalQuantitiesByMonthAndYear(reqs);
            //For months with no value, to fill it with 0.
            Dictionary<string, int> monthsAndQuantitiesForChart = AnalyticsService.FillEmptyData(totalQuantitiesByMonthAndYear, months, month, year);

            //Contact Python API to get predictions for next 3 months for only English Dept, Commerce Dept and Computer Science
            string data;
            string[] preds = new string[] { };
            if (formCollection["department"] == "English Dept" || formCollection["department"] == "Commerce Dept" || formCollection["department"] == "Computer Science")
            {
                data = await AnalyticsService.GetRequest("http://127.0.0.1:5000/" + formCollection["department"]);
                preds = data.Split(new char[] { ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            }

            //Put predictions in dictionary for displaying
            if (month == currentMonth && (formCollection["department"] == "English Dept"|| formCollection["department"] == "Commerce Dept" || formCollection["department"] == "Computer Science"))
            {
                if (month == 12)
                {
                    monthsAndQuantitiesForChart[months[month] + " " + year + " (Predicted)"] = (int)Math.Round(double.Parse(preds[0]));
                    monthsAndQuantitiesForChart[months[1] + " " + (year + 1) + " (Predicted)"] = (int)Math.Round(double.Parse(preds[1]));
                    monthsAndQuantitiesForChart[months[2] + " " + (year + 1) + " (Predicted)"] = (int)Math.Round(double.Parse(preds[2]));
                }
                else if (month == 11)
                {
                    monthsAndQuantitiesForChart[months[month] + " " + year + " (Predicted)"] = (int)Math.Round(double.Parse(preds[0]));
                    monthsAndQuantitiesForChart[months[month + 1] + " " + year + " (Predicted)"] = (int)Math.Round(double.Parse(preds[1]));
                    monthsAndQuantitiesForChart[months[1] + " " + (year + 1) + " (Predicted)"] = (int)Math.Round(double.Parse(preds[2]));
                }
                else
                {
                    monthsAndQuantitiesForChart[months[month] + " " + year + " (Predicted)"] = (int)Math.Round(double.Parse(preds[0]));
                    monthsAndQuantitiesForChart[months[month + 1] + " " + year + " (Predicted)"] = (int)Math.Round(double.Parse(preds[1]));
                    monthsAndQuantitiesForChart[months[month + 2] + " " + year + " (Predicted)"] = (int)Math.Round(double.Parse(preds[2]));
                }
            }
         
            ViewData["monthsAndQuantitiesForChart"] = monthsAndQuantitiesForChart;
            ViewData["sessionId"] = sessionId;

            return View();
        }

        public ActionResult ByStationeryCategoryRequested(FormCollection formCollection, string sessionId)
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
            ViewData["sessionId"] = sessionId;
            return View();
        }

        public ActionResult ByStationeryCategoryRequestedForSelectedMonth(FormCollection formCollection, string sessionId)
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
            ViewData["sessionId"] = sessionId;

            return View("ByStationeryCategoryRequested");
        }

        public ActionResult ByStationeryCategoryOrdered(FormCollection formCollection, string sessionId)
        {
            List<PurchaseOrderDetails> pos = AnalyticsService.GetOrderDetailsByStationeryCategory();

            //To get list of pair of category and quantity for a particular month
            Dictionary<string, double> dataForDisplay = AnalyticsService.GetCategoriesAmountsForMonth(pos, currentMonth, currentYear); 

            ViewData["dataForDisplay"] = dataForDisplay;

            ViewData["month"] = currentMonth;
            ViewData["year"] = currentYear;
            ViewData["monthsInInt"] = monthsInInt;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["chartTitle"] = "Total Amount Ordered By Stationery Category For " + months[currentMonth] + " " + currentYear;
            ViewData["sessionId"] = sessionId;

            return View();
        }

        public ActionResult ByStationeryCategoryOrderedForSelectedMonth(FormCollection formCollection, string sessionId)
        {
            List<PurchaseOrderDetails> pos = AnalyticsService.GetOrderDetailsByStationeryCategory();
            
            int month = monthsInInt[formCollection["month"]];
            //To get list of pair of category and quantity for a particular month
            Dictionary<string, double> dataForDisplay = AnalyticsService.GetCategoriesAmountsForMonth(pos, month, currentYear);

            ViewData["dataForDisplay"] = dataForDisplay;

            ViewData["month"] = month;
            ViewData["year"] = currentYear;
            ViewData["monthsInInt"] = monthsInInt;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["chartTitle"] = "Total Amount Ordered By Stationery Category For " + months[month] + " " + currentYear;
            ViewData["sessionId"] = sessionId;

            return View("ByStationeryCategoryOrdered");
        }

        public ActionResult DisplayChartBySupplier(FormCollection formCollection, string sessionId)
        {
            List<PurchaseOrderDetails> pos = AnalyticsService.GetOrderDetailsBySupplier();

            //Dictionary:<supplierName,totalPricePaid>
            Dictionary<string, double> dataForDisplay = AnalyticsService.GetSuppliersAndAmountsForMonth(pos, currentMonth, currentYear);
            
            ViewData["dataForDisplay"] = dataForDisplay;

            ViewData["month"] = currentMonth;
            ViewData["year"] = currentYear;
            ViewData["monthsInInt"] = monthsInInt;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["chartTitle"] = "Total Amount Purchased From Each Supplier For " + months[currentMonth] + " " + currentYear;
            ViewData["sessionId"] = sessionId;

            return View("DisplayChartBySupplier");
        }

        public ActionResult DisplayChartBySupplierForSelectedMonth(FormCollection formCollection, string sessionId)
        {
            List<PurchaseOrderDetails> pos = AnalyticsService.GetOrderDetailsBySupplier();
            
            int month = monthsInInt[formCollection["month"]];

            //Dictionary:<supplierName,totalPricePaid>
            Dictionary<string, double> dataForDisplay = AnalyticsService.GetSuppliersAndAmountsForMonth(pos, month, currentYear);

            ViewData["dataForDisplay"] = dataForDisplay;

            ViewData["month"] = month;
            ViewData["year"] = currentYear;
            ViewData["monthsInInt"] = monthsInInt;
            ViewData["monthsToDisplay"] = monthsToDisplay;
            ViewData["chartTitle"] = "Total Amount Purchased From Each Supplier For " + months[month] + " " + currentYear;
            ViewData["sessionId"] = sessionId;

            return View("DisplayChartBySupplier");
        }
    }
}