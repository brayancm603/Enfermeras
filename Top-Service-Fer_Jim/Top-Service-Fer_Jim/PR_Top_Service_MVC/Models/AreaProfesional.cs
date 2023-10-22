using System.ComponentModel.DataAnnotations;
namespace PR_Top_Service_MVC.Models
{
    public class AreaProfesional
    {
        [Key]
        public int IdProfesional { get; set; }
        public byte idArea { get; set; }

        public virtual Profesional IdProfesionalNavigation { get; set; } 
        public virtual JobArea IdAreaNavigation { get; set; } 
    }
}
