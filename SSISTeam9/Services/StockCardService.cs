using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class StockCardService
    {
        public static List<StockCard> GetStockCardById(long ItemId)
        {
            List<StockCard> stockCards = StockCardDAO.GetStockCardById(ItemId);
            foreach(StockCard stockCard in stockCards)
            {
                switch (stockCard.SourceType)
                {
                    case 1:
                        stockCard.AdjVoucher.AdjId = stockCard.SourceId;
                        break;
                    case 2:
                        stockCard.DisbursementList.ListId = stockCard.SourceId;
                        break;
                    case 3:
                        stockCard.PurchaseOrder.OrderId = stockCard.SourceId;
                        stockCard.PurchaseOrder.SupplierId = PurchaseOrderService.GetOrderById(stockCard.PurchaseOrder.OrderId).SupplierId;
                        string SupplierCode = SupplierService.GetSupplierById(stockCard.PurchaseOrder.SupplierId).SupplierCode;
                        stockCard.Display = "Supplier - " + SupplierCode;
                        break;
                }
            }
            return stockCards;
        }

        public static void CreateStockCardFromDisburse(DisbursementListDetails disbursementDetails, DisbursementList disbursementList, int balance)
        {
            StockCardDAO.CreateStockCardFromDisburse(disbursementDetails, disbursementList, balance);
        }
    }
}