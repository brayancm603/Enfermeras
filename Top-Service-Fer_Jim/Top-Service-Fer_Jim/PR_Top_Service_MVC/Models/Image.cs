using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Models
{
    public class Image
    {
        [Key]
        public int IdImages { get; set; }

        public int IdService { get; set; }

        public byte[] Imaged { get; set; } = null!;

        public virtual Service IdServiceNavigation { get; set; } = null!;
    }
}
