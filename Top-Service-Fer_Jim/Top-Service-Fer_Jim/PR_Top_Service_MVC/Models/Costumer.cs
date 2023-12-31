﻿using System;
using System.Collections.Generic;

namespace PR_Top_Service_MVC.Models
{
    public partial class Costumer
    {
        public Costumer()
        {
            Quotations = new HashSet<Quotation>();
            Services = new HashSet<Service>();
            Ratings = new HashSet<Rating>();
        }

        public int IdCostumer { get; set; }
        public string Address { get; set; } = null!;
        public string Ci{ get; set; }
        public double? Latitude { get; set; }
        public double? Longitud { get; set; }
        public string? CellphoneNumber { get; set; }
        public virtual Person IdCostumerNavigation { get; set; } = null!;
        public virtual ICollection<Quotation> Quotations { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
