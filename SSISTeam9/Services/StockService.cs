using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;
using System.Threading.Tasks;

namespace SSISTeam9.Services
{
    public class StockService
    {
        public async static Task<List<Inventory>> GetAllItemsOrdered()
        {
            List<Inventory> items = StockDAO.GetAllItemsOrdered();

            //Contact Python API to get predicted re-order amount and level for item with code 'P021'
            string data;
            string[] preds = new string[] { };

            foreach (var item in items)
            {
                if (item.ItemCode == "P021")
                {
                    data = await AnalyticsService.GetRequest("http://127.0.0.1:5000/reorder/" + item.ItemCode);
                    preds = data.Split(new char[] { ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    item.ReorderQty = (int)Math.Round(double.Parse(preds[0]));
                    item.ReorderLevel = (int)Math.Round(double.Parse(preds[1]));
                }
            }
            return items;
        }

        public static List<long> GetItemsFirstSupplierIds(List<long> itemIds)
        {
            return StockDAO.GetItemsFirstSupplierIds(itemIds);
        }

        public static long GetItemId(string itemCode)
        {
            return StockDAO.GetItemId(itemCode);
        }

        public static List<Inventory> GetPendingOrderQuantities(List<Inventory> items)
        {
            return StockDAO.GetPendingOrderQuantities(items);
        }

        public static void CreatePurchaseOrders(string empId, List<long> itemIds, List<long> itemsFirstSupplierIds, List<int> quantities)
        {
            //Key: SupplierId
            //Value: List of Tuple of Item Id and Corresponding Quantity
            Dictionary<long, List<Tuple<long, int>>> uniquePurchaseOrders = new Dictionary<long, List<Tuple<long, int>>>();
            string newOrderNumber;

            //To group items with same alt supplier together into one Purchase Order
            for (int i = 0; i < itemIds.Count; i++)
            {
                if (quantities[i] > 0)
                {
                    if (uniquePurchaseOrders.ContainsKey(itemsFirstSupplierIds[i]))
                    {
                        uniquePurchaseOrders[itemsFirstSupplierIds[i]].Add(new Tuple<long, int>(itemIds[i], quantities[i]));
                    }
                    else
                    {
                        List<Tuple<long, int>> selectedItemsIdsAndQuantites = new List<Tuple<long, int>>();
                        uniquePurchaseOrders.Add(itemsFirstSupplierIds[i], selectedItemsIdsAndQuantites);
                        uniquePurchaseOrders[itemsFirstSupplierIds[i]].Add(new Tuple<long, int>(itemIds[i], quantities[i]));
                    }
                }
            }

            foreach (KeyValuePair<long, List<Tuple<long, int>>> po in uniquePurchaseOrders)
            {
                //Create new record under PurchaseOrder table.
                newOrderNumber = PurchaseOrderDAO.GetNewOrderNumber();

                StockDAO.CreatePurchaseOrder(po.Key, po.Value, newOrderNumber,long.Parse(empId));
                
            }
        }

    }
}