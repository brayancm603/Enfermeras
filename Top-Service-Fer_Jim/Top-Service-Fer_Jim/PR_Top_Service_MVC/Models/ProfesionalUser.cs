using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Models
{
    public class ProfesionalUser
    {
        [Key]
        public int IdProfesional { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public DateTime Birthdate { get; set; }


        public int IdPerson { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? SecondLastName { get; set; } = null!;
        public byte IdDepartment { get; set; }
        public virtual Department? IdDepartmentNavigation { get; set; } = null!;
        public byte status { get; set; }
        public byte Deleted { get; set; }



        public int IdUser { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
