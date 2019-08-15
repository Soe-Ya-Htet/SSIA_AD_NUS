using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SSISTeam9.Controllers.DisbursementController;

namespace SSISTeam9.Models
{
    public class DisburmentDTO
    {
        public long ListId { get; set; }
        public string CollectionPoint { get; set; }
        public List<PerItem> Items { get; set; }
    }
}