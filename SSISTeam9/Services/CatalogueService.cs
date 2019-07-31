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
        public static bool VerifyCodeExist(string itemCode)
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

    }
}