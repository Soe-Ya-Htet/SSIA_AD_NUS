using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.Models;
using SSISTeam9.DAO;
using System.Threading.Tasks;

namespace SSISTeam9.Services
{
    public class AdjVoucherService
    {
        public static void CreateAdjVoucher(long adjId, long itemId, int qty)
        {
            AdjVoucherDAO.CreateAdjVoucher(adjId, itemId, qty);
        }

        public static long? GetLastId()
        {
            return AdjVoucherDAO.GetLastId();
        }

        public static List<AdjVoucher> GetUnauthorisedAdj()
        {
            return AdjVoucherDAO.GetUnauthorisedAdj();
        }
    }
}