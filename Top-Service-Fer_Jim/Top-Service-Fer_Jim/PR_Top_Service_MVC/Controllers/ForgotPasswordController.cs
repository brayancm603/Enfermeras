using Microsoft.AspNetCore.Mvc;
using PR_Top_Service_MVC.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;

namespace PR_Top_Service_MVC.Controllers
{
   
    public class ForgotPasswordController : Controller
    {
        private readonly TopServiceBDOContext _context;
        Random _random = new Random();

        public ForgotPasswordController(TopServiceBDOContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMail(string typeEmail, int randomCode)
        {
            try
            {
                // Configuración de correo
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("jperezlopez7895@gmail.com", "xzzzbjpxknkojzvu");

                // Crear un mensaje de correo
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("jperezlopez7895@gmail.com");
                mailMessage.To.Add(typeEmail);
                mailMessage.Subject = "Olvidó su contraseña";
                mailMessage.Body = "Parece que intenta cambiar su contraseña. " +
                                  "Ingrese este código en el formulario " + randomCode +
                                  " Una vez se verifique que realmente es usted, podrá cambiar su contraseña.";

                // Envía el correo
                smtpClient.Send(mailMessage);

                // Envío exitoso
                return Ok();
            }
            catch (Exception ex)
            {
                // Error al enviar el correo
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetUserIdByEmail(string email)
        {
            // Realiza una consulta en la base de datos para encontrar el ID del usuario
            var user = _context.Users.FirstOrDefault(x => x.Email == email);


            if (user != null)
            {
                // Devuelve el ID como un valor JSON
                return Json(user.IdUser);
            }
            else
            {
                // Devuelve un error si no se encuentra el usuario
                return BadRequest("Usuario no encontrado");
            }
        }


        public async Task<IActionResult> UpdatePasswordForgot(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePasswordForgot(int id, [Bind("IdUser, Password")] User user)
        {
            // Busca el usuario en la base de datos
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser != null)
            {
                existingUser.Password = HashPassword(user.Password);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

 
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hashedBytes);
                return hashedPassword;
            }
        }

    }
}
