using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class PurchaseOrder
    {
        public long OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime OrderDate { get; set; }
        public long SupplierId { get; set; }
        public long EmployeeId { get; set; }
        public string DeliverTo { get; set; }
        public DateTime DeliverBy { get; set; }
        public Supplier Supplier { get; set; }
        public Employee Employee { get; set; }
        public List<PurchaseOrderDetails> ItemDetails { get; set; }
    }
}