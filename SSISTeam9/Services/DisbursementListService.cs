using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class DisbursementListService
    {

        public static List<DisbursementList> ViewOutstandingDisbursementsByCollection(string collectionPt)
        {
            List<DisbursementList> disbursementLists = DisbursementListDAO.ViewDisbursements(collectionPt);
            return disbursementLists;
        }

        public static List<DisbursementListDetails> ViewDisbursementDetails(long listId)
        {
            List<DisbursementListDetails> disbursementListDetails = DisbursementListDetailsDAO.ViewDetails(listId);
            return disbursementListDetails;
        }
    }
}