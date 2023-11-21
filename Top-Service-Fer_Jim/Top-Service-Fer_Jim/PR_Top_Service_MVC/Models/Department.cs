using System;
using System.Collections.Generic;

namespace PR_Top_Service_MVC.Models
{
    public partial class Department
    {
        public Department()
        {
            People = new HashSet<Person>();
        }

        public byte IdDepartment { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Person> People { get; set; }
    }
}
