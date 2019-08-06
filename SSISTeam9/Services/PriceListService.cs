using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class PriceListService
    {
        public static PriceList GetPriceListByItemId(long itemId)
        {
            PriceList pricelist = new PriceList();
            pricelist = PriceListDAO.GetPriceListById(itemId);
            if(pricelist != null)
            {
                pricelist.Supplier1Name = SupplierDAO.GetSupplierById(pricelist.Supplier1Id).Name;
                pricelist.Supplier2Name = SupplierDAO.GetSupplierById(pricelist.Supplier2Id).Name;
                pricelist.Supplier3Name = SupplierDAO.GetSupplierById(pricelist.Supplier3Id).Name;
                return pricelist;
            }
            return null;
        }

        public static void CreatePriceListDetaills(PriceList priceList)
        {
            priceList.Supplier1Id = SupplierService.GetSupplierId(priceList.Supplier1Name);
            priceList.Supplier2Id = SupplierService.GetSupplierId(priceList.Supplier2Name);
            priceList.Supplier3Id = SupplierService.GetSupplierId(priceList.Supplier3Name);

            PriceListDAO.CreatePriceList(priceList);
        }

        public static void DeletePriceList(long itemId)
        {
            PriceListDAO.DeletePriceList(itemId);
        }

        public static List<PriceList> GetPriceListByItemIds(List<long> itemIds)
        {
            List<PriceList> priceLists = new List<PriceList>();

            foreach(var itemId in itemIds)
            {
                priceLists.Add(PriceListDAO.GetPriceListById(itemId));
            }

            return priceLists;
        }

        public static void UpdatePriceList(PriceList priceList)
        {
            priceList.Supplier1Id = SupplierService.GetSupplierId(priceList.Supplier1Name);
            priceList.Supplier2Id = SupplierService.GetSupplierId(priceList.Supplier2Name);
            priceList.Supplier3Id = SupplierService.GetSupplierId(priceList.Supplier3Name);
            PriceListDAO.UpdatePriceList(priceList);
        }
    }
}