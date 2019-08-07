using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class StockCard
    {
        //SourceType: 1-Adjustment Voucher; 2-Disbursement List; 3-Purchase Order

        public long CardId { get; set; }
        public long ItemId { get; set; }
        public DateTime Date { get; set; }

        public int SourceType { get; set; }
        public long SourceId { get; set; }
        public AdjVoucher AdjVoucher { get; set; }
        public DisbursementList DisbursementList { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public string Display { get; set; }
        public string Qty { get; set; }
        public string Balance { get; set; }
    }
}