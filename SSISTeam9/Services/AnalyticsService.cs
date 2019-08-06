using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class AnalyticsService
    {
        public static List<RequisitionDetails> GetRequisitionsByDept(string department)
        {
            return AnalyticsDAO.GetRequisitionsByDept(department);
        }
        
        public static Dictionary<Tuple<int, int>, int> GetTotalQuantitiesByMonthAndYear(List<RequisitionDetails> reqs)
        {
            Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear = new Dictionary<Tuple<int, int>, int>();
            
            foreach (var r in reqs)
            {
                totalQuantitiesByMonthAndYear[new Tuple<int,int>(r.MonthOfRequest,r.YearOfRequest)] =  r.Quantity;
            }
            return totalQuantitiesByMonthAndYear;
        }

        public static Dictionary<string, int> FillEmptyData(Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear, Dictionary<int, string> months, int month, int year)
        {
            Dictionary<string, int> monthsAndQuantitiesForChart = new Dictionary<string, int>();

            if (month > 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        monthsAndQuantitiesForChart[months[(month - 2)] + " " + year] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(month - 2, year)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[months[(month - 2)] + " " + year] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart[months[(month - 1)] + " " + year] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(month - 1, year)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[months[(month - 1)] + " " + year] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart[months[(month)] + " " + year] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(month, year)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[months[(month)] + " " + year] = 0;
                    }
                }
            }
            else if (month == 2)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        monthsAndQuantitiesForChart["Dec " + (year - 1)] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(12, year - 1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart["Dec " + (year - 1)] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart[months[(month - 1)] + " " + year] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(month - 1, year)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[months[(month - 1)] + " " + year] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart[months[(month)] + " " + year] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(month, year)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[months[(month)] + " " + year] = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {

                    try
                    {
                        monthsAndQuantitiesForChart["Nov " + (year - 1)] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(11, year - 1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart["Nov " + (year - 1)] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart["Dec " + (year - 1)] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(12, year - 1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart["Dec " + (year - 1)] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart[months[(month)] + " " + year] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(month, year)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[months[(month)] + " " + year] = 0;
                    }
                }
            }
            return monthsAndQuantitiesForChart;
        }

        public static List<RequisitionDetails> GetRequisitionsByStationeryCategory()
        {
            return AnalyticsDAO.GetRequisitionsByStationeryCategory();
        }
    }
}