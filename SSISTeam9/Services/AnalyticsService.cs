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

        public static List<PurchaseOrderDetails> GetOrderDetailsByStationeryCategory()
        {
            List<PurchaseOrderDetails> pos = AnalyticsDAO.GetOrderDetailsByStationeryCategory();
            List<long> itemIds = new List<long>();

            foreach (var po in pos)
            {
                itemIds.Add(po.ItemId);
            }

            List<PriceList> priceLists = PriceListService.GetPriceListByItemIds(itemIds);

            //To get the unit price paid for the order, which may not have been provided by main supplier for that item
            //This is to calculate the total amount paid for the chart
            for (int i = 0; i < priceLists.Count; i++)
            {
                if (pos[i].SupplierId == priceLists[i].Supplier1Id)
                {
                    pos[i].UnitPricePaid = double.Parse(priceLists[i].Supplier1UnitPrice);
                }
                else if (pos[i].SupplierId == priceLists[i].Supplier2Id)
                {
                    pos[i].UnitPricePaid = double.Parse(priceLists[i].Supplier2UnitPrice);
                }
                else if (pos[i].SupplierId == priceLists[i].Supplier3Id)
                {
                    pos[i].UnitPricePaid = double.Parse(priceLists[i].Supplier3UnitPrice);
                }
            }
            return pos;
        }

        public static Dictionary<string, double> GetCategoriesAmountsForMonth(List<PurchaseOrderDetails> pos, int month, int year)
        {
            Dictionary<string, double> categoriesAmounts = new Dictionary<string, double>();

            foreach(var po in pos)
            {
                if (po.MonthOfOrder == month && po.YearOfOrder == year)
                {
                    if (categoriesAmounts.ContainsKey(po.ItemCategory))
                    {
                        double update = categoriesAmounts[po.ItemCategory] + Math.Round(po.Quantity * po.UnitPricePaid, 2);
                        categoriesAmounts[po.ItemCategory] = update;
                    }
                    else
                    {
                        categoriesAmounts[po.ItemCategory] = Math.Round(po.Quantity * po.UnitPricePaid, 2);
                    }
                }
            }
            return categoriesAmounts;
        }

        public static List<PurchaseOrderDetails> GetOrderDetailsBySupplier()
        {
            List<PurchaseOrderDetails> pos = AnalyticsDAO.GetOrderDetailsBySupplier();

            List<long> itemIds = new List<long>();

            foreach (var po in pos)
            {
                itemIds.Add(po.ItemId);
            }
            List<PriceList> priceLists = PriceListService.GetPriceListByItemIds(itemIds);

            //To get the unit price paid for the order, which may not have been provided by main supplier for that item
            //This is to calculate the total amount paid for the chart
            for (int i = 0; i < priceLists.Count; i++)
            {
                if (pos[i].SupplierId == priceLists[i].Supplier1Id)
                {
                    pos[i].UnitPricePaid = double.Parse(priceLists[i].Supplier1UnitPrice);
                }
                else if (pos[i].SupplierId == priceLists[i].Supplier2Id)
                {
                    pos[i].UnitPricePaid = double.Parse(priceLists[i].Supplier2UnitPrice);
                }
                else if (pos[i].SupplierId == priceLists[i].Supplier3Id)
                {
                    pos[i].UnitPricePaid = double.Parse(priceLists[i].Supplier3UnitPrice);
                }
            }
            return pos;
        }

        public static Dictionary<string, double> GetSuppliersAndAmountsForMonth(List<PurchaseOrderDetails> pos, int month, int year)
        {
            Dictionary<string, double> supsAmtsForMonth = new Dictionary<string, double>();
            
            foreach (var po in pos)
            {
                if(po.MonthOfOrder == month && po.YearOfOrder == year)
                {
                    if (supsAmtsForMonth.ContainsKey(po.SupplierName))
                    {
                        double update = supsAmtsForMonth[po.SupplierName] + Math.Round(po.Quantity * po.UnitPricePaid, 2);
                        supsAmtsForMonth[po.SupplierName] = update;
                    }
                    else
                    {
                        supsAmtsForMonth[po.SupplierName] = Math.Round(po.Quantity * po.UnitPricePaid, 2);
                    }
                }
            }
            return supsAmtsForMonth;
        }
    }
}