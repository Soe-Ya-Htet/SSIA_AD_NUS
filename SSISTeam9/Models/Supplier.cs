using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class Supplier
    {
        public long SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string GstNumber { get; set; }
        public string Address { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}