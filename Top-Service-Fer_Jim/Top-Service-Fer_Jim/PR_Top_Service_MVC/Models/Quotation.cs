using System;
using System.Collections.Generic;

namespace PR_Top_Service_MVC.Models
{
    public partial class Quotation
    {
        public int IdQuotation { get; set; }
        public int IdCostumer { get; set; }
        public int IdProfesional { get; set; }
        public DateTime Date { get; set; }
    
        public string Description { get; set; } = null!;
        public byte Status { get; set; }
        public virtual Costumer? IdCostumerNavigation { get; set; } = null!;
        public virtual Profesional? IdProfesionalNavigation { get; set; } = null!;
        
    }
}
