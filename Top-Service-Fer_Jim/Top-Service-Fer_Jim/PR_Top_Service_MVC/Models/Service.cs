using System;
using System.Collections.Generic;

namespace PR_Top_Service_MVC.Models
{

    public enum Status
    {
        Terminado,
        Aceptado,
        En_Proceso,
    }
    public partial class Service
    {

        public static string ReturnToStatus(int num)
        {
            return ((Status)num).ToString().Replace("_", " ");
        }

        public int IdService { get; set; }
        public int IdAdmin { get; set; }
        public int IdProfessional { get; set; }
        
       
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public byte Status { get; set; }
        public int IdCostumer{ get; set; }

        public byte? Status_Service { get; set; }        
        public DateTime? DateTime_On_Service { get; set; }
        public DateTime? DateTime_Off_Service { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public byte deleted { get; set; }

        public virtual Admin? IdAdminNavigation { get; set; } = null!;
        public virtual Profesional? IdProfessionalNavigation { get; set; } = null!;
        public virtual Costumer? IdCostumerNavigation { get; set; } = null!;
        public virtual Receipt? Receipt { get; set; }
        
    }
}
