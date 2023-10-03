using Microsoft.AspNetCore.Authorization;
using PR_Top_Service_MVC.Models;
using System.ComponentModel.DataAnnotations;

namespace PR_Top_Service_MVC.Controllers
{


    [Authorize]
    public static class UserConfig
    {
        private static User _user;

        public static User userLogin { 
            get { return _user; }
            set {
                _user = value;
            }
        }

    }
}
