using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Security.Cryptography;

namespace PR_Top_Service_MVC.Controllers
{
  

    public class LoginController : Controller
	{
		public readonly TopServiceBDOContext _db;

		public LoginController(TopServiceBDOContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
            ViewData["alert"] = "hidden";
            return View();
		}
        public bool ValidatePassword(string enteredPassword, string storedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] enteredPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword);
                byte[] enteredPasswordHash = sha256.ComputeHash(enteredPasswordBytes);
                string enteredPasswordHashString = Convert.ToBase64String(enteredPasswordHash);

                // Comparar los hashes
                return enteredPasswordHashString.Equals(storedPassword);
            }
        }
        [HttpPost]
		public async Task<IActionResult> Index(User user)
		{
			var usdb = (await _db.Users.ToListAsync());

			foreach (var u in usdb)
			{
                if (u.Email.Equals(user.Email))
                {
                    bool isPasswordValid = ValidatePassword(user.Password, u.Password);

                    if (isPasswordValid)
                    {
                        // Resto del código...
                        UserConfig.userLogin = u;
                        string rol = u.Role.ToString();
                        Debug.WriteLine(rol);
                        var claims = new List<Claim>()
                            {
                                new Claim(ClaimTypes.Name, "User"),
                                new Claim(ClaimTypes.Email, u.Email),
                                new Claim(ClaimTypes.Role, rol)
                            };

                        var claimsId = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsId));

                        return Redirect("/");
                    }

                }
            }
            ViewData["alert"] = "";
            return View();
		}

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
