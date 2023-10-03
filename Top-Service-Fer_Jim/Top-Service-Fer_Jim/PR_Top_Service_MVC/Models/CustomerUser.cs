using System.ComponentModel.DataAnnotations;
namespace PR_Top_Service_MVC.Models
{
    public class CustomerUser
    {

        [Key]
        public int IdCostumer { get; set; }
        [Required(ErrorMessage = "La dirección es requerida.")]
        public string Address { get; set; } = null!;
        [Required(ErrorMessage = "El CI es requerido.")]
        public string Ci { get; set; }

        public int IdPerson { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "El apellido es requerido.")]
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; } = null!;
        public byte IdDepartment { get; set; }
        public virtual Department? IdDepartmentNavigation { get; set; } = null!;
        public byte status { get; set; }


        public int IdUser { get; set; }
        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
