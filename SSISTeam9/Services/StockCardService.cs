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
                        stockCard.AdjVoucher = new AdjVoucher
                        {
                            AdjId = stockCard.SourceId
                        };
                        stockCard.Display = "Stock Adjustment";
                        break;
                    case 2:
                        stockCard.DisbursementList = new DisbursementList()
                        {
                            Department = new Department()
                            {
                                DeptId = stockCard.SourceId
                            }
                        };
                        Department department = DepartmentService.GetDepartmentById(stockCard.DisbursementList.Department.DeptId);
                        stockCard.Display = department.DeptName;
                        break;
                    case 3:
                        stockCard.PurchaseOrder = new PurchaseOrder()
                        {
                            OrderId = stockCard.SourceId                          
                        };
                        stockCard.PurchaseOrder.SupplierId = PurchaseOrderService.GetOrderById(stockCard.PurchaseOrder.OrderId).SupplierId;
                        Supplier supplier = SupplierService.GetSupplierById(stockCard.PurchaseOrder.SupplierId);
                        if(supplier.SupplierCode == null)
                        {
                            supplier.SupplierCode = "Nil";
                        }
                        stockCard.Display = "Supplier - " + supplier.SupplierCode;
                        break;
                }
            }
            return stockCards;
        }

        public static void CreateStockCardFromDisburse(DisbursementListDetails disbursementDetails, DisbursementList disbursementList, int balance)
        {
            StockCardDAO.CreateStockCardFromDisburse(disbursementDetails, disbursementList, balance);
        }


        public static void CreateStockCardFromOrder(PurchaseOrder order, List<long> itemIds, List<int> itemQuantities)
        {
            int totalQuantity = itemQuantities.Sum(m => m);
            if (totalQuantity != 0)
            {
                for (int i = 0; i < itemIds.Count; i++)
                {
                    if (itemQuantities[i] != 0)
                    {
                        StockCard stockCard = new StockCard();
                        stockCard.Date = DateTime.Now;
                        stockCard.ItemId = itemIds[i];
                        stockCard.SourceId = order.SupplierId;
                        stockCard.Qty = "+ " + itemQuantities[i];
                        stockCard.Balance = CatalogueService.GetCatalogueById(itemIds[i]).StockLevel + itemQuantities[i];
                        StockCardDAO.CreateStockCardFromOrder(stockCard);
                    }
                }
            }

        }

    }
}