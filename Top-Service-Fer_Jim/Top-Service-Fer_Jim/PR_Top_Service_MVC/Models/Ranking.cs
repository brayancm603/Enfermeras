namespace PR_Top_Service_MVC.Models
{
    public class Ranking
    {
        public int IdProfessional { get; set; }
        public double PromedioRating { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public Profesional? Profesional { get; set; }
    }
}
