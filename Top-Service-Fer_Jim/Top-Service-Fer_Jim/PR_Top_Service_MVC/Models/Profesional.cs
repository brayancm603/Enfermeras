using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Models
{
    public partial class Profesional
    {
        public Profesional()
        {
            Postulations = new HashSet<Postulation>();
            Services = new HashSet<Service>();
            Quotations = new HashSet<Quotation>();
            AreaProfesionals = new HashSet<AreaProfesional>();
        }

        public int IdProfesional { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime Birthdate { get; set; }
       
        public virtual Person? IdProfesionalNavigation { get; set; } = null!;
       
        public virtual ICollection<Postulation>? Postulations { get; set; }
        public virtual ICollection<Service>? Services { get; set; }
        public virtual ICollection<Quotation> Quotations { get; set; }
        public virtual ICollection<AreaProfesional> AreaProfesionals { get; set; }
    }
}
