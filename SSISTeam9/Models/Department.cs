using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class Department
    {
        public long DeptId { get; set; }

        [Display(Name = "Department Code")]
        [Required]
        public string DeptCode { get; set; }

        [Display(Name = "Department Name")]
        [Required]
        public string DeptName { get; set; }

        [Display(Name = "Contact Name")]
        [Required]
        public string Contact { get; set; }

        [Display(Name = "Telephone No.")]
        [Required]
        public string Telephone { get; set; }

        [Display(Name = "Fax No.")]
        [Required]
        public string Fax { get; set; }

        [Display(Name = "Head's Name")]
        [Required]
        public string Head { get; set; }
        public Employee Representative { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
    }
}