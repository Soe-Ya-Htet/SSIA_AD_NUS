using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class CatalogueService
    {
        public static bool VerifyExist(string itemCode)
        {
            List<string> catalogueCodes = CatalogueDAO.GetAllItemCodes();
            foreach(string code in catalogueCodes)
            {
                if(itemCode == code)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Inventory> GetAllCatalogue()
        {
            return CatalogueDAO.GetAllCatalogue();
        }

        public static Inventory GetCatalogueById(long itemId)
        {
            return CatalogueDAO.GetCatalogueById(itemId);
        }

        public static void DeleteCatalogue(long itemId)
        {
            CatalogueDAO.DeleteCatalogue(itemId);
        }

        public static void CreateCatalogueDetaills(Inventory catalogue, List<string> supplierCodes)
        {
            string itemCode = catalogue.ItemCode.ToUpper();
            catalogue.ItemCode = itemCode;

            CatalogueDAO.CreateCatalogue(catalogue);
            CatalogueDAO.CreatePriceList(catalogue.ItemId);
            int number = 1;
            foreach(string supplierCode in supplierCodes)
            {
                CatalogueDAO.UpdatePriceList(catalogue.ItemId, supplierCode, number);
                number++;
            }
        }

        public static void UpdateCatalogue(Inventory catalogue, List<string> supplierCodes)
        {
            string itemCode = catalogue.ItemCode.ToUpper();
            catalogue.ItemCode = itemCode;

            CatalogueDAO.UpdateCatalogue(catalogue);
            int number = 1;
            foreach (string supplierCode in supplierCodes)
            {
                CatalogueDAO.UpdatePriceList(catalogue.ItemId, supplierCode, number);
                number++;
            }
        }

    }
}