using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Models
{
    public partial class Comment
    {
        [Key]
        public int IdComment { get; set; }
        [Display(Name = "Comentario")]
        public string? Description { get; set; }
        public int IdProfessional { get; set; }

        public virtual Profesional? IdProfessionalNavigation { get; set; } = null!;
    }
}
