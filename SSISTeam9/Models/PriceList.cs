using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class PriceList
    {
        public Inventory Item { get; set; }
        public long Supplier1Id { get; set; }
        public long Supplier2Id { get; set; }
        public long Supplier3Id { get; set; }

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
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Supplier1UnitPrice { get; set; }

        [Display(Name = "Unit Price from Second Supplier")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Supplier2UnitPrice { get; set; }

        [Display(Name = "Unit Price from Third Supplier")]
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public double Supplier3UnitPrice { get; set; }

        public string Supplier1Code { get; set; }
        public string Supplier2Code { get; set; }
        public string Supplier3Code { get; set; }
    }
}