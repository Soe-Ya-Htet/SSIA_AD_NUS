using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class StockService
    {
        public static List<Inventory> GetAllItems()
        {
            return CatalogueDAO.GetAllCatalogue();
        }
    }
}