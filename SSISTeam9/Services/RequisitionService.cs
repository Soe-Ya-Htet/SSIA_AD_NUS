using SSISTeam9.DAO;
using SSISTeam9.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Services
{
    public class RequisitionService
    {
        public static List<Requisition> DisplayPendingRequisitions()
        {
            return RequisitionDAO.GetPendingRequisitionsFromDB();
        }

    }
}