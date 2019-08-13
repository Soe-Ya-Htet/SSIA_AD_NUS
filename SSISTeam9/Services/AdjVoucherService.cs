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

        public static List<AdjVoucher> GetAdjByStatus(int status)
        {
            List<AdjVoucher> adjVouchers = new List<AdjVoucher>();
            adjVouchers = AdjVoucherDAO.GetAdjByStatus(status);
            if(adjVouchers != null)
            {
                foreach(AdjVoucher adj in adjVouchers)
                {
                    adj.ItemCode = CatalogueService.GetCatalogueById(adj.ItemId).ItemCode;
                }
            }

            return adjVouchers;
        }

        public static void UpdateReason(AdjVoucher adjVoucher)
        {
            AdjVoucherDAO.UpdateReason(adjVoucher);
        }

        public static void UpdateStatus(long adjId, int status)
        {
            AdjVoucherDAO.UpdateStatus(adjId, status);
        }

        public static void AuthoriseBy(long adjId, long empId)
        {
            AdjVoucherDAO.AuthoriseBy(adjId,empId);
        }
    }
}