using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSISTeam9.DAO;
using SSISTeam9.Models;

namespace SSISTeam9.Services
{
    public class AdjVoucherService
    {
        public static void InsertReason(List<AdjVoucher> adjVouchers)
        {
            AdjVoucherDAO.InsertReason(adjVouchers, "");
        }
    }
}