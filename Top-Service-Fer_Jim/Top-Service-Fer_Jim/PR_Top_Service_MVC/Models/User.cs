using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Models
{
    public partial class User
    {
        [Key]
        public int IdUser { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public string? Role { get; set; } = null!;
        public virtual Person IdUserNavigation { get; set; } = null!;
    }
}
