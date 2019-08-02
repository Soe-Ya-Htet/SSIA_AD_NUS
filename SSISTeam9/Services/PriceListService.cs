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
                pricelist.Supplier1Name = SupplierDAO.GetSupplierByIdstring(pricelist.Supplier1Id).Name;
                pricelist.Supplier2Name = SupplierDAO.GetSupplierByIdstring(pricelist.Supplier2Id).Name;
                pricelist.Supplier3Name = SupplierDAO.GetSupplierByIdstring(pricelist.Supplier3Id).Name;
                return pricelist;
            }
            return null;
        }
    }
}