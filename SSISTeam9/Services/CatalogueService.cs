﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class CatalogueService
    {
        //public static bool VerifyExist(string itemCode)
        //{
        //    List<string> catalogueCodes = CatalogueDAO.GetAllItemCodes();
        //    foreach(string code in catalogueCodes)
        //    {
        //        if(itemCode == code)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

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

        public static long CreateCatalogueDetaills(Inventory catalogue)
        {
            
            return CatalogueDAO.CreateCatalogue(catalogue);
            
        }

        public static void UpdateCatalogue(Inventory catalogue)
        {
            CatalogueDAO.UpdateCatalogue(catalogue);
        }

        public static List<string> GetAllUnits()
        {
            return CatalogueDAO.GetAllUnits();
        }

        public static List<string> GetAllCategories()
        {
            return CatalogueDAO.GetAllCategories();
        }

    }
}