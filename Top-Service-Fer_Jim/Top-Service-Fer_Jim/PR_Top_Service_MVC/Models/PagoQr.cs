using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Models
{
    public partial class PagoQr
    {
        [Key]
        public int id { get; set; }
        public string ImageQr { get; set; }
    }
}
