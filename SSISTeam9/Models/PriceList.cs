﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSISTeam9.Models
{
    public class PriceList
    {
        public string Supplier1Id { get; set; }
        public string Supplier2Id { get; set; }
        public string Supplier3Id { get; set; }
        public string Supplier1Name { get; set; }
        public string Supplier2Name { get; set; }
        public string Supplier3Name { get; set; }
        public string Supplier1UnitPrice { get; set; }
        public string Supplier2UnitPrice { get; set; }
        public string Supplier3UnitPrice { get; set; }
        public Inventory Item { get; set; }
    }
}