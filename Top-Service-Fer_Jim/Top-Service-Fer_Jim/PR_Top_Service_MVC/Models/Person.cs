using System;
using System.Collections.Generic;

namespace PR_Top_Service_MVC.Models
{
    public partial class Person
    {
        public int IdPerson { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string SecondLastName { get; set; } = null!;
        public byte IdDepartment { get; set; }
        public byte status { get; set; }
        public byte deleted { get; set; }
        public virtual Department IdDepartmentNavigation { get; set; } = null!;
        public virtual Admin? Admin { get; set; }
        public virtual Costumer? Costumer { get; set; }
        public virtual Profesional? Profesional { get; set; }
        public virtual User? User { get; set; }
    }
}
