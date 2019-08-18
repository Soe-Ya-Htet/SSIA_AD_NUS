using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class EmailNotification
    {
        public Department Dept { get; set; }

        public List<Inventory> Items { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string ReceiverMailAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public PurchaseOrder Order { get; set; }

        public string CollectionDate { get; set; }

    }
}