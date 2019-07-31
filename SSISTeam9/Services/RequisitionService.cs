using SSISTeam9.DAO;
using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class RequisitionService
    {
        public static List<Inventory> GetAllInventory()
        {
            return CatalogueDAO.DisplayAllCatalogue();
        }
        public static List<Requisition> DisplayPendingRequisitions()
        {
            return RequisitionDAO.GetPendingRequisitionsFromDB();
        }

    }
}