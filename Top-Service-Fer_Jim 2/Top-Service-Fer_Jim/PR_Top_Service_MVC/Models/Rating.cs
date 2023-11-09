namespace PR_Top_Service_MVC.Models
{
    public class Rating
    {
        public int IdRating { get; set; }
        public int rating { get; set; }
        public int IdCostumer { get; set; }
        public int IdProfesional { get; set; }
        public string? Description { get; set; } = null!;
        public virtual Profesional? IdProfesionalNavigation { get; set; }
        public virtual Costumer? IdCostumerNavigation { get; set; }
    }
}
