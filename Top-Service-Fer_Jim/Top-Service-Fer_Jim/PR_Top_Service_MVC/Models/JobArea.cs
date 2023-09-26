namespace PR_Top_Service_MVC.Models
{
    public class JobArea
    {

        public JobArea()
        {
            Postulations = new HashSet<Postulation>();
            
            
            AreaProfesionals = new HashSet<AreaProfesional>();

        }
        public byte idArea { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public byte deleted { get; set; }

        public virtual ICollection<Postulation>? Postulations { get; set; }
      
       
        public virtual ICollection<AreaProfesional> AreaProfesionals { get; set; }

    }
}
