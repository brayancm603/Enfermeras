using System.ComponentModel.DataAnnotations;
namespace PR_Top_Service_MVC.Models
{
    public class ServiceReceipt
    {
        [Key]
        public int IdService { get; set; }
        public int IdAdmin { get; set; }
        public int IdProfessional { get; set; }
        public int IdCostumer { get; set; }
       
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public byte Status { get; set; }
        public int IdReceipt { get; set; }
        public string DescriptionReceipt { get; set; } = null!;
        public decimal Total { get; set; }
        public virtual Admin? IdAdminNavigation { get; set; } = null!;
        public virtual Profesional? IdProfessionalNavigation { get; set; } = null!;
        public virtual Costumer? IdCostumerNavigation { get; set; } = null!;
    }
}
