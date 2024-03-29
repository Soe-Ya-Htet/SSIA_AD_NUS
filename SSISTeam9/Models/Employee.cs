﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSISTeam9.Models
{
    public class Employee
    {
        public long EmpId { get; set; }
        public long DeptId { get; set; }
        public string EmpName { get; set; }
        public string EmpRole { get; set; }
        public string EmpDisplayRole { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string Email { get; set; }

        public Department Department { get; set; }
        public string SessionId { get; set; }
    }
}