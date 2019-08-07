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
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@".\S+.", ErrorMessage = "No white space allowed")]
        public string SupplierCode { get; set; }

        [Display(Name = "Supplier Name")]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "GST Registration Number")]
        [Required(AllowEmptyStrings = false)]
        public string GstNumber { get; set; }

        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; }

        [Display(Name = "Contact Name")]
        [Required(AllowEmptyStrings = false)]
        public string ContactName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Fax Number")]
        [Required(AllowEmptyStrings = false)]
        public string FaxNumber { get; set; }
    }
}