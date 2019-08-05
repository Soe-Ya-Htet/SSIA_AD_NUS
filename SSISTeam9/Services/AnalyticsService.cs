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

        public static Dictionary<Tuple<int, int>, List<Tuple<long, int>>> GetItemQuantitiesByMonth(List<RequisitionDetails> reqs)
        {
            Dictionary<Tuple<int, int>, List<Tuple<long, int>>> itemQuantitiesByMonthAndYear = new Dictionary<Tuple<int, int>, List<Tuple<long, int>>>();

            foreach(var req in reqs)
            {
                Tuple<int, int> monthYear = new Tuple<int, int>(req.DateOfRequest.Date.Month, req.DateOfRequest.Date.Year);

                if (itemQuantitiesByMonthAndYear.ContainsKey(monthYear))
                {
                    itemQuantitiesByMonthAndYear[monthYear].Add(new Tuple<long, int>(req.ItemId, req.Quantity));
                }
                else
                {
                    List<Tuple<long, int>> itemsIdsAndQuantites = new List<Tuple<long, int>>();
                    itemQuantitiesByMonthAndYear.Add(monthYear, itemsIdsAndQuantites);
                    itemQuantitiesByMonthAndYear[monthYear].Add(new Tuple<long, int>(req.ItemId, req.Quantity));
                }
            }
            return itemQuantitiesByMonthAndYear;
        }

        public static Dictionary<Tuple<int, int>, int> GetTotalQuantitiesByMonth(Dictionary<Tuple<int, int>, List<Tuple<long, int>>> itemQuantitiesByMonthAndYear)
        {
            Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear = new Dictionary<Tuple<int, int>, int>();
            int quantity = 0;

            foreach (KeyValuePair<Tuple<int, int>, List<Tuple<long, int>>> r in itemQuantitiesByMonthAndYear)
            {
                foreach (var qty in r.Value)
                {
                    quantity += qty.Item2;
                }
                totalQuantitiesByMonthAndYear[r.Key] =  quantity;
                quantity = 0;
            }
            return totalQuantitiesByMonthAndYear;
        }

        public static Dictionary<string, int> FormatDataForChart(Dictionary<Tuple<int, int>, int> totalQuantitiesByMonthAndYear, Dictionary<int, string> months, int month, int year)
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
                        monthsAndQuantitiesForChart[12.ToString() + " " + (year - 1)] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(12, year - 1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[12.ToString() + " " + (year - 1)] = 0;
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
                        monthsAndQuantitiesForChart[11.ToString() + " " + (year - 1)] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(11, year - 1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[11.ToString() + " " + (year - 1)] = 0;
                    }
                    try
                    {
                        monthsAndQuantitiesForChart[12.ToString() + " " + (year - 1)] = totalQuantitiesByMonthAndYear[new Tuple<int, int>(12, year - 1)];
                    }
                    catch (KeyNotFoundException)
                    {
                        monthsAndQuantitiesForChart[12.ToString() + " " + (year - 1)] = 0;
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
    }
}