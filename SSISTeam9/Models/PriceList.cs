using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class PriceList
    {
        public string Supplier1Id { get; set; }
        public string Supplier2Id { get; set; }
        public string Supplier3Id { get; set; }

        [Display(Name = "First Supplier's Name")]
        [Required]
        public string Supplier1Name { get; set; }

        [Display(Name = "Second Supplier's Name")]
        [Required]
        public string Supplier2Name { get; set; }

        [Display(Name = "Third Supplier's Name")]
        [Required]
        public string Supplier3Name { get; set; }

        [Display(Name = "Unit Price from First Supplier")]
        [Required]
        public string Supplier1UnitPrice { get; set; }

        [Display(Name = "Unit Price from Second Supplier")]
        [Required]
        public string Supplier2UnitPrice { get; set; }

        [Display(Name = "Unit Price from Third Supplier")]
        [Required]
        public string Supplier3UnitPrice { get; set; }

        public Inventory Item { get; set; }
    }
}