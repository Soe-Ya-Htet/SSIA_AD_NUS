using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class Supplier
    {
        public long SupplierId { get; set; }

        [Display(Name = "Supplier Code")]
        [Required]
        public string SupplierCode { get; set; }

        [Display(Name = "Supplier Name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "GST Registration Number")]
        [Required]
        public string GstNumber { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Contact Name")]
        [Required]
        public string ContactName { get; set; }

        [Display(Name = "Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        [Required]
        public string FaxNumber { get; set; }
    }
}