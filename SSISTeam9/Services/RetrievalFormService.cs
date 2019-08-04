using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;

namespace SSISTeam9.Services
{
    public class RetrievalFormService
    {
        public static List<RetrievalForm> ViewRetrievalForm()
        {
            List<RetrievalForm> retrievalForms = new List<RetrievalForm>();
            retrievalForms = RetrievalFormDAO.GetItemAndQuantity();

            foreach (var item in retrievalForms)
            {
                item.deptNeeds = RetrievalFormDAO.GetDeptNeeds(/*requisition,*/ item.itemId);
            }

            return retrievalForms;
        }

       
    }
}