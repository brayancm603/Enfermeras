using System.ComponentModel.DataAnnotations;
namespace PR_Top_Service_MVC.Models
{
    public class PersonAdmin
    {
        [Key]
        public int IdAdmin { get; set; }
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de teléfono debe tener exactamente 8 dígitos")]
        public string? CelphoneNumber { get; set; } = null!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime Birthdate { get; set; }


        public byte deleted { get; set; }
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
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        [RegularExpression(@"\w+@gmail.com", ErrorMessage = "El correo electrónico debe tener la extensión @gmail.com")]
        public string Email { get; set; } = null!;

        public string? Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
